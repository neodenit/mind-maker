using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.GPT3.Models;
using OpenAI_API;

namespace Neodenit.MindMaker.Services.GPT3
{
    public class GetAdvice
    {
        private readonly ISettings settings;

        public GetAdvice(ISettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        [Function(nameof(GetAdvice))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(GetAdvice));
            logger.LogInformation(nameof(GetAdvice));

            try
            {
                var request = await JsonSerializer.DeserializeAsync<AdviceRequestDTO>(req.Body);

                var nodeSeparator = FixString(settings.NodeSeparator);
                var blockSeparator = FixString(settings.BlockSeparator);

                IEnumerable<IEnumerable<string>> branches = GetBranches(request.Root);
                var textBranches = branches.Select(b => string.Join(nodeSeparator, b));
                var context = string.Join(settings.BlockSeparator, textBranches);
                var prompt = string.Join(nodeSeparator, request.Parents);

                var fullPrompt = context + settings.BlockSeparator + prompt + nodeSeparator;

                var stopSequences = settings.StopSequences.Select(s => FixString(s)).ToArray();

                OpenAIAPI api = new(APIAuthentication.LoadFromEnv(), new Engine(settings.Engine));

                CompletionResult completionResult = await api.Completions.CreateCompletionAsync(fullPrompt, settings.MaxTokens, settings.Temperature, numOutputs: settings.NumOutputs, stopSequences: stopSequences);

                var resuts = completionResult.Completions.Select(c => c.Text.Trim()).Distinct();

                LogRequest(logger, fullPrompt, resuts, stopSequences);

                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(resuts);
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private readonly Dictionary<string, string> Replacements = new()
        {
            { "\\n", "\n" },
            { "\\s", " " }
        };

        private string FixString(string s) =>
            Replacements.Aggregate(s, (acc, item) => acc.Replace(item.Key, item.Value));

        private IEnumerable<IEnumerable<string>> GetBranches(NodeDTO node) =>
            GetBranches(node, Enumerable.Empty<string>());

        private void LogRequest(ILogger logger, string request, IEnumerable<string> response, IEnumerable<string> stopSequences)
        {
            logger.LogInformation(
                JsonSerializer.Serialize(
                    new
                    {
                        Function = nameof(GetAdvice),
                        settings.Engine,
                        settings.Temperature,
                        settings.MaxTokens,
                        StopSequences = stopSequences,
                        Request = request,
                        Response = response
                    },
                    new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    }));
        }

        private IEnumerable<IEnumerable<string>> GetBranches(NodeDTO node, IEnumerable<string> acc)
        {
            var newAcc = acc.Append(node.Name);

            if (node.Children.Any())
            {
                foreach (var subNode in node.Children)
                {
                    var branches = GetBranches(subNode, newAcc);

                    foreach (var branch in branches)
                    {
                        yield return branch;
                    }
                }
            }
            else
            {
                yield return newAcc;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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

            var request = await JsonSerializer.DeserializeAsync<AdviceRequestDTO>(req.Body);

            IEnumerable<IEnumerable<string>> branches = GetBranches(request.Root);
            var textBranches = branches.Select(b => string.Join(settings.NodeSeparator, b));
            var context = string.Join(settings.BlockSeparator, textBranches);
            var prompt = string.Join(settings.NodeSeparator, request.Parents);

            var fullPrompt = context + settings.BlockSeparator + prompt + settings.NodeSeparator;

            OpenAIAPI api = new(APIAuthentication.LoadFromEnv(), new Engine(settings.Engine));

            CompletionResult completionResult = await api.Completions.CreateCompletionAsync(fullPrompt, settings.MaxTokens, settings.Temperature, numOutputs: settings.NumOutputs, stopSequences: settings.StopSequences);

            var resuts = completionResult.Completions.Select(c => c.Text);

            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(resuts);
            return response;
        }

        private IEnumerable<IEnumerable<string>> GetBranches(NodeDTO node) =>
            GetBranches(node, Enumerable.Empty<string>());

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

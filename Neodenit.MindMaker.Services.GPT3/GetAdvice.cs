using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.GPT3.Converters;
using Neodenit.MindMaker.Services.GPT3.Models;
using OpenAI_API;

namespace Neodenit.MindMaker.Services.GPT3
{
    public class GetAdvice
    {
        private readonly IBranchConverter branchConverter;
        private readonly ISubBranchConverter subBranchConverter;
        private readonly ISimpleBranchConverter simpleBranchConverter;

        public GetAdvice(IBranchConverter branchConverter, ISubBranchConverter subBranchConverter, ISimpleBranchConverter simpleBranchConverter)
        {
            this.branchConverter = branchConverter ?? throw new ArgumentNullException(nameof(branchConverter));
            this.subBranchConverter = subBranchConverter ?? throw new ArgumentNullException(nameof(subBranchConverter));
            this.simpleBranchConverter = simpleBranchConverter ?? throw new ArgumentNullException(nameof(simpleBranchConverter));
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
                var response = req.CreateResponse();

                var converter = GetConverter(request.Mode);
                var openAIRequest = converter.GetParameters(request.Root, request.Parents);
                var parameters = openAIRequest.Params;

                OpenAIAPI api = new(APIAuthentication.LoadFromEnv(), new Engine(openAIRequest.Params.Engine));

                CompletionResult completionResult = await api.Completions.CreateCompletionAsync(openAIRequest.Prompt, parameters.MaxTokens, parameters.Temperature, parameters.TopP, parameters.NumOutputs, parameters.PresencePenalty, parameters.FrequencyPenalty, stopSequences: parameters.StopSequences);

                var resuts = completionResult.Completions.Select(c => c.Text.Trim()).Distinct();

                LogRequest(logger, openAIRequest, resuts);

                await response.WriteAsJsonAsync(resuts);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private IConverter GetConverter(ConverterType converterType) =>
            converterType switch
            {
                ConverterType.Branch => branchConverter,
                ConverterType.SubBranch => subBranchConverter,
                ConverterType.SimpleBranch => simpleBranchConverter,
                _ => throw new NotImplementedException()
            };

        private static void LogRequest(ILogger logger, OpenAIRequest settings, IEnumerable<string> response)
        {
            logger.LogInformation(
                JsonSerializer.Serialize(
                    new
                    {
                        Function = nameof(GetAdvice),
                        settings.Params.Engine,
                        settings.Params.Temperature,
                        settings.Params.TopP,
                        settings.Params.MaxTokens,
                        settings.Params.StopSequences,
                        Request = settings.Prompt,
                        Response = response
                    },
                    new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    }));
        }
    }
}

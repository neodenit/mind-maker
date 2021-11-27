using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.GPT3.Converters;
using Neodenit.MindMaker.Services.GPT3.Helpers;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3
{
    public class GetAdvice
    {
        private readonly IBranchConverter branchConverter;
        private readonly ISubBranchConverter subBranchConverter;
        private readonly ISimpleBranchConverter simpleBranchConverter;
        private readonly ISimpleSubBranchConverter simpleSubBranchConverter;

        public GetAdvice(
            IBranchConverter branchConverter,
            ISubBranchConverter subBranchConverter,
            ISimpleBranchConverter simpleBranchConverter,
            ISimpleSubBranchConverter simpleSubBranchConverter)
        {
            this.branchConverter = branchConverter ?? throw new ArgumentNullException(nameof(branchConverter));
            this.subBranchConverter = subBranchConverter ?? throw new ArgumentNullException(nameof(subBranchConverter));
            this.simpleBranchConverter = simpleBranchConverter ?? throw new ArgumentNullException(nameof(simpleBranchConverter));
            this.simpleSubBranchConverter = simpleSubBranchConverter ?? throw new ArgumentNullException(nameof(simpleSubBranchConverter));
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
                openAIRequest.Params.TopP = request.Randomness;

                IEnumerable<string> results = await GPT3Helper.GetGPT3Completion(openAIRequest);

                LogRequest(logger, openAIRequest, results);

                await response.WriteAsJsonAsync(results);

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
                ConverterType.SimpleSubBranch => simpleSubBranchConverter,
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

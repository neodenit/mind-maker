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
using Neodenit.MindMaker.Services.MindMapping.Models;

namespace Neodenit.MindMaker.Services.GPT3
{
    public class GetAdvice
    {
        private readonly IBranchConverter branchConverter;
        private readonly ISubBranchConverter subBranchConverter;
        private readonly ISimpleBranchConverter simpleBranchConverter;
        private readonly ISimpleSubBranchConverter simpleSubBranchConverter;
        private readonly IParentChildConverter parentChildConverter;
        private readonly IHuggingFaceHelper huggingFaceHelper;

        public GetAdvice(IBranchConverter branchConverter,
                         ISubBranchConverter subBranchConverter,
                         ISimpleBranchConverter simpleBranchConverter,
                         ISimpleSubBranchConverter simpleSubBranchConverter,
                         IParentChildConverter parentChildConverter,
                         IParentChildrenConverter parentChildrenConverter,
                         IHuggingFaceHelper huggingFaceHelper)
        {
            this.branchConverter = branchConverter ?? throw new ArgumentNullException(nameof(branchConverter));
            this.subBranchConverter = subBranchConverter ?? throw new ArgumentNullException(nameof(subBranchConverter));
            this.simpleBranchConverter = simpleBranchConverter ?? throw new ArgumentNullException(nameof(simpleBranchConverter));
            this.simpleSubBranchConverter = simpleSubBranchConverter ?? throw new ArgumentNullException(nameof(simpleSubBranchConverter));
            this.parentChildConverter = parentChildConverter ?? throw new ArgumentNullException(nameof(parentChildConverter));
            this.parentChildrenConverter = parentChildrenConverter ?? throw new ArgumentNullException(nameof(parentChildrenConverter));
            this.huggingFaceHelper = huggingFaceHelper ?? throw new ArgumentNullException(nameof(huggingFaceHelper));
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
                var externalRequest = converter.GetParameters(request.Root, request.Parents);
                externalRequest.Params.TopP = request.Randomness;
                externalRequest.Engine = request.Engine;

                IEnumerable<string> results =
                    request.Engine == Engine.GPT3
                        ? await GPT3Helper.GetCompletion(externalRequest)
                        : await huggingFaceHelper.GetCompletion(externalRequest, logger);

                LogRequest(logger, externalRequest, results);

                await response.WriteAsJsonAsync(results);

                PromptSettings promptSettings = new()
                {
                    Id = request.Owner,
                    Engine = request.Engine,
                    Mode = request.Mode,
                    Randomness = request.Randomness
                };

                return new MultiResponse { PromptSettings = promptSettings, HttpResponse = response };
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
                ConverterType.ParentChildConverter => parentChildConverter,
                ConverterType.ParentChildrenConverter => parentChildrenConverter,
                _ => throw new NotImplementedException()
            };

        private static void LogRequest(ILogger logger, ExternalRequest settings, IEnumerable<string> response)
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

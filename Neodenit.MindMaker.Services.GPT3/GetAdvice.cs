using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.GPT3.Models;
using OpenAI_API;
using SmartListAdviceService;

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
            var prompt = string.Join(Constants.NewLine, request.Parents) + Constants.NewLine;

            OpenAIAPI api = new (APIAuthentication.LoadFromEnv(), new Engine(settings.Engine));

            CompletionResult completionResult = await api.Completions.CreateCompletionAsync(prompt, settings.MaxTokens, settings.Temperature, numOutputs: settings.NumOutputs, stopSequences: settings.StopSequences);

            var resuts = completionResult.Completions.Select(c => c.Text);

            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(resuts);
            return response;
        }
    }
}

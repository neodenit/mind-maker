using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Neodenit.MindMaker.Services.GPT3.Models;
using Newtonsoft.Json;

namespace Neodenit.MindMaker.Services.GPT3.Helpers
{
    public class HuggingFaceHelper : IHuggingFaceHelper
    {
        private readonly HttpClient httpClient;
        private readonly Settings settings;

        public HuggingFaceHelper(HttpClient httpClient, Settings settings)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<IEnumerable<string>> GetCompletion(ExternalRequest request)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("HF_KEY"));

            var parameters = request.Params;

            var prompt = new HFRequest
            {
                Inputs = request.Prompt,
                Parameters = new HFRequestParameters
                {
                    MaxNewTokens = parameters.MaxTokens,
                    TopP = parameters.TopP,
                    NumReturnSequences = parameters.NumOutputs,
                    NumBeams = settings.NumBeams,
                    ReturnFullText = false
                },
                Options = new HFRequestOptions { WaitForModel = true }
            };

            var url = GetEngineUrl(request.Engine);
            var json = JsonConvert.SerializeObject(prompt);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);

            var model = await response.Content.ReadFromJsonAsync<IEnumerable<HFResponse>>();

            var result = model.Select(x => GetText(request, x)).Where(x => !string.IsNullOrEmpty(x)).Distinct();
            return result;
        }

        private string GetEngineUrl(Engine engine) =>
            engine switch
            {
                Engine.GPT2 => settings.Urls.GPT2,
                Engine.ruGPT => settings.Urls.RuGPT,
                _ => throw new NotImplementedException()
            };

        private static string GetText(ExternalRequest request, HFResponse completion)
        {
            var stopWordIndexes = request.Params.StopSequences.Select(s => completion.generated_text.IndexOf(s));
            var minIndex = stopWordIndexes.Min();
            var result = minIndex > -1 ? completion.generated_text.Substring(0, minIndex) : completion.generated_text;
            return result.Trim();
        }
    }
}

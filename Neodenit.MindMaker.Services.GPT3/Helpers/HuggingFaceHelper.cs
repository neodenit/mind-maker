using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        public async Task<IEnumerable<string>> GetCompletion(ExternalRequest request, ILogger logger = null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("HF_KEY"));

            var parameters = request.Params;

            var prompt = new HFRequest
            {
                Inputs = request.Prompt,
                Parameters = new HFRequestParameters
                {
                    MaxNewTokens = parameters.MaxTokens,
                    DoSample = parameters.DoSample,
                    TopP = parameters.TopP,
                    NumReturnSequences = parameters.NumOutputs,
                    NumBeams = settings.NumBeams,
                    ReturnFullText = false
                },
                Options = new HFRequestOptions { WaitForModel = true }
            };

            var url = GetEngineUrl(request.Engine);
            var requestJson = JsonConvert.SerializeObject(prompt);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IEnumerable<HFResponse>>(responseJson);

                var result = model.Select(x => GetText(request, x)).Where(x => !string.IsNullOrEmpty(x)).Distinct();
                return result;
            }
            else
            {
                if (logger is not null)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    logger.LogError(responseJson);
                }

                return Enumerable.Empty<string>();
            }

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
            var stopWordIndexes = request.Params.StopSequences.Select(s => completion.GeneratedText.IndexOf(s));
            var minIndex = stopWordIndexes.Min();
            var result = minIndex > -1 ? completion.GeneratedText.Substring(0, minIndex) : completion.GeneratedText;
            return result.Trim();
        }
    }
}

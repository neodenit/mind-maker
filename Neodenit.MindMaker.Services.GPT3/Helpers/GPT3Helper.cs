using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.MindMaker.Services.GPT3.Models;
using OpenAI_API;

namespace Neodenit.MindMaker.Services.GPT3.Helpers
{
    public static class GPT3Helper
    {
        public static async Task<IEnumerable<string>> GetCompletion(ExternalRequest openAIRequest)
        {
            var parameters = openAIRequest.Params;

            OpenAIAPI api = new(APIAuthentication.LoadFromEnv(), new OpenAI_API.Engine(openAIRequest.Params.Engine));

            CompletionResult completionResult = await api.Completions.CreateCompletionAsync(openAIRequest.Prompt, parameters.MaxTokens, parameters.Temperature, parameters.TopP, parameters.NumOutputs, parameters.PresencePenalty, parameters.FrequencyPenalty, stopSequences: parameters.StopSequences);

            var results = completionResult.Completions.Select(c => c.Text.Trim()).Distinct();
            return results;
        }
    }
}

using Newtonsoft.Json;

namespace Neodenit.MindMaker.Services.GPT3.Models
{
    public class HFResponse
    {
        [JsonProperty("generated_text")]
        public string GeneratedText { get; set; }
    }
}

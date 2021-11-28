using Newtonsoft.Json;

namespace Neodenit.MindMaker.Services.GPT3.Models
{
    public class HFRequest
    {
        [JsonProperty("inputs")]
        public string Inputs { get; set; }

        [JsonProperty("parameters")]
        public HFRequestParameters Parameters { get; set; }

        [JsonProperty("options")]
        public HFRequestOptions Options { get; set; }
    }
}

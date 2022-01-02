using Newtonsoft.Json;

namespace Neodenit.MindMaker.Services.GPT3.Models
{
    public class HFRequestParameters
    {
        [JsonProperty("do_sample")]
        public bool? DoSample { get; set; }

        [JsonProperty("top_k")]
        public int? TopK { get; set; }

        [JsonProperty("top_p")]
        public double? TopP { get; set; }

        [JsonProperty("temperature")]
        public double? Temperature { get; set; }

        [JsonProperty("repetition_penalty")]
        public double? RepetitionPenalty { get; set; }

        [JsonProperty("max_new_tokens")]
        public int MaxNewTokens { get; set; }

        [JsonProperty("num_beams")]
        public int? NumBeams { get; set; }

        [JsonProperty("return_full_text")]
        public bool ReturnFullText { get; set; }

        [JsonProperty("num_return_sequences")]
        public int NumReturnSequences { get; set; }
    }
}

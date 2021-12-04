using System.Text.Json.Serialization;

namespace Neodenit.MindMaker.Services.GPT3.Models
{
    public class PromptSettings
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public ConverterType Mode { get; set; }

        public Engine Engine { get; set; }

        public double Randomness { get; set; }
    }
}

namespace Neodenit.MindMaker.Services.GPT3
{
    public class Settings : ISettings
    {
        public string Engine { get; set; }

        public int MaxTokens { get; set; }

        public double Temperature { get; set; }

        public int NumOutputs { get; set; }

        public string[] StopSequences { get; set; }
    }
}

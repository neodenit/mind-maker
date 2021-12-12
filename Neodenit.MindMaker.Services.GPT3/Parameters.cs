namespace Neodenit.MindMaker.Services.GPT3
{
    public class Parameters
    {
        public string Engine { get; set; }

        public int MaxTokens { get; set; }

        public double Temperature { get; set; }

        public double TopP { get; set; }

        public int NumOutputs { get; set; }

        public double PresencePenalty { get; set; }

        public double FrequencyPenalty { get; set; }

        public string[] StopSequences { get; set; }

        public string PromptStart { get; set; }

        public string NodeSeparator { get; set; }

        public string BlockSeparator { get; set; }

        public string PromptEnd { get; set; }

        public string ChildBlockStart { get; set; }

        public string ChildSeparator { get; set; }

        public Parameters Clone() => MemberwiseClone() as Parameters;
    }
}

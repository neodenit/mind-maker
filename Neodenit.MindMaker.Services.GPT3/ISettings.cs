namespace Neodenit.MindMaker.Services.GPT3
{
    public interface ISettings
    {
        string Engine { get; set; }

        int MaxTokens { get; set; }

        int NumOutputs { get; set; }

        string[] StopSequences { get; set; }

        double Temperature { get; set; }

        string NodeSeparator { get; set; }

        string BlockSeparator { get; set; }
    }
}
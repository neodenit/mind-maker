using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3
{
    public static class Constants
    {
        public const string NewLine = "\n";

        public const string DatabaseId = "MindMakerDB";

        public const string ContainerId = "Settings";

        public const string PartitionKey = "/id";

        public const double DefaultRandomness = 0.5;

        public const ConverterType DefaultConverterType = ConverterType.Branch;

        public const Engine DefaultEngine = Engine.GPT3;
    }
}

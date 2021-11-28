namespace Neodenit.MindMaker.Services.GPT3
{
    public class Settings
    {
        public Parameters Default { get; set; }

        public Parameters Branches { get; set; }

        public Parameters SubBranches { get; set; }

        public Parameters SimpleBranches { get; set; }

        public Parameters SimpleSubBranches { get; set; }

        public Urls Urls { get; set; }

        public int NumBeams { get; set; }
    }
}

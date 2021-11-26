using System.Collections.Generic;

namespace Neodenit.MindMaker.Web.Shared
{
    public static class ModeList
    {
        public static IEnumerable<(Mode mode, string label)> Modes { get; set; } = new[]
        {
            (Mode.Branch, "Branches"),
            (Mode.SubBranch, "Sub-Branches"),
            (Mode.SimpleBranch, "Simple Branches"),
            (Mode.SimpleSubBranch, "Simple Sub-Branches")
        };
    }
}

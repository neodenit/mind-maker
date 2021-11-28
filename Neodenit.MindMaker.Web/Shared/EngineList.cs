using System.Collections.Generic;

namespace Neodenit.MindMaker.Web.Shared
{
    public static class EngineList
    {
        public static IEnumerable<(Engine engine, string label)> Items { get; set; } = new[]
        {
            (Engine.GPT3, "GPT-3"),
            (Engine.GPT2, "GPT-2"),
            (Engine.ruGPT, "ruGPT")
        };
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Neodenit.MindMaker.Services.GPT3.Helpers
{
    public static class StringHelper
    {
        private static readonly Dictionary<string, string> Replacements = new()
        {
            { "\\n", "\n" },
            { "\\s", " " }
        };

        public static string FixString(string s) =>
            Replacements.Aggregate(s, (acc, item) => acc.Replace(item.Key, item.Value));
    }
}

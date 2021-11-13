using System.Collections.Generic;
using System.Linq;

namespace Neodenit.MindMaker.Services.GPT3.Helpers
{
    public static class BlockPromptHelper
    {
        public static string GetPrompt(IEnumerable<IEnumerable<string>> contextBlocks, IEnumerable<string> promptBlocks, Parameters parameters)
        {
            var textBranches = contextBlocks.Select(b => string.Join(parameters.NodeSeparator, b));
            var context = string.Join(parameters.BlockSeparator, textBranches);
            var prompt = string.Join(parameters.NodeSeparator, promptBlocks);

            var fullPrompt = parameters.PromptStart + context + parameters.BlockSeparator + prompt + parameters.PromptEnd;
            return fullPrompt;
        }
    }
}

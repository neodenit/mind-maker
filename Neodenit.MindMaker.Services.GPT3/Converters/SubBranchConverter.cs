using System;
using System.Collections.Generic;
using System.Linq;
using Neodenit.MindMaker.Services.GPT3.Helpers;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Converters
{
    public class SubBranchConverter : ISubBranchConverter
    {
        private readonly Settings settings;

        public SubBranchConverter(Settings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public Request GetParameters(NodeDTO node, IEnumerable<string> parents)
        {
            var promptStart = StringHelper.FixString(settings.SubBranches.PromptStart);
            var promptEnd = StringHelper.FixString(settings.SubBranches.PromptEnd);
            var nodeSeparator = StringHelper.FixString(settings.SubBranches.NodeSeparator);
            var blockSeparator = StringHelper.FixString(settings.SubBranches.BlockSeparator);

            IEnumerable<IEnumerable<string>> branches = GetSubBranches(node);

            var textBranches = branches.Select(b => string.Join(nodeSeparator, b));
            var context = string.Join(blockSeparator, textBranches);
            var prompt = string.Join(nodeSeparator, parents);

            var fullPrompt = promptStart + context + blockSeparator + prompt + promptEnd;

            var parameters = settings.Default.Clone();

            parameters.StopSequences = settings.SubBranches.StopSequences.Select(s => StringHelper.FixString(s)).ToArray();

            return new Request { Prompt = fullPrompt, Params = parameters };
        }

        public static IEnumerable<IEnumerable<string>> GetSubBranches(NodeDTO node) =>
            GetSubBranches(node, Enumerable.Empty<string>());

        private static IEnumerable<IEnumerable<string>> GetSubBranches(NodeDTO node, IEnumerable<string> acc)
        {
            var newAcc = acc.Append(node.Name);

            if (newAcc.Count() > 1)
            {
                yield return newAcc;
            }

            if (node.Children.Any())
            {
                foreach (var subNode in node.Children)
                {
                    var branches = GetSubBranches(subNode, newAcc);

                    foreach (var branch in branches)
                    {
                        yield return branch;
                    }
                }
            }
        }
    }
}

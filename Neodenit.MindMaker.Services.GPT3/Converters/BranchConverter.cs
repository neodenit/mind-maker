using System;
using System.Collections.Generic;
using System.Linq;
using Neodenit.MindMaker.Services.GPT3.Helpers;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Converters
{
    public class BranchConverter : IBranchConverter
    {
        private readonly Settings settings;

        public BranchConverter(Settings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public Request GetParameters(NodeDTO node, IEnumerable<string> parents)
        {
            var promptStart = StringHelper.FixString(settings.Branches.PromptStart);
            var promptEnd = StringHelper.FixString(settings.Branches.PromptEnd);
            var nodeSeparator = StringHelper.FixString(settings.Branches.NodeSeparator);
            var blockSeparator = StringHelper.FixString(settings.Branches.BlockSeparator);

            IEnumerable<IEnumerable<string>> branches = GetBranches(node);

            var textBranches = branches.Select(b => string.Join(nodeSeparator, b));
            var context = string.Join(blockSeparator, textBranches);
            var prompt = string.Join(nodeSeparator, parents);

            var fullPrompt = promptStart + context + blockSeparator + prompt + promptEnd;

            var parameters = settings.Default.Clone();

            parameters.TopP = settings.Branches.TopP;
            parameters.StopSequences = settings.Branches.StopSequences.Select(s => StringHelper.FixString(s)).ToArray();

            return new Request { Prompt = fullPrompt, Params = parameters };
        }


        public static IEnumerable<IEnumerable<string>> GetBranches(NodeDTO node) =>
            GetBranches(node, Enumerable.Empty<string>());

        private static IEnumerable<IEnumerable<string>> GetBranches(NodeDTO node, IEnumerable<string> acc)
        {
            var newAcc = acc.Append(node.Name);

            if (node.Children.Any())
            {
                foreach (var subNode in node.Children)
                {
                    var branches = GetBranches(subNode, newAcc);

                    foreach (var branch in branches)
                    {
                        yield return branch;
                    }
                }
            }
            else
            {
                yield return newAcc;
            }
        }
    }
}

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
            Parameters parameters = ParametersHelper.GetParameters(settings.Default, settings.Branches);

            IEnumerable<IEnumerable<string>> branches = GetSubBranches(node);

            var textBranches = branches.Select(b => string.Join(parameters.NodeSeparator, b));
            var context = string.Join(parameters.BlockSeparator, textBranches);
            var prompt = string.Join(parameters.NodeSeparator, parents);

            var fullPrompt = parameters.PromptStart + context + parameters.BlockSeparator + prompt + parameters.PromptEnd;

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

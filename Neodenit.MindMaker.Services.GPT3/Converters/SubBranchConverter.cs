using System;
using System.Collections.Generic;
using System.Linq;
using Neodenit.MindMaker.Services.GPT3.Helpers;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Converters
{
    public class SubBranchConverter : ISubBranchConverter
    {
        protected Settings settings;
        protected Parameters newParameters;

        public SubBranchConverter() { }

        public SubBranchConverter(Settings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            newParameters = settings.SubBranches ?? throw new ArgumentNullException(nameof(settings.SubBranches));
        }

        public OpenAIRequest GetParameters(NodeDTO node, IEnumerable<string> parents)
        {
            Parameters parameters = ParametersHelper.GetParameters(settings.Default, newParameters);

            IEnumerable<IEnumerable<string>> branches = GetSubBranches(node);

            string fullPrompt = BlockPromptHelper.GetPrompt(branches, parents, parameters);

            return new OpenAIRequest { Prompt = fullPrompt, Params = parameters };
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

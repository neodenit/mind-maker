using System;
using System.Collections.Generic;
using System.Linq;
using Neodenit.MindMaker.Services.GPT3.Helpers;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Converters
{
    public class BranchConverter : IBranchConverter
    {
        internal Settings settings;
        protected Parameters newParameters;

        public BranchConverter() { }

        public BranchConverter(Settings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            newParameters = settings.Branches ?? throw new ArgumentNullException(nameof(settings.Branches));
        }

        public OpenAIRequest GetParameters(NodeDTO node, IEnumerable<string> parents)
        {
            Parameters parameters = ParametersHelper.GetParameters(settings.Default, newParameters);

            IEnumerable<IEnumerable<string>> branches = GetBranches(node);

            string fullPrompt = BlockPromptHelper.GetPrompt(branches, parents, parameters);

            return new OpenAIRequest { Prompt = fullPrompt, Params = parameters };
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

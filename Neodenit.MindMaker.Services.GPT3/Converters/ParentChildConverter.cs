using System;
using System.Collections.Generic;
using System.Linq;
using Neodenit.MindMaker.Services.GPT3.Helpers;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Converters
{
    public class ParentChildConverter : IParentChildConverter
    {
        protected Settings settings;
        protected Parameters newParameters;

        public ParentChildConverter() { }

        public ParentChildConverter(Settings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            newParameters = settings.ParentChild ?? throw new ArgumentNullException(nameof(settings.ParentChild));
        }

        public ExternalRequest GetParameters(NodeDTO node, IEnumerable<string> parents)
        {
            Parameters parameters = ParametersHelper.GetParameters(settings.Default, newParameters);

            IEnumerable<IEnumerable<string>> pairs = GetPairs(node);

            var lastParent = parents.Last();

            string fullPrompt = BlockPromptHelper.GetPrompt(pairs, new[] { lastParent }, parameters);

            return new ExternalRequest { Prompt = fullPrompt, Params = parameters };
        }

        public static IEnumerable<IEnumerable<string>> GetPairs(NodeDTO node) =>
            GetPairs(node, Enumerable.Empty<IEnumerable<string>>());

        private static IEnumerable<IEnumerable<string>> GetPairs(NodeDTO node, IEnumerable<IEnumerable<string>> acc)
        {
            if (node.Children.Any())
            {
                foreach (var subNode in node.Children)
                {
                    var newAcc = acc.Append(new[] { node.Name, subNode.Name });

                    var pairs = GetPairs(subNode, newAcc);

                    foreach (var pair in pairs)
                    {
                        yield return pair;
                    }
                }
            }
            else
            {
                foreach (var pair in acc)
                {
                    yield return pair;
                }
            }
        }
    }
}

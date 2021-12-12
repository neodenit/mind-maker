using System;
using System.Collections.Generic;
using System.Linq;
using Neodenit.MindMaker.Services.GPT3.Helpers;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Converters
{
    public class ParentChildrenConverter : IParentChildrenConverter
    {
        protected Settings settings;
        private readonly INodeHelper nodeHelper;
        protected Parameters newParameters;

        public ParentChildrenConverter() { }

        public ParentChildrenConverter(Settings settings, INodeHelper nodeHelper)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.nodeHelper = nodeHelper ?? throw new ArgumentNullException(nameof(nodeHelper));
            newParameters = settings.ParentChildren ?? throw new ArgumentNullException(nameof(settings.ParentChildren));
        }

        public ExternalRequest GetParameters(NodeDTO node, IEnumerable<string> parents)
        {
            Parameters parameters = ParametersHelper.GetParameters(settings.Default, newParameters);

            IEnumerable<IEnumerable<string>> blocks = GetBlocks(node);
            var markedBlocks = blocks.Select(b => GetMarkedBlocks(b, parameters));
            IEnumerable<string> prompt = GetPrompt(node, parents, parameters);

            string fullPrompt = BlockPromptHelper.GetPrompt(markedBlocks, prompt, parameters);

            return new ExternalRequest { Prompt = fullPrompt, Params = parameters };
        }

        private IEnumerable<string> GetPrompt(NodeDTO node, IEnumerable<string> parents, Parameters parameters)
        {
            var path = parents.Skip(1);
            var targetNode = nodeHelper.GetNode(node, path);
            var blocks = targetNode.Children.Select(c => c.Name).Prepend(targetNode.Name);
            var markedBlocks = GetMarkedBlocks(blocks, parameters);
            return markedBlocks;
        }

        private static IEnumerable<string> GetMarkedBlocks(IEnumerable<string> blocks, Parameters parameters)
        {
            yield return blocks.First() + parameters.ChildBlockStart;

            var rest = blocks.Skip(1);

            foreach (var item in rest)
            {
                yield return parameters.ChildSeparator + item;
            }
        }

        private static IEnumerable<IEnumerable<string>> GetBlocks(NodeDTO node) =>
            GetBlocks(node, Enumerable.Empty<IEnumerable<string>>());

        private static IEnumerable<IEnumerable<string>> GetBlocks(NodeDTO node, IEnumerable<IEnumerable<string>> acc)
        {
            if (node.Children.Any())
            {
                var newAcc = acc.Append(node.Children.Select(c => c.Name).Prepend(node.Name));

                foreach (var subNode in node.Children)
                {
                    var blocks = GetBlocks(subNode, newAcc);

                    foreach (var block in blocks)
                    {
                        yield return block;
                    }
                }
            }
            else
            {
                foreach (var block in acc)
                {
                    yield return block;
                }
            }
        }
    }
}

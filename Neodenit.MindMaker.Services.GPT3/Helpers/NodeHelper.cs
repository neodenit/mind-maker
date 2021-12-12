using System.Collections.Generic;
using System.Linq;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Helpers
{
    public class NodeHelper : INodeHelper
    {
        public NodeDTO GetNode(NodeDTO node, IEnumerable<string> path)
        {
            if (path.Any())
            {
                var head = path.First();
                var tail = path.Skip(1);

                var nextNode = node.Children.Single(x => x.Name == head);
                return GetNode(nextNode, tail);
            }
            else
            {
                return node;
            }
        }
    }
}

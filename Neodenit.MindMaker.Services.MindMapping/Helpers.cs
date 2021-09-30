using System.Collections.Generic;
using System.Linq;
using Neodenit.MindMaker.Services.MindMapping.Models;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class Helpers : IHelpers
    {
        public Node GetNode(Node node, IEnumerable<string> path)
        {
            if (path.Any())
            {
                var head = path.First();
                var tail = path.Skip(1);

                var nextNode = node.Children.Single(x => x.Id == head);
                return GetNode(nextNode, tail);
            }
            else
            {
                return node;
            }
        }
    }
}

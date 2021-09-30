using System.Collections.Generic;
using Neodenit.MindMaker.Services.MindMapping.Models;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public interface IHelpers
    {
        Node GetNode(Node node, IEnumerable<string> path);
    }
}
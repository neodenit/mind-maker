using System.Collections.Generic;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Helpers
{
    public interface INodeHelper
    {
        NodeDTO GetNode(NodeDTO node, IEnumerable<string> path);
    }
}
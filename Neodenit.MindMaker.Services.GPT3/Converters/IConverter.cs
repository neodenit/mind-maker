using System.Collections.Generic;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Converters
{
    public interface IConverter
    {
        Request GetParameters(NodeDTO node, IEnumerable<string> parents);
    }
}
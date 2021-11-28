using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Helpers
{
    public interface IHuggingFaceHelper
    {
        Task<IEnumerable<string>> GetCompletion(ExternalRequest request);
    }
}
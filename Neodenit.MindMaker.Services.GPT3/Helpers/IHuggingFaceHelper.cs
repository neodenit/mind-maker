using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3.Helpers
{
    public interface IHuggingFaceHelper
    {
        Task<IEnumerable<string>> GetCompletion(ExternalRequest request, ILogger logger = null);
    }
}
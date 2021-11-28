using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.MindMaker.Web.Shared;

namespace Neodenit.MindMaker.Web.Server.Services
{
    public interface IAdviceService
    {
        Task<IEnumerable<string>> GetAdviceAsync(AdviceRequestDTO request);
    }
}

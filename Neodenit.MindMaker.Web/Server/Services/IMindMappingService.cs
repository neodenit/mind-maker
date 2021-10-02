using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.MindMaker.Web.Shared;

namespace Neodenit.MindMaker.Web.Server
{
    public interface IMindMappingService
    {
        Task<IEnumerable<NodeDTO>> GetMindMapsAsync(string owner);

        Task<NodeDTO> PostMindMapAsync(CreateMindMapRequestDTO request);

        Task<NodeDTO> PostNodeAsync(CreateNodeRequestDTO request);

        Task<NodeDTO> PutNodeAsync(UpdateNodeRequestDTO request);

        Task DeleteMindMapAsync(DeleteMindMapRequestDTO request);

        Task DeleteNodeAsync(DeleteNodeRequestDTO request);
    }
}
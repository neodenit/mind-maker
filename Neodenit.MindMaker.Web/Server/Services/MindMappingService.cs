using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Neodenit.MindMaker.Web.Shared;

namespace Neodenit.MindMaker.Web.Server
{
    public class MindMappingService : IMindMappingService
    {
        private readonly HttpClient httpClient;

        public MindMappingService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<NodeDTO>> GetMindMapsAsync(string owner)
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<NodeDTO>>($"api/GetMindMaps/{owner}/");
        }

        public async Task<NodeDTO> PostMindMapAsync(CreateMindMapRequestDTO request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/CreateMindMap", content);
            var mindMap = await response.Content.ReadFromJsonAsync<NodeDTO>();
            return mindMap;
        }

        public async Task<NodeDTO> PostNodeAsync(CreateNodeRequestDTO request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/CreateNode", content);
            var node = await response.Content.ReadFromJsonAsync<NodeDTO>();
            return node;
        }

        public async Task<NodeDTO> PutNodeAsync(UpdateNodeRequestDTO request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("api/UpdateNode", content);
            var node = await response.Content.ReadFromJsonAsync<NodeDTO>();
            return node;
        }

        public async Task DeleteMindMapAsync(DeleteMindMapRequestDTO request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/DeleteMindMap", content);
        }

        public async Task DeleteNodeAsync(DeleteNodeRequestDTO request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/DeleteNode", content);
        }
    }
}

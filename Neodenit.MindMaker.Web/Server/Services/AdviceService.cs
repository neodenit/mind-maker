using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Neodenit.MindMaker.Web.Shared;

namespace Neodenit.MindMaker.Web.Server.Services
{
    public class AdviceService : IAdviceService
    {
        private readonly HttpClient httpClient;

        public AdviceService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<string>> GetAdviceAsync(AdviceRequestDTO request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/GetAdvice", content);
            var advice = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            return advice;
        }
    }
}

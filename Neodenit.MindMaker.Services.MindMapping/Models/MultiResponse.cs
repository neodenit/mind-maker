using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Neodenit.MindMaker.Services.MindMapping.Models
{
    public class MultiResponse
    {
        [CosmosDBOutput(Constants.DatabaseId, Constants.ContainerId, ConnectionStringSetting = "ConnectionString")]
        public Node OutputNode { get; set; }

        public HttpResponseData HttpResponse { get; set; }
    }
}

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Neodenit.MindMaker.Services.GPT3;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.MindMapping.Models
{
    public class MultiResponse
    {
        [CosmosDBOutput(Constants.DatabaseId, Constants.ContainerId, ConnectionStringSetting = "ConnectionString")]
        public PromptSettings PromptSettings { get; set; }

        public HttpResponseData HttpResponse { get; set; }
    }
}

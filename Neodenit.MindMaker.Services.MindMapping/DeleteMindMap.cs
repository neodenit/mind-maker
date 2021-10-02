using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.MindMapping.Models;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class DeleteMindMap
    {
        private readonly CosmosClient cosmosClient;

        public DeleteMindMap(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
        }

        [Function(nameof(DeleteMindMap))]
        public async Task RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [CosmosDBInput(
                Constants.DatabaseId,
                Constants.ContainerId,
                ConnectionStringSetting = "ConnectionString",
                Id = "{" + nameof(DeleteMindMapRequestDTO.MindMapId) + "}",
                PartitionKey = "{" + nameof(DeleteMindMapRequestDTO.Owner) + "}")] Node mindMap,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(DeleteMindMap));
            logger.LogInformation(nameof(DeleteMindMap));

            var request = await JsonSerializer.DeserializeAsync<DeleteMindMapRequestDTO>(req.Body);

            if (mindMap.Owner != request.Owner)
            {
                throw new UnauthorizedAccessException();
            }

            var container = cosmosClient.GetContainer(Constants.DatabaseId, Constants.ContainerId);
            await container.DeleteItemAsync<Node>(mindMap.Id, new PartitionKey(mindMap.Owner));
        }
    }
}

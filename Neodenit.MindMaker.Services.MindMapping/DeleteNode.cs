using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.MindMapping.Models;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class DeleteNode
    {
        private readonly IHelpers helpers;

        public DeleteNode(IHelpers helpers, CosmosClient cosmosClient)
        {
            this.helpers = helpers ?? throw new ArgumentNullException(nameof(helpers));
        }

        [Function(nameof(DeleteNode))]
        [CosmosDBOutput(Constants.DatabaseId, Constants.ContainerId, ConnectionStringSetting = "ConnectionString")]
        public async Task<Node> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [CosmosDBInput(
                Constants.DatabaseId,
                Constants.ContainerId,
                ConnectionStringSetting = "ConnectionString",
                Id = "{" + nameof(DeleteNodeRequestDTO.RootId) + "}",
                PartitionKey = "{" + nameof(DeleteNodeRequestDTO.Owner) + "}")] Node root,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(DeleteNode));
            logger.LogInformation(nameof(DeleteNode));

            var request = await JsonSerializer.DeserializeAsync<DeleteNodeRequestDTO>(req.Body);

            if (root.Owner != request.Owner)
            {
                throw new UnauthorizedAccessException();
            }

            var parentPath = request.SubPath.Take(request.SubPath.Count() - 1);
            var nodeId = request.SubPath.Last();

            var parent = helpers.GetNode(root, parentPath);
            parent.Children = parent.Children.Where(c => c.Id != nodeId );

            return root;
        }
    }
}

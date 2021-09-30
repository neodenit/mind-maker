using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.MindMapping.Models;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class UpdateNode
    {
        private readonly IHelpers helpers;

        public UpdateNode(IHelpers helpers)
        {
            this.helpers = helpers ?? throw new ArgumentNullException(nameof(helpers));
        }

        [Function(nameof(UpdateNode))]
        public async Task<MultiResponse> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req,
            [CosmosDBInput(
                Constants.DatabaseId,
                Constants.ContainerId,
                ConnectionStringSetting = "ConnectionString",
                Id = "{" + nameof(UpdateNodeRequestDTO.RootId) + "}",
                PartitionKey = "{" + nameof(UpdateNodeRequestDTO.Owner) + "}")] Node root,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(UpdateNode));
            logger.LogInformation(nameof(UpdateNode));

            var request = await JsonSerializer.DeserializeAsync<UpdateNodeRequestDTO>(req.Body);

            if (root.Owner != request.Owner)
            {
                throw new UnauthorizedAccessException();
            }

            var node = helpers.GetNode(root, request.SubPath);
            node.Name = request.UpdatedNodeName;
            node.LastUpdateTime = DateTime.UtcNow;

            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(node);

            var result = new MultiResponse { OutputNode = node, HttpResponse = response };
            return result;
        }
    }
}

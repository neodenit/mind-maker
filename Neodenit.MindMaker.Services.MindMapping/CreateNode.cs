using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.MindMapping.Models;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class CreateNode
    {
        private readonly IHelpers helpers;

        public CreateNode(IHelpers helpers)
        {
            this.helpers = helpers ?? throw new ArgumentNullException(nameof(helpers));
        }

        [Function(nameof(CreateNode))]
        public async Task<MultiResponse> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [CosmosDBInput(
                Constants.DatabaseId,
                Constants.ContainerId,
                ConnectionStringSetting = "ConnectionString",
                Id = "{" + nameof(CreateNodeRequestDTO.RootId) + "}",
                PartitionKey = "{" + nameof(CreateNodeRequestDTO.Owner) + "}")] Node root,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(CreateNode));
            logger.LogInformation(nameof(CreateNode));

            var request = await JsonSerializer.DeserializeAsync<CreateNodeRequestDTO>(req.Body);

            if (root.Owner != request.Owner)
            {
                throw new UnauthorizedAccessException();
            }

            var newNode = new Node
            {
                CreationTime = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Name = request.NewNodeName,
                Owner = request.Owner,
            };

            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(newNode);

            var parent = helpers.GetNode(root, request.SubPath);
            parent.Children = parent.Children.Append(newNode);

            var result = new MultiResponse { OutputNode = root, HttpResponse = response };
            return result;
        }
    }
}

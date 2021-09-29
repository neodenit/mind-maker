using System;
using System.Collections.Generic;
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
        [Function(nameof(CreateNode))]
        public static async Task<MultiResponse> RunAsync(
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

            var parent = GetNode(root, request.SubPath);
            parent.Children = parent.Children.Append(newNode);

            var result = new MultiResponse { OutputNode = root, HttpResponse = response };
            return result;
        }

        private static Node GetNode(Node node, IEnumerable<string> path)
        {
            if (path.Any())
            {
                var head = path.First();
                var tail = path.Skip(1);

                var nextNode = node.Children.Single(x => x.Id == head);
                return GetNode(nextNode, tail);
            }
            else
            {
                return node;
            }
        }
    }
}

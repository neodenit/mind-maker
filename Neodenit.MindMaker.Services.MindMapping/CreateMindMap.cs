using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.MindMapping.Models;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class CreateMindMap
    {
        [Function(nameof(CreateMindMap))]
        public static async Task<MultiResponse> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(CreateMindMap));
            logger.LogInformation(nameof(CreateMindMap));

            var request = await JsonSerializer.DeserializeAsync<CreateMindMapRequestDTO>(req.Body);

            var newNode = new Node
            {
                CreationTime = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Name = request.NewMindMapName,
                Owner = request.Owner,
            };

            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(newNode);

            return new MultiResponse { OutputNode = newNode, HttpResponse = response };
        }
    }
}

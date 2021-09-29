using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Neodenit.MindMaker.Services.MindMapping.Models;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class GetMindMaps
    {
        [Function(nameof(GetMindMaps))]
        public static IEnumerable<Node> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetMindMaps/{owner}/")] HttpRequestData req,
            [CosmosDBInput(
                Constants.DatabaseId,
                Constants.ContainerId,
                ConnectionStringSetting = "ConnectionString",
                SqlQuery = "SELECT * FROM c WHERE c.Owner = {owner}")] IEnumerable<Node> mindMaps,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(GetMindMaps));
            logger.LogInformation(nameof(GetMindMaps));

            return mindMaps;
        }
    }
}

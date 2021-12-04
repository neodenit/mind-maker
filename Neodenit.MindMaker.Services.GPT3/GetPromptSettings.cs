using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Neodenit.MindMaker.Services.GPT3.Models;

namespace Neodenit.MindMaker.Services.GPT3
{
    public static class GetPromptSettings
    {
        [Function(nameof(GetPromptSettings))]
        public static PromptSettings Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetPromptSettings/{owner}/")] HttpRequestData req,
            [CosmosDBInput(
                Constants.DatabaseId,
                Constants.ContainerId,
                ConnectionStringSetting = "ConnectionString",
                Id = "{owner}",
                PartitionKey = "{owner}")] PromptSettings promptSettings)
        {
            return promptSettings ?? new PromptSettings
            {
                Engine = Constants.DefaultEngine,
                Mode = Constants.DefaultConverterType,
                Randomness = Constants.DefaultRandomness
            };
        }
    }
}

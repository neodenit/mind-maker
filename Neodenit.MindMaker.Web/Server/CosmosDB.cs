using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Neodenit.MindMaker.Web.Server
{
    public class CosmosDB : ICosmosDB
    {
        public async Task InitAsync(string connectionString)
        {
            CosmosClient cosmosClient = new(connectionString);

            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(Constants.DatabaseId);
            await database.CreateContainerIfNotExistsAsync(Constants.MindMapsContainerId, Constants.MindMapsPartitionKey);
            await database.CreateContainerIfNotExistsAsync(Constants.SettingsContainerId, Constants.SettingsPartitionKey);
        }
    }
}

using System.Threading.Tasks;

namespace Neodenit.MindMaker.Web.Server
{
    public interface ICosmosDB
    {
        Task InitAsync(string connectionString);
    }
}
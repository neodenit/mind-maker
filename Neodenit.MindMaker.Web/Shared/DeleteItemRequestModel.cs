using System.Collections.Generic;

namespace Neodenit.MindMaker.Web.Shared
{
    public class DeleteItemRequestModel
    {
        public IEnumerable<string> Path { get; set; }
    }
}
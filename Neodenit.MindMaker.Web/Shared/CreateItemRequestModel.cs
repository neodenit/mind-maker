using System.Collections.Generic;

namespace Neodenit.MindMaker.Web.Shared
{
    public class CreateItemRequestModel
    {
        public IEnumerable<string> Path { get; set; }

        public string NewItemName { get; set; }
    }
}

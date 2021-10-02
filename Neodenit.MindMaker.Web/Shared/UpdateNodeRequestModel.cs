using System.Collections.Generic;

namespace Neodenit.MindMaker.Web.Shared
{
    public class UpdateNodeRequestModel
    {
        public IEnumerable<string> Path { get; set; }

        public string UpdatedNodeName { get; set; }
    }
}

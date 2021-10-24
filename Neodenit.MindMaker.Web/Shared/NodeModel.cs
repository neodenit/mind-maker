using System.Collections.Generic;
using System.Linq;

namespace Neodenit.MindMaker.Web.Shared
{
    public class NodeModel
    {
        public virtual string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<NodeModel> Children { get; set; } = Enumerable.Empty<NodeModel>();

        public bool IsOpened { get; set; }
    }
}

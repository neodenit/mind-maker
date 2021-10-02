using System.Collections.Generic;
using System.Linq;

namespace Neodenit.MindMaker.Web.Shared
{
    public class NodeDTO
    {
        public virtual string Id { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public IEnumerable<NodeDTO> Children { get; set; } = Enumerable.Empty<NodeDTO>();
    }
}

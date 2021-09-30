using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Neodenit.MindMaker.Services.MindMapping.Models
{
    public class Node
    {
        [JsonPropertyName("id")]
        public virtual string Id { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public IEnumerable<Node> Children { get; set; } = Enumerable.Empty<Node>();
    }
}

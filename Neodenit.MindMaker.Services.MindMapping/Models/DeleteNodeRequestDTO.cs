using System.Collections.Generic;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class DeleteNodeRequestDTO
    {
        public string RootId { get; set; }

        public IEnumerable<string> SubPath { get; set; }

        public string Owner { get; set; }
    }
}
using System.Collections.Generic;

namespace Neodenit.MindMaker.Web.Shared
{
    public class AdviceRequestModel
    {
        public IEnumerable<string> Parents { get; set; }

        public NodeModel Root { get; set; }

        public Mode Mode { get; set; }

        public Engine Engine { get; set; }

        public int Creativity { get; set; }
    }
}
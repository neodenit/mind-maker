﻿using System.Collections.Generic;

namespace Neodenit.MindMaker.Web.Shared
{
    public class AdviceRequestDTO
    {
        public IEnumerable<string> Parents { get; set; }

        public NodeDTO Root { get; set; }

        public Mode Mode { get; set; }

        public Engine Engine { get; set; }

        public double Randomness { get; set; }

        public string Owner { get; set; }
    }
}
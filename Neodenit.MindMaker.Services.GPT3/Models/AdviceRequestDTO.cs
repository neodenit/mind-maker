﻿using System.Collections.Generic;

namespace Neodenit.MindMaker.Services.GPT3.Models
{
    public class AdviceRequestDTO
    {
        public IEnumerable<string> Parents { get; set; }

        public NodeDTO Root { get; set; }
    }
}

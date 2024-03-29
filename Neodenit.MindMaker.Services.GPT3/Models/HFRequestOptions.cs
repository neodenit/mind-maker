﻿using Newtonsoft.Json;

namespace Neodenit.MindMaker.Services.GPT3.Models
{
    public class HFRequestOptions
    {
        [JsonProperty("wait_for_model")]
        public bool WaitForModel { get; set; }

        [JsonProperty("use_cache")]
        public bool UseCache { get; set; }
    }
}

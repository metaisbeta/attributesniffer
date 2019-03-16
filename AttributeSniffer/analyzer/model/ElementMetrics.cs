using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.model
{
    class ElementMetrics
    {
        [JsonProperty]
        public string ElementType { get; set; }

        [JsonProperty]
        public string ElementIdentifier { get; set; }

        [JsonProperty]
        public Dictionary<string, int> Metrics { get; set; }
    }
}

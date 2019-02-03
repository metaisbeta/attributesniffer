using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeSniffer.analyzer.classMetrics
{
    class ClassMetrics
    {
        [JsonProperty]
        private string className { get; set; }

        [JsonProperty]
        private Dictionary<string, int> metrics { get; set; }

        public ClassMetrics(string className, Dictionary<string, int> metrics)
        {
            this.className = className;
            this.metrics = metrics;
        }
    }
}

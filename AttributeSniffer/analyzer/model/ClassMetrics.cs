using System.Collections.Generic;
using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.classMetrics
{
    /// <summary>
    /// Represent the metrics of a single class.
    /// </summary>
    public class ClassMetrics
    {
        [JsonProperty]
        public string ClassName { get; set; }

        [JsonProperty]
        public Dictionary<string, int> Metrics { get; set; }

        public ClassMetrics()
        {
            this.Metrics = new Dictionary<string, int>();
        }

        public ClassMetrics(string className, Dictionary<string, int> metrics)
        {
            this.ClassName = className;
            this.Metrics = metrics;
        }
    }
}

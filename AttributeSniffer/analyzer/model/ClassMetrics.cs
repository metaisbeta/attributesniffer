using System.Collections.Generic;
using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.classMetrics
{
    public class ClassMetrics
    {
        [JsonProperty]
        public string ClassName { get;}

        [JsonProperty]
        public Dictionary<string, int> Metrics { get;}

        public ClassMetrics(string className, Dictionary<string, int> metrics)
        {
            this.ClassName = className;
            this.Metrics = metrics;
        }
    }
}

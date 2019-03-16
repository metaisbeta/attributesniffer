using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.model
{
    public class MetricResult
    {        
        [JsonProperty]
        public string ElementIdentifier { get; set; }

        [JsonProperty]
        public string ElementType { get; set; }

        [JsonProperty]
        public string Metric { get; set; }

        [JsonProperty]
        public int Result { get; set; }

        public MetricResult()
        {
            // For serialization
        }

        public MetricResult(string elementType, string elementIdentifier, string metric, int result)
        {
            this.ElementType = elementType;
            this.ElementIdentifier = elementIdentifier;
            this.Metric = metric;
            this.Result = result;
        }
    }
}

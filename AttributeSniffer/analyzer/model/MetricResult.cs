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

        public MetricResult(string elementIdentifier, string elementType, string metric, int result)
        {
            this.ElementIdentifier = elementIdentifier;
            this.ElementType = elementType;
            this.Metric = metric;
            this.Result = result;
        }
    }
}

using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.model
{
    /// <summary>
    /// Represents a metric result. Includes the element identifier, the metric abbreviation and its result.
    /// </summary>
    public class MetricResult
    {        
        [JsonProperty]
        public ElementIdentifier ElementIdentifier { get; set; }

        [JsonProperty]
        public string Metric { get; set; }

        [JsonProperty]
        public int Result { get; set; }

        public MetricResult()
        {
            // For serialization
        }

        public MetricResult(ElementIdentifier elementIdentifier, string metric, int result)
        {
            ElementIdentifier = elementIdentifier;
            Metric = metric;
            Result = result;
        }
    }
}

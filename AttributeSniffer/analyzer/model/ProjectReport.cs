using System.Collections.Generic;
using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.model
{
    /// <summary>
    /// Represents the project report, with the all the metrics of the C# classes.
    /// </summary>
    public class ProjectReport
    {
        [JsonProperty]
        public string ProjectName { get; set; }

        [JsonProperty]
        public int classesNumber { get; set; }

        [JsonProperty]
        public int classesWithAttributesNumber { get; set; }

        [JsonProperty]
        public int attributesNumber { get; set; }

        [JsonProperty]
        public List<MetricResult> MetricsResults { get; set; }

        public ProjectReport()
        {
            // For serialization
            this.MetricsResults = new List<MetricResult>();
        }

        public ProjectReport(string projectName, int classesNumber, int classesWithAttributesNumber, int attributesNumber, List<MetricResult> metricsResults)
        {
            ProjectName = projectName;
            this.classesNumber = classesNumber;
            this.classesWithAttributesNumber = classesWithAttributesNumber;
            this.attributesNumber = attributesNumber;
            MetricsResults = metricsResults;
        }
    }
}

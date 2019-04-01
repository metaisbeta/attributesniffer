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
        public int ClassesNumber { get; set; }

        [JsonProperty]
        public int ClassesWithAttributesNumber { get; set; }

        [JsonProperty]
        public int AttributesNumber { get; set; }

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
            ClassesNumber = classesNumber;
            ClassesWithAttributesNumber = classesWithAttributesNumber;
            AttributesNumber = attributesNumber;
            MetricsResults = metricsResults;
        }
    }
}

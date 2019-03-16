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
        public List<MetricResult> MetricsResults { get; set; }

        public ProjectReport()
        {
            // For serialization
            this.MetricsResults = new List<MetricResult>();
        }

        public ProjectReport(string projectName, List<MetricResult> metricsResults)
        {
            this.ProjectName = projectName;
            this.MetricsResults = metricsResults;
        }
    }
}

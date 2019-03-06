using System.Collections.Generic;
using AttributeSniffer.analyzer.classMetrics;
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
        public List<ClassMetrics> ClassesMetrics { get; set; }

        public ProjectReport()
        {
            this.ClassesMetrics = new List<ClassMetrics>();
        }

        public ProjectReport(string projectName, List<ClassMetrics> classesMetrics)
        {
            this.ProjectName = projectName;
            this.ClassesMetrics = classesMetrics;
        }
    }
}

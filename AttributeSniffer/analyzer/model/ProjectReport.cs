using System.Collections.Generic;
using AttributeSniffer.analyzer.classMetrics;
using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.model
{
    public class ProjectReport
    {
        [JsonProperty]
        public string ProjectName { get;}

        [JsonProperty]
        public List<ClassMetrics> ClassesMetrics { get; set; }

        public ProjectReport(string projectName, List<ClassMetrics> classesMetrics)
        {
            this.ProjectName = projectName;
            this.ClassesMetrics = classesMetrics;
        }
    }
}

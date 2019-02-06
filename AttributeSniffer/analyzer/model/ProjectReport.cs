using AttributeSniffer.analyzer.classMetrics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

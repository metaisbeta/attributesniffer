using AttributeSniffer.analyzer.classMetrics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeSniffer.analyzer.model
{
    class ProjectReport
    {
        [JsonProperty]
        private string projectName { get; set; }

        [JsonProperty]
        private List<ClassMetrics> classesMetrics { get; set; }

        public ProjectReport(string projectName, List<ClassMetrics> classesMetrics)
        {
            this.projectName = projectName;
            this.classesMetrics = classesMetrics;
        }
    }
}

using AttributeSniffer.analyzer.classMetrics;
using AttributeSniffer.analyzer.model;
using System.Collections.Generic;
using System.IO;

namespace AttributeSniffer.analyzer
{
    public class Sniffer{

        public ProjectReport Sniff(string folderPath){
            MetricsCollector metricsCollector = new MetricsCollector();
            List<ClassMetrics> collectedMetrics = new List<ClassMetrics>();
            foreach (string file in Directory.GetFiles(folderPath, "*.cs",
                                        SearchOption.AllDirectories)){
                string classContent = File.ReadAllText(file);
                collectedMetrics.Add(metricsCollector.collect(classContent));  
            }
            //TODO fetch project name correctly
            return new ProjectReport("projectName", collectedMetrics);
            //return new ReportConverter().convert(projectReport);
        }

    }
}

using AttributeSniffer.analyzer.classMetrics;
using AttributeSniffer.analyzer.model;
using AttributeSniffer.analyzer.report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeSniffer.analyzer
{
    class Sniffer
    {
        public String Sniff(string folderPath)
        {
            MetricsCollector metricsCollector = new MetricsCollector();
            List<ClassMetrics> collectedMetrics = new List<ClassMetrics>();
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.cs"))
            {
                String classContent = File.ReadAllText(file);
                collectedMetrics.Add(metricsCollector.collect(classContent));  
            }

            ProjectReport projectReport = new ProjectReport("projectName", collectedMetrics);

            return new ReportConverter().convert(projectReport);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AttributeSniffer.analyzer.classMetrics;
using AttributeSniffer.analyzer.model;

namespace AttributeSniffer.analyzer
{
    public class Sniffer{

        public ProjectReport Sniff(string folderPath)
        {
            MetricsCollector metricsCollector = new MetricsCollector();
            List<ClassMetrics> collectedMetrics = new List<ClassMetrics>();
            List<Task> metricsCollectorTasks = new List<Task>();
            foreach (string file in Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories))
            {
                Task.Factory.StartNew(() => {
                    string classContent = File.ReadAllText(file);
                    collectedMetrics.Add(metricsCollector.collect(classContent));
                    Console.WriteLine($"Thread id {Thread.CurrentThread.ManagedThreadId}");
                });
            }

            Task.WaitAll(metricsCollectorTasks.ToArray());

            // TODO fetch project name correctly
            return new ProjectReport("projectName", collectedMetrics);
        }

    }
}

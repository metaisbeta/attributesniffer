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

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private MetricsCollector metricsCollector;

        public Sniffer()
        {
            this.metricsCollector = new MetricsCollector();
        }

        public ProjectReport Sniff(string folderPath)
        {
            logger.Info("Starting to analyze path: {0}", folderPath);

            List<ClassMetrics> collectedMetrics = new List<ClassMetrics>();
            List<Task> metricsCollectorTasks = new List<Task>();
            foreach (string file in Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories))
            {
                metricsCollectorTasks.Add(Task.Factory.StartNew(collectMetrics(metricsCollector, collectedMetrics, file)));
            }

            Task.WaitAll(metricsCollectorTasks.ToArray());
            logger.Info("Finished analyzing all files of path: {0}", folderPath);

            // TODO fetch project name correctly
            return new ProjectReport("projectName", collectedMetrics);
        }

        private Action collectMetrics(MetricsCollector metricsCollector, List<ClassMetrics> collectedMetrics, string file)
        {
            return () =>
            {
                string classContent = File.ReadAllText(file);
                collectedMetrics.Add(metricsCollector.collect(classContent));
                logger.Trace("Finished collecting metrics for file '{0}' at thread {1} ", file, Thread.CurrentThread.ManagedThreadId);
            };
        }
    }
}

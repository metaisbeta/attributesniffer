using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AttributeSniffer.analyzer.model;

namespace AttributeSniffer.analyzer
{
    /// <summary>
    /// Sniffer component.
    /// </summary>
    public class Sniffer
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private MetricsCollector metricsCollector;

        public Sniffer()
        {
            this.metricsCollector = new MetricsCollector();
        }

        /// <summary>
        /// Sniff code to extract metrics.
        /// </summary>
        /// <param name="folderPath">Folder where the files are contained.</param>
        /// <returns>Project report with all the metrics extracted.</returns>
        public ProjectReport Sniff(string folderPath)
        {
            logger.Info("Starting to analyze path: {0}", folderPath);

            List<MetricResult> collectedMetrics = new List<MetricResult>();
            List<Task> metricsCollectorTasks = new List<Task>();
            foreach (string file in Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories))
            {
                metricsCollectorTasks.Add(Task.Factory.StartNew(CollectMetrics(metricsCollector, collectedMetrics, file)));
            }

            Task.WaitAll(metricsCollectorTasks.ToArray());
            logger.Info("Finished analyzing all files of path: {0}", folderPath);


            return new ProjectReport(GetProjectName(folderPath), collectedMetrics.OrderBy(metric => metric.Metric).ToList());
        }

        /// <summary>
        /// Action responsible to collect metrics.
        /// </summary>
        /// <param name="metricsCollector">Metrics collector component.</param>
        /// <param name="collectedMetrics">List of metrics.</param>
        /// <param name="file">File to be analyzed.</param>
        /// <returns>Collecting metrics Action.</returns>
        private Action CollectMetrics(MetricsCollector metricsCollector, List<MetricResult> collectedMetrics, string file)
        {
            return () =>
            {
                string classContent = File.ReadAllText(file);
                collectedMetrics.AddRange(metricsCollector.Collect(classContent));
                logger.Trace("Finished collecting metrics for file '{0}' at thread {1} ", file, Thread.CurrentThread.ManagedThreadId);
            };
        }

        /// <summary>
        /// Get the project name of the project path.
        /// </summary>
        /// <param name="path">Project path.</param>
        /// <returns>Project name.</returns>
        private string GetProjectName(string path)
        {
            return new DirectoryInfo(path).Name;
        }
    }
}

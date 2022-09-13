using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AttributeSniffer.analyzer.metrics;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;

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

            string[] csFiles = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);
            string[] dllFiles = Directory.GetFiles(folderPath, "*.dll", SearchOption.AllDirectories);
            List<MetadataReference> metadataReferences = new List<MetadataReference>();
            try
            {
                foreach (var dll in dllFiles)
                {
                    metadataReferences.Add(MetadataReference.CreateFromFile(dll));
                }

                this.metricsCollector.metadataReferences = metadataReferences;

                //ParallelLoopResult parallelLoopResult = Parallel.ForEach(csFiles, (csFile) =>
                //{
                //    collectedMetrics.AddRange(CollectMetrics(metricsCollector, csFile, folderPath));
                //});
                foreach (var csFile in csFiles)
                {
                    collectedMetrics.AddRange(CollectMetrics(metricsCollector, csFile, folderPath));
                }
                logger.Info("Finished analyzing all files of path: {0}", folderPath);

                collectedMetrics.RemoveAll(metric => metric == null);
                List<MetricResult> acMetrics = collectedMetrics.FindAll(metric => metric.Metric.Equals(Metric.METADATA_DECLARATION_IN_CLASS.GetIdentifier()));
                int numberOfAttributes = acMetrics.Sum(ac => ac.Result);

                List<MetricResult> metricsWithoutNamespace =
                    collectedMetrics.Where(x => x.Metric != Metric.METADATA_SCHEMA_IN_CLASS.GetIdentifier()).OrderBy(metric => metric.Metric).ToList();

                List<MetricResult> namespaceMetricsPerClass = collectedMetrics.Where(x => x.Metric.Equals(Metric.METADATA_SCHEMA_IN_CLASS.GetIdentifier())).ToList();
                List<MetricResult> namespaceMetricsAllProject = namespaceMetricsPerClass.Distinct(x => x.ElementIdentifier.ElementName).ToList();
                int numberOfNamespaces = namespaceMetricsAllProject.Count();
                                
                return new ProjectReport(GetProjectName(folderPath), csFiles.Count(), acMetrics.Count, numberOfAttributes, numberOfNamespaces,
                    metricsWithoutNamespace, namespaceMetricsPerClass.OrderBy(x => x.ElementIdentifier.FileDeclarationPath).ToList(), namespaceMetricsAllProject);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Error collecting metrics");
                throw ex;
            }
        }

        /// <summary>
        /// Collect metrics.
        /// </summary>
        /// <param name="metricsCollector">Metrics collector component.</param>
        /// <param name="file">File to be analyzed.</param>
        /// <returns>Collecting metrics Action.</returns>
        private List<MetricResult> CollectMetrics(MetricsCollector metricsCollector, string file, string folderPath)
        {
            logger.Trace("Starting to collect metrics for file '{0}' at thread {1} ", file, Thread.CurrentThread.ManagedThreadId);
            string classContent = File.ReadAllText(file);
            List<MetricResult> metricResults = metricsCollector.Collect(Path.GetRelativePath(folderPath, file), classContent);
            logger.Trace("Finished collecting metrics for file '{0}' at thread {1} ", file, Thread.CurrentThread.ManagedThreadId);

            return metricResults;
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

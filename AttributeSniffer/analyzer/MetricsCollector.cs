using System;
using System.Collections.Generic;
using System.Linq;
using AttributeSniffer.analyzer.metrics;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer
{
    /// <summary>
    /// Metrics collector component.
    /// </summary>
    public class MetricsCollector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Collect metrics of a C# class.
        /// </summary>
        /// <param name="classContent">Content of the class.</param>
        /// <returns>class metrics information.</returns>
        public List<MetricResult> Collect(string classContent)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(classContent);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            List<MetricResult> metricsResults = new List<MetricResult>();

            // Collect metrics
            foreach (var metric in GetAllMetrics())
            {
                ((CSharpSyntaxWalker)metric).Visit(root);
                metricsResults.Add(metric.GetResult());
                logger.Info("Collected {0} metric for element '{1}'.", metric.GetName(), metric.GetElementIdentifier());
            }

            return metricsResults;
        }

        /// <summary>
        /// Get all the Metrics which implement the <c>MetricCollector</c> interface.
        /// </summary>
        /// <returns>List of all the metrics.</returns>
        private List<MetricCollector> GetAllMetrics()
        {
            Type metrictCollectorType = typeof(MetricCollector);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => metrictCollectorType.IsAssignableFrom(type) && !type.IsInterface)
                .Select(type => (MetricCollector)Activator.CreateInstance(type))
                .ToList();
        }
    }
}

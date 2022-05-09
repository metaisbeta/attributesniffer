using System;
using System.Collections.Generic;
using System.IO;
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
        public List<MetricResult> Collect(string fileName, string classContent)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(classContent);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            SemanticModel semanticModel = getSemanticModel(Path.GetFileName(fileName), tree);
            List<MetricResult> metricsResults = new List<MetricResult>();

            // Collect metrics
            foreach (var metric in GetAllMetrics())
            {
                metric.SetSemanticModel(semanticModel);
                metric.SetFilePath(fileName);
                ((CSharpSyntaxWalker)metric).Visit(root);
                metric.SetResult(metricsResults);
                metric.Dispose();
            }

            return metricsResults;
        }

        /// <summary>
        /// Get all the Metrics which implement the <c>MetricCollector</c> interface.
        /// </summary>
        /// <returns>List of all the metrics.</returns>
        private List<IMetricCollector> GetAllMetrics()
        {
            Type metrictCollectorType = typeof(IMetricCollector);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => metrictCollectorType.IsAssignableFrom(type) && !type.IsInterface)
                .Select(type => (IMetricCollector)Activator.CreateInstance(type))
                .ToList();
        }

        private SemanticModel getSemanticModel(String assemblyName, SyntaxTree tree)
        {
            PortableExecutableReference Mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            CSharpCompilation compilation = CSharpCompilation.Create(assemblyName).AddReferences(Mscorlib).AddSyntaxTrees(tree);

            return compilation.GetSemanticModel(tree);
        }
    }
}

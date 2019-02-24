using System;
using System.Collections.Generic;
using System.Linq;
using AttributeSniffer.analyzer.classMetrics;
using AttributeSniffer.analyzer.metrics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer
{
    public class MetricsCollector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ClassMetrics collect(string classContent)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(classContent);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            Dictionary<string, int> classMetricsResult = new Dictionary<string, int>();

            ClassInfo classInfo = new ClassInfo();
            classInfo.Visit(root);

            // Collect metrics
            foreach (var metric in getAllMetrics())
            {
                ((CSharpSyntaxWalker)metric).Visit(root);
                classMetricsResult.Add(metric.getName(), metric.getResult());
                logger.Info("Collected {0} metric for class '{1}'.", metric.getName(), classInfo.FullClassName);
            }

            logger.Info("Finished collecting all metrics for class '{0}'.",classInfo.FullClassName);
            return new ClassMetrics(classInfo.FullClassName, classMetricsResult);
        }

        private List<MetricCollector> getAllMetrics()
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

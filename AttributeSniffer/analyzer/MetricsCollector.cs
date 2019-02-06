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
        public ClassMetrics collect(string classContent)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(classContent);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            Dictionary<string, int> classMetricsResult = new Dictionary<string, int>();

            // Get class info
            ClassInfo classInfo = new ClassInfo();
            classInfo.Visit(root);
                
            // Collect metrics
            foreach (var metric in getAllMetrics())
            {
                ((CSharpSyntaxWalker)metric).Visit(root);
                classMetricsResult.Add(metric.getName(), metric.getResult());
            }

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

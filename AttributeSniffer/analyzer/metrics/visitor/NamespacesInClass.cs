using AttributeSniffer.analyzer.metrics.model;
using AttributeSniffer.analyzer.metrics.visitor.util;
using AttributeSniffer.analyzer.model;
using AttributeSniffer.analyzer.model.exception;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeSniffer.analyzer.metrics.visitor
{
    public class NamespacesInClass : CSharpSyntaxWalker, IMetricCollector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private SemanticModel SemanticModel { get; set; }
        private string FilePath { get; set; }
        private int NumberOfArguments { get; set; } = 0;

        private List<MetricResult> ResultsByElement { get; set; } = new List<MetricResult>();

        public override void Visit(SyntaxNode node)
        {
            List<MetricResult> metricResults = new List<MetricResult>();

            this.NumberOfArguments = node.DescendantNodes().Where(a => a is UsingDirectiveSyntax).ToList().Count();
            metricResults = GetResults(node);

            if (metricResults.Count != 0)
            {
                ResultsByElement.AddRange(metricResults);
            }
        }

        private List<MetricResult> GetResults(SyntaxNode syntaxNode)
        {
            List<MetricResult> metricResults = new List<MetricResult>();
            
            try
            {
                foreach (var item in syntaxNode.DescendantNodes().Where(x => x is AttributeSyntax).ToList())
                {                    
                     List<ElementIdentifier> elementIdentifiers = ElementIdentifierHelper.getElementIdentifiersForNamespaceMetrics(FilePath,
                        SemanticModel, syntaxNode.DescendantNodes().Where(a => a is UsingDirectiveSyntax).ToList(), (AttributeSyntax)item);
                    //usings
                    string metricName = Metric.NAMESPACES_IN_CLASS.GetIdentifier();
                    string metricType = MetricType.ELEMENT_METRIC.GetIdentifier();
                    elementIdentifiers.ForEach(identifier =>
                    {
                        metricResults.Add(new MetricResult(identifier, metricType, metricName, NumberOfArguments));
                    });
                }
            }
            catch (IgnoreElementIdentifierException e)
            {
                logger.Info("Ignoring using due to syntax error {0}.");
            }

            return metricResults;
        }

        public void SetSemanticModel(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;
        }

        public void SetFilePath(string filePath)
        {
            this.FilePath = filePath;
        }

        public void SetResult(List<MetricResult> metricResults)
        {
            metricResults.AddRange(ResultsByElement);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }

    public static class INamespaceSymbolExtension
    {
        public static IEnumerable<INamedTypeSymbol> GetAllTypes(this INamespaceSymbol @namespace)
        {
            Queue<INamespaceOrTypeSymbol> symbols = new Queue<INamespaceOrTypeSymbol>();
            symbols.Enqueue(@namespace);

            while (symbols.Count > 0)
            {
                INamespaceOrTypeSymbol namespaceOrTypeSymbol = symbols.Dequeue();
                INamespaceSymbol namespaceSymbol = namespaceOrTypeSymbol as INamespaceSymbol;
                if (namespaceSymbol == null)
                {
                    INamedTypeSymbol typeSymbol = (INamedTypeSymbol)namespaceOrTypeSymbol;
                    Array.ForEach(typeSymbol.GetTypeMembers().ToArray(), symbols.Enqueue);

                    yield return typeSymbol;
                }
                else
                {
                    Array.ForEach(namespaceSymbol.GetMembers().ToArray(), symbols.Enqueue);
                }
            }
        }
    }
}

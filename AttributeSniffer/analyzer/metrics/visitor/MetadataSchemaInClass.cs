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

namespace AttributeSniffer.analyzer.metrics.visitor
{
    /// <summary>
    /// Visit a compilation unit to extract the MSC metric.
    /// </summary>
    public class MetadataSchemaInClass : CSharpSyntaxWalker, IMetricCollector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private SemanticModel SemanticModel { get; set; }
        private string FilePath { get; set; }
        private List<MetricResult> ResultsByElement { get; set; } = new List<MetricResult>();

        private static Dictionary<string, (string, int)> NamespacesSaved = new Dictionary<string, (string, int)>();

        public override void Visit(SyntaxNode node)
        {
            List<MetricResult> metricResults = new List<MetricResult>();

            metricResults = GetResults(node);
            if (metricResults.Count != 0)
                ResultsByElement.AddRange(metricResults);
        }

        private List<MetricResult> GetResults(SyntaxNode syntaxNode)
        {
            List<MetricResult> metricResults = new List<MetricResult>();
            List<ElementIdentifier> elementIdentifiers = new();
            try
            {
                foreach (var item in syntaxNode.DescendantNodes().Where(x => x is AttributeSyntax).ToList())
                {
                    elementIdentifiers.AddRange(ElementIdentifierHelper.getElementIdentifiersForNamespaceMetrics(FilePath,
                        SemanticModel, syntaxNode.DescendantNodes().Where(a => a is UsingDirectiveSyntax).ToList(), (AttributeSyntax)item, NamespacesSaved));
                }
                string metricName = Metric.METADATA_SCHEMA_IN_CLASS.GetIdentifier();
                string metricType = MetricType.ELEMENT_METRIC.GetIdentifier();
                elementIdentifiers.ForEach(identifier =>
                {
                    metricResults.Add(new MetricResult(identifier, metricType, metricName, elementIdentifiers.Distinct(x => x.ElementSchema).Count()));
                });
            }
            catch (IgnoreElementIdentifierException e)
            {
                logger.Info("Ignoring using due to syntax error {0}.", e.Message);
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
}

using System;
using System.Collections.Generic;
using AttributeSniffer.analyzer.metrics.model;
using AttributeSniffer.analyzer.metrics.visitor.util;
using AttributeSniffer.analyzer.model;
using AttributeSniffer.analyzer.model.exception;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor
{
    /// <summary>
    /// Visit a compilation unit to extract the MDE metric.
    /// </summary>
    class MetadataDeclarationInElement : CSharpSyntaxWalker, IMetricCollector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private SemanticModel SemanticModel { get; set; }
        private string FilePath { get; set; }
        private Dictionary<ElementIdentifier, List<AttributeSyntax>> attributesByElement = new Dictionary<ElementIdentifier, List<AttributeSyntax>>();

        public override void VisitAttribute(AttributeSyntax node)
        {
            try
            {
                List<ElementIdentifier> elementIdentifiers = ElementIdentifierHelper.getElementIdentifiersForElementMetrics(FilePath, SemanticModel, (AttributeListSyntax)node.Parent);

                elementIdentifiers.ForEach(identifier =>
                {
                    if (attributesByElement.ContainsKey(identifier))
                    {
                        attributesByElement[identifier].Add(node);
                    }
                    else
                    {
                        attributesByElement.Add(identifier, new List<AttributeSyntax>() { node });
                    }
                });
            }
            catch (IgnoreElementIdentifierException e)
            {
                logger.Info("Ignoring attribute due to syntax error {0}.", node.GetText());
            }
        }

        public void SetResult(List<MetricResult> metricResults)
        {
            foreach (KeyValuePair<ElementIdentifier, List<AttributeSyntax>> entry in attributesByElement)
            {
                string metricName = Metric.METADATA_DECLARATION_IN_ELEMENT.GetIdentifier();
                string metricType = MetricType.ELEMENT_METRIC.GetIdentifier();
                MetricResult metricResult = new MetricResult(entry.Key, metricType, metricName, entry.Value.Count);
                metricResults.Add(metricResult);
            }
        }

        public void SetSemanticModel(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;
        }

        public void SetFilePath(string filePath)
        {
            this.FilePath = filePath;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}

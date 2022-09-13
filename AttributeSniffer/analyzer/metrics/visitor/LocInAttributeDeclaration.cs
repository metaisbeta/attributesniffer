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
    /// Visit a compilation unit to extract the LOCAD metric.
    /// </summary>
    class LocInAttributeDeclaration : CSharpSyntaxWalker, IMetricCollector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private SemanticModel SemanticModel { get; set; }
        private string FilePath { get; set; }
        private AttributeSyntax VisitedAttribute { get; set; }
        private int NumberOfLines { get; set; }
        private List<MetricResult> ResultsByElement { get; set; } = new List<MetricResult>();

        public override void VisitAttribute(AttributeSyntax node)
        {
            this.VisitedAttribute = node;
            NumberOfLines = node.GetText().Lines.Count;

            List<MetricResult> metricResults = GetResult();

            if (metricResults.Count != 0)
            {
                ResultsByElement.AddRange(metricResults);
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

        public void SetResult(List<MetricResult> metricResults)
        { 
            metricResults.AddRange(ResultsByElement);
        }

        private List<MetricResult> GetResult()
        {
            List<MetricResult> metricResults = new List<MetricResult>();
            try
            {
                List<ElementIdentifier> elementIdentifiers = ElementIdentifierHelper.getElementIdentifiersForAttributeMetrics(FilePath, SemanticModel, VisitedAttribute);

                string metricName = Metric.LOC_IN_METADATA_DECLARATION.GetIdentifier();
                string metricType = MetricType.ELEMENT_METRIC.GetIdentifier();

                elementIdentifiers.ForEach(identifier => metricResults.Add(new MetricResult(identifier, metricType, metricName, NumberOfLines)));
            }
            catch (IgnoreElementIdentifierException e)
            {
                logger.Info("Ignoring attribute due to syntax error {0}.", VisitedAttribute.GetText());
            }

            return metricResults;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}

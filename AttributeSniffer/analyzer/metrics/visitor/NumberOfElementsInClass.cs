using AttributeSniffer.analyzer.metrics.model;
using AttributeSniffer.analyzer.metrics.visitor.util;
using AttributeSniffer.analyzer.model;
using AttributeSniffer.analyzer.model.exception;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeSniffer.analyzer.metrics.visitor
{
    public class NumberOfElementsInClass : CSharpSyntaxWalker, IMetricCollector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private SemanticModel SemanticModel { get; set; }
        private string FilePath { get; set; }
        private List<MetricResult> ResultsByElement { get; set; } = new List<MetricResult>();
        public override void Visit(SyntaxNode node)
        {
            try
            {
                int elementIdentifiers = ElementIdentifierHelper.getElementIdentifiersNumberInClass(node);
                string metricName = Metric.NUMBER_OF_ELEMENTS_IN_CLASS.GetIdentifier();
                string metricType = MetricType.CLASS_METRIC.GetIdentifier();

                if (elementIdentifiers > 0)
                {
                    ResultsByElement.Add(new MetricResult(new ElementIdentifier()
                    {
                        ElementName = "Number Of Elements In Class",
                        ElementType = ElementIdentifierType.NUMBER_OF_ELEMENTS.GetTypeIdentifier(),
                        FileDeclarationPath = FilePath,
                        LineNumber = 0
                    }, metricType, metricName, elementIdentifiers));
                }
            }
            catch (IgnoreElementIdentifierException e)
            {
                logger.Info("Ignoring class due to syntax error {0}.", e.Message);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
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
    }
}

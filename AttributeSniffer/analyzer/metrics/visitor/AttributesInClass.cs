using System;
using System.Collections.Generic;
using AttributeSniffer.analyzer.metrics.model;
using AttributeSniffer.analyzer.metrics.visitor.util;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics
{
    /// <summary>
    /// Visit a compilation unit to extract the AC metric.
    /// </summary>
    class AttributesInClass : CSharpSyntaxWalker, MetricCollector
    {
        private SemanticModel SemanticModel { get; set; }
        private string FilePath { get; set; }
        private int NumberOfAttributes { get; set; } = 0;
        private ElementIdentifier ElementIdentifier { get; set; }

        public override void VisitAttribute(AttributeSyntax node)
        {
            this.NumberOfAttributes++;

            if (ElementIdentifier == null)
            {
                ElementIdentifier = ElementIdentifierHelper.getElementIdentifierForClassMetrics(FilePath, SemanticModel, node);
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
            if (ElementIdentifier != null) {
                string metricName = Metric.ATTRIBUTES_IN_CLASS.GetIdentifier();
                string metricType = MetricType.CLASS_METRIC.GetIdentifier();
                metricResults.Add(new MetricResult(ElementIdentifier, metricType, metricName, NumberOfAttributes));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}

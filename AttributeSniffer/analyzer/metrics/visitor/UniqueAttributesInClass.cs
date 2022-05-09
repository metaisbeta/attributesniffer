using System;
using System.Collections.Generic;
using AttributeSniffer.analyzer.metrics.model;
using AttributeSniffer.analyzer.metrics.visitor.util;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor
{
    /// <summary>
    /// Visit a compilation unit to extract the UAC metric.
    /// </summary>
    class UniqueAttributesInClass : CSharpSyntaxWalker, IMetricCollector
    {
        private SemanticModel SemanticModel { get; set; }
        private string FilePath { get; set; }
        private List<AttributeSyntax> UniqueAttributes { get; set; } = new List<AttributeSyntax>();
        private ElementIdentifier ElementIdentifier { get; set; }

        public override void VisitAttribute(AttributeSyntax node)
        {
            AttributeSyntax normalized = node.NormalizeWhitespace();
            if (!UniqueAttributes.Exists(attribute => attribute.IsEquivalentTo(normalized)))
            {
                UniqueAttributes.Add(normalized);
            }

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
            if (ElementIdentifier != null)
            {
                string metricName = Metric.UNIQUE_ATTRIBUTES_IN_CLASS.GetIdentifier();
                string metricType = MetricType.CLASS_METRIC.GetIdentifier();
                metricResults.Add(new MetricResult(ElementIdentifier, metricType, metricName, UniqueAttributes.Count));
            }
            
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}

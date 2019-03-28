using System.Collections.Generic;
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
        private int NumberOfAttributes { get; set; } = 0;
        private AttributeSyntax VisitedAttribute;

        public override void VisitAttribute(AttributeSyntax node)
        {
            this.VisitedAttribute = node;
            this.NumberOfAttributes++;
        }

        public void SetSemanticModel(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;
        }

        public void SetResult(List<MetricResult> metricResults)
        {
            metricResults.Add(GetResult());
        }

        private MetricResult GetResult()
        {
            ElementIdentifier elementIdentifier = ElementIdentifierHelper.getTargetElementForClassMetrics(SemanticModel, VisitedAttribute.AncestorsAndSelf());

            string metricName = Metric.ATTRIBUTES_IN_CLASS.GetIdentifier();
            return new MetricResult(elementIdentifier, metricName, NumberOfAttributes);
        }
    }
}

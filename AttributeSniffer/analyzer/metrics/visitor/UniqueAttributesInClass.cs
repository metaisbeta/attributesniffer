using System.Collections.Generic;
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
    class UniqueAttributesInClass : CSharpSyntaxWalker, MetricCollector
    {
        private SemanticModel SemanticModel { get; set; }
        private List<AttributeSyntax> UniqueAttributes { get; set; } = new List<AttributeSyntax>();
        private AttributeSyntax VisitedAttribute { get; set; }

        public override void VisitAttribute(AttributeSyntax node)
        {
            VisitedAttribute = node;
            if (!UniqueAttributes.Exists(attribute => attribute.IsEquivalentTo(node)))
            {
                UniqueAttributes.Add(node);
            }
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
            ElementIdentifier elementIdentifier = ElementIdentifierHelper.getElementIdentifierForClassMetrics(SemanticModel, VisitedAttribute.AncestorsAndSelf());
            string metricName = Metric.UNIQUE_ATTRIBUTES_IN_CLASS.GetIdentifier();
            return new MetricResult(elementIdentifier, metricName, UniqueAttributes.Count);
        }
    }
}

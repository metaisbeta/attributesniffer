using System.Collections.Generic;
using AttributeSniffer.analyzer.metrics.visitor.util;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor
{
    /// <summary>
    /// Visit a compilation unit to extract the AED metric.
    /// </summary>
    class AttributesInElementDeclaration : CSharpSyntaxWalker, MetricCollector
    {
        private SemanticModel SemanticModel { get; set; }
        private Dictionary<ElementIdentifier, List<AttributeSyntax>> attributesByElement = new Dictionary<ElementIdentifier, List<AttributeSyntax>>();

        public override void VisitAttribute(AttributeSyntax node)
        {
            ElementIdentifier elementIdentifier = ElementIdentifierHelper.getElementIdentifierForElementMetrics(SemanticModel, (AttributeListSyntax)node.Parent);

            if (attributesByElement.ContainsKey(elementIdentifier))
            {
                attributesByElement[elementIdentifier].Add(node);
            }
            else
            {
                attributesByElement.Add(elementIdentifier, new List<AttributeSyntax>() { node });
            }
        }

        public void SetResult(List<MetricResult> metricResults)
        {
            foreach (KeyValuePair<ElementIdentifier, List<AttributeSyntax>> entry in attributesByElement)
            {
                MetricResult metricResult = new MetricResult(entry.Key, Metric.ATTRIBUTES_IN_ELEMENT_DECLARATION.GetIdentifier(), entry.Value.Count);
                metricResults.Add(metricResult);
            }
        }

        public void SetSemanticModel(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;
        }
    }
}

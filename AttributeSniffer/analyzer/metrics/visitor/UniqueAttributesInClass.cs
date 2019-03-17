using System;
using System.Collections.Generic;
using System.Text;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using AttributeSniffer.analyzer.metrics.visitor.util;

namespace AttributeSniffer.analyzer.metrics.visitor
{
    /// <summary>
    /// Visit a compilation unit to extract the UAC metric.
    /// </summary>
    class UniqueAttributesInClass : CSharpSyntaxWalker, MetricCollector
    {
        private List<AttributeSyntax> uniqueAttributes { get; set; } = new List<AttributeSyntax>();
        private AttributeSyntax visitedAttribute { get; set; }

        public override void VisitAttribute(AttributeSyntax node)
        {
            visitedAttribute = node;
            if (!uniqueAttributes.Exists(attribute => attribute.IsEquivalentTo(node)))
            {
                uniqueAttributes.Add(node);
            }
        }

        public MetricResult GetResult(SemanticModel semanticModel)
        {
            ITypeSymbol targetElementSymbol = ElementIdentifierHelper.getTargetElementForClassMetrics(semanticModel, visitedAttribute.AncestorsAndSelf());

            string elementType = targetElementSymbol.TypeKind.ToString();
            string elementIdentifier = targetElementSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            string metricName = Metric.UNIQUE_ATTRIBUTES_IN_CLASS.GetIdentifier();
            return new MetricResult(elementIdentifier, elementType, metricName, uniqueAttributes.Count);
        }
    }
}

using AttributeSniffer.analyzer.metrics.visitor.util;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace AttributeSniffer.analyzer.metrics
{
    /// <summary>
    /// Visit a compilation unit to extract the AC metric.
    /// </summary>
    class AttributesInClass : CSharpSyntaxWalker, MetricCollector
    {
        private int numberOfAttributes { get; set; } = 0;
        private AttributeSyntax visitedAttribute;

        public override void VisitAttribute(AttributeSyntax node)
        {
            this.visitedAttribute = node;
            this.numberOfAttributes++;
        }

        public MetricResult GetResult(SemanticModel semanticModel)
        {
            ITypeSymbol targetElementSymbol = ElementIdentifierHelper.getTargetElementForClassMetrics(semanticModel, visitedAttribute.AncestorsAndSelf());

            string elementType = targetElementSymbol.TypeKind.ToString();
            string elementIdentifier = targetElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            string metricName = Metric.ATTRIBUTES_IN_CLASS.GetIdentifier();
            return new MetricResult(elementIdentifier, elementType, metricName, numberOfAttributes);
        }
    }
}

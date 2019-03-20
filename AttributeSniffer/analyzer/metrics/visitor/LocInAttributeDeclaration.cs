using System.Collections.Generic;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor
{
    /// <summary>
    /// Visit a compilation unit to extract the LOCAD metric.
    /// </summary>
    class LocInAttributeDeclaration : CSharpSyntaxWalker, MetricCollector
    {
        private SemanticModel SemanticModel { get; set; }
        private AttributeSyntax VisitedAttribute { get; set; }
        private int NumberOfLines { get; set; }
        private List<MetricResult> ResultsByElement { get; set; } = new List<MetricResult>();

        public override void VisitAttribute(AttributeSyntax node)
        {
            this.VisitedAttribute = node;
            NumberOfLines = node.GetText().Lines.Count;

            ResultsByElement.Add(GetResult());
        }

        public void SetSemanticModel(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;
        }

        public void SetResult(List<MetricResult> metricResults)
        { 
            metricResults.AddRange(ResultsByElement);
        }

        private MetricResult GetResult()
        {
            ISymbol parentElementSymbol = SemanticModel.GetDeclaredSymbol(VisitedAttribute.Parent.Parent);
            string parentIdentifier = parentElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);

            string elementType = "Attribute";
            string elementIdentifier = string.Format("{0}#{1}", parentIdentifier, VisitedAttribute.Name.ToString());
            string metricName = Metric.LOC_IN_ATTRIBUTE_DECLARATION.GetIdentifier();
            return new MetricResult(elementIdentifier, elementType, metricName, NumberOfLines);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor
{
    /// <summary>
    /// Visit a compilation unit to extract the UAC metric.
    /// </summary>
    class UniqueAttributesInClass : CSharpSyntaxWalker, MetricCollector
    {
        private List<AttributeSyntax> uniqueAttributes { get; set; } = new List<AttributeSyntax>();

        public override void VisitAttribute(AttributeSyntax node)
        {
            if (!uniqueAttributes.Exists(attribute => attribute.IsEquivalentTo(node)))
            {
                uniqueAttributes.Add(node);
            }
        }

        public string GetName()
        {
            return Metric.UNIQUE_ATTRIBUTES_IN_CLASS.GetIdentifier();
        }

        public string GetElementType()
        {
            return ElementType.CLASS.ToString();
        }

        public string GetElementIdentifier()
        {
            return "elementIdentifier";
        }

        public MetricResult GetResult()
        {
            return new MetricResult(GetElementType(), GetElementIdentifier(), GetName(), uniqueAttributes.Count);
        }
    }
}

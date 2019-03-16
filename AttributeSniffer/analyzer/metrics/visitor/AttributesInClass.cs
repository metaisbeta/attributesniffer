using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics
{
    /// <summary>
    /// Visit a compilation unit to extract the AC metric.
    /// </summary>
    class AttributesInClass : CSharpSyntaxWalker, MetricCollector
    {
        private int numberOfAttributes { get; set; } = 0;

        public override void VisitAttribute(AttributeSyntax node)
        {
            this.numberOfAttributes++;
        }

        public string GetName()
        {
            return Metric.ATTRIBUTES_IN_CLASS.GetIdentifier();
        }

        public int GetResult()
        {
            return this.numberOfAttributes;
        }
    }
}

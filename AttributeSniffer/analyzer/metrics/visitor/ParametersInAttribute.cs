using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace AttributeSniffer.analyzer.metrics.visitor
{
    /// <summary>
    /// Visit a compilation unit to extract the PA metric.
    /// </summary>
    class ParametersInAttribute : CSharpSyntaxWalker, MetricCollector
    {
        public string GetName()
        {
            return Metrics.ATTRIBUTES_IN_CLASS.GetIdentifier();
        }

        public int GetResult()
        {
            throw new NotImplementedException();
        }
    }
}

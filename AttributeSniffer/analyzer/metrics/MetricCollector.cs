using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;

namespace AttributeSniffer.analyzer.metrics
{
    /// <summary>
    /// Metric Collector interface. Should be implemented by all the metrics to be analyzed.
    /// </summary>
    interface MetricCollector
    {
        MetricResult GetResult(SemanticModel semanticModel);
    }
}

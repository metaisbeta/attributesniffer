using AttributeSniffer.analyzer.model;

namespace AttributeSniffer.analyzer.metrics
{
    /// <summary>
    /// Metric Collector interface. Should be implemented by all the metrics to be analyzed.
    /// </summary>
    interface MetricCollector
    {
        string GetElementType();
        string GetElementIdentifier();
        string GetName();

        MetricResult GetResult();
    }
}

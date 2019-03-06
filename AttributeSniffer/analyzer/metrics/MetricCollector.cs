namespace AttributeSniffer.analyzer.metrics
{
    /// <summary>
    /// Metric Collector interface. Should be implemented by all the metrics to be analyzed.
    /// </summary>
    interface MetricCollector
    {
        string GetName();
        int GetResult();
    }
}

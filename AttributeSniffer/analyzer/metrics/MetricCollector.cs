namespace AttributeSniffer.analyzer.metrics
{
    interface MetricCollector
    {
        string GetName();
        int GetResult();
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using AttributeSniffer.analyzer.metrics;

namespace AttributeSniffer.analyzer.model
{
    abstract class MetricsProperties
    {
        protected static List<Metric> metrics = new List<Metric>();

        // Metric's properties
        public readonly string identifier;

        public MetricsProperties(string identifier)
        {
            this.identifier = identifier;
            metrics.Add((Metric)this);
        }

        protected static ReadOnlyCollection<Metric> GetMetrics()
        {
            return metrics.AsReadOnly();
        }

        protected static Metric GetMetricByIdentifier(string identifier)
        {
            foreach (Metric metric in metrics)
            {
                if (metric.identifier == identifier)
                {
                    return metric;
                }
            }
            return null;
        }

        public string getMetricIdentifier()
        {
            return identifier;
        }
    }
}

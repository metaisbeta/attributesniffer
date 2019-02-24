using System.Collections.ObjectModel;
using AttributeSniffer.analyzer.model;

namespace AttributeSniffer.analyzer.metrics
{
    class Metric : MetricsProperties
    {
        // Metrics
        public static readonly Metric ATTRIBUTES_IN_CLASS = new Metric("AC");

        private Metric(string metricIdentifier) : base(metricIdentifier)
        {
        }
    }
}

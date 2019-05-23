using System;
using System.Collections.Generic;
using System.Text;

namespace AttributeSniffer.analyzer.metrics.model
{
    /// <summary>
    /// Struct to keep all the metrics types supported by the system.
    /// </summary>
    public struct MetricType
    {
        // Metric Types
        public static MetricType CLASS_METRIC
        {
            get { return new MetricType("Class"); }
        }

        public static MetricType ELEMENT_METRIC
        {
            get { return new MetricType("Element"); }
        }

        private string identifier;

        public string GetIdentifier() { return this.identifier; }

        public MetricType(string identifier)
        {
            this.identifier = identifier;
        }

        public static List<MetricType> GetMetricTypes()
        {
            return new List<MetricType>
            {
                CLASS_METRIC,
                ELEMENT_METRIC
            };
        }

        public static MetricType GetMetricType(string identifier)
        {
            return GetMetricTypes().Find(metricType => identifier.Equals(metricType.GetIdentifier()));
        }
    }
}

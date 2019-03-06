using System.Collections.Generic;

namespace AttributeSniffer.analyzer.metrics
{
    /// <summary>
    /// Struct to keep all the metrics supported by the system.
    /// </summary>
    public struct Metrics
    {
        // Metrics
        public static Metrics ATTRIBUTES_IN_CLASS
        {
            get { return new Metrics("AC"); }
        }

        private string identifier;

        public string GetIdentifier() { return this.identifier; }

        public Metrics(string identifier)
        {
            this.identifier = identifier;
        }

        public static List<Metrics> GetMetrics()
        {
            return new List<Metrics>
            {
                ATTRIBUTES_IN_CLASS
            };
        }

        public static Metrics GetMetric(string identifier)
        {
            return GetMetrics().Find(metric => identifier.Equals(metric.GetIdentifier()));
        }
    }
}

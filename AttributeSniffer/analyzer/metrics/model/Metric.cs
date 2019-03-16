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

        public static Metrics UNIQUE_ATTRIBUTES_IN_CLASS
        {
            get { return new Metrics("UAC"); }
        }

        public static Metrics ATTRIBUTES_IN_ELEMENT_DECLARATION
        {
            get { return new Metrics("UED"); }
        }

        public static Metrics PARAMETERS_IN_ATTRIBUTE
        {
            get { return new Metrics("PA"); }
        }

        public static Metrics LOC_IN_ATTRIBUTE_DECLARATION
        {
            get { return new Metrics("LOCAD"); }
        }

        public static Metrics ATTRIBUTE_NESTING_LEVEL
        {
            get { return new Metrics("ANL"); }
        }

        public static Metrics ATTRIBUTE_SCHEMAS_IN_CLASS
        {
            get { return new Metrics("ASC"); }
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

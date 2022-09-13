using System.Collections.Generic;

namespace AttributeSniffer.analyzer.metrics
{
    /// <summary>
    /// Struct to keep all the metrics supported by the system.
    /// </summary>
    public struct Metric
    {
        // Metrics
        //Class
        public static Metric METADATA_DECLARATION_IN_CLASS
        {
            get { return new Metric("MC"); }
        }
        public static Metric UNIQUE_METADATA_DECLARATION_IN_CLASS
        {
            get { return new Metric("UMC"); }
        }
        //Namespace
        public static Metric METADATA_SCHEMA_IN_CLASS
        {
            get { return new Metric("MSC"); }
        }
        //Code
        public static Metric METADATA_DECLARATION_IN_ELEMENT
        {
            get { return new Metric("MDE"); }
        }
        //Attributes
        public static Metric ARGUMENTS_IN_METADATA
        {
            get { return new Metric("AM"); }
        }

        public static Metric LOC_IN_METADATA_DECLARATION
        {
            get { return new Metric("LOCMD"); }
        }
        public static Metric NUMBER_OF_ELEMENTS_IN_CLASS
        {
            get { return new Metric("NEC"); }
        } 
        public static Metric NUMBER_OF_ELEMENTS_WITH_METADATA_IN_CLASS
        {
            get { return new Metric("NEMC"); }
        }
        private string identifier;

        public string GetIdentifier() { return this.identifier; }

        public Metric(string identifier)
        {
            this.identifier = identifier;
        }

        public static List<Metric> GetMetrics()
        {
            return new List<Metric>
            {
                METADATA_DECLARATION_IN_CLASS,
                UNIQUE_METADATA_DECLARATION_IN_CLASS,
                METADATA_DECLARATION_IN_ELEMENT,
                ARGUMENTS_IN_METADATA,
                LOC_IN_METADATA_DECLARATION,
                METADATA_SCHEMA_IN_CLASS,
                NUMBER_OF_ELEMENTS_WITH_METADATA_IN_CLASS,
                NUMBER_OF_ELEMENTS_IN_CLASS
            };
        }

        public static Metric GetMetric(string identifier)
        {
            return GetMetrics().Find(metric => identifier.Equals(metric.GetIdentifier()));
        }
    }
}

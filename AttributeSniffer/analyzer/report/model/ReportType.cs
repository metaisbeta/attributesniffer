using System;
using System.Collections.Generic;

namespace AttributeSniffer.analyzer.report
{
    /// <summary>
    /// Struct to keep all the report types supported by the system.
    /// </summary>
    class ReportType
    {
        // Report types
        public static ReportType JSON
        {
            get { return new ReportType("json", ".json"); }
        }
        public static ReportType XML
        {
            get { return new ReportType("xml", ".xml"); }
        }

        private string identifier;
        private string fileExtension;

        public string GetIdentifier() { return this.identifier; }

        public string GetExtension() { return this.fileExtension; }

        public ReportType(string identifier, string fileExtension)
        {
            this.identifier = identifier;
            this.fileExtension = fileExtension;
        }

        public static List<ReportType> GetReportTypes()
        {
            return new List<ReportType>
            {
                JSON, XML
            };
        }

        public static ReportType GetReportType(string identifier)
        {
            return GetReportTypes().Find(type => identifier.Equals(type.GetIdentifier(), StringComparison.OrdinalIgnoreCase));
        }
    }
}

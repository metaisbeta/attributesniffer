using System.Collections.Generic;

namespace AttributeSniffer.analyzer.report
{
    class ReportType
    {
        // Report types
        public static ReportType JSON
        {
            get { return new ReportType("json"); }
        }
        public static ReportType XML
        {
            get { return new ReportType("xml"); }
        }

        private string identifier;

        public string GetIdentifier() { return this.identifier; }

        public ReportType(string identifier)
        {
            this.identifier = identifier;
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
            return GetReportTypes().Find(type => identifier.Equals(type.GetIdentifier()));
        }
    }
}

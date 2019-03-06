using System;
using System.Collections.Generic;
using System.Text;

namespace AttributeSniffer.analyzer.report
{
    class Report
    {
        public string ReportContent { get; set; }
        public ReportType ReportType { get; set; }

        public Report(string reportContent, ReportType reportType)
        {
            ReportContent = reportContent;
            ReportType = reportType;
        }
    }
}

namespace AttributeSniffer.analyzer.report
{
    /// <summary>
    /// Represents the report already processed.
    /// </summary>
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

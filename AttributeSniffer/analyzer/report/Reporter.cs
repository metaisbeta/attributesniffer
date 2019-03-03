using AttributeSniffer.analyzer.model;

namespace AttributeSniffer.analyzer.report
{
    class Reporter
    {
        public string report(ProjectReport projectReport, string reportType, string pathToSave)
        {
            string reportConverted = new ReportConverter().convert(projectReport, reportType);
            new ReportWriter().write(pathToSave, projectReport.ProjectName, reportConverted);

            return reportConverted;
        }
    }
}

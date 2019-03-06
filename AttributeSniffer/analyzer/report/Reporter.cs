using AttributeSniffer.analyzer.model;

namespace AttributeSniffer.analyzer.report
{
    /// <summary>
    /// Reporter component.
    /// </summary>
    class Reporter
    {
        /// <summary>
        /// Process the project report.
        /// </summary>
        /// <param name="projectReport">Project report.</param>
        /// <param name="reportType">Report type.</param>
        /// <param name="pathToSave">Path where to save the report.</param>
        /// <returns>The project report content converted to the report type.</returns>
        public string Report(ProjectReport projectReport, string reportType, string pathToSave)
        {
            Report report = new ReportConverter().Convert(projectReport, reportType);
            new ReportWriter().Write(pathToSave, projectReport.ProjectName, report);

            return report.ReportContent;
        }
    }
}

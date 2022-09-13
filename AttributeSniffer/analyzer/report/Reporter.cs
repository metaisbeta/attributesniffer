using AttributeSniffer.analyzer.metrics;
using AttributeSniffer.analyzer.model;
using System;

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
        /// <param name="reportType">Report type.</param>,
        /// <param name="pathToSave">Path where to save the report.</param>
        /// <returns>The project report content converted to the report type.</returns>
        public void Report(ProjectReport projectReport, string reportType, string pathToSave)
        {
            if (!ReportType.CSV.GetIdentifier().Equals(reportType))
            {
                Report report = new ReportConverter().Convert(projectReport, reportType);
                new ReportWriter().Write(pathToSave, projectReport.ProjectName, report);
            }
            else
            {
                foreach (var metric in Metric.GetMetrics())
                {
                    Report report = new ReportConverter().Convert(projectReport, reportType, metric.GetIdentifier().ToString());
                    new ReportWriter().Write(pathToSave + System.IO.Path.DirectorySeparatorChar + projectReport.ProjectName, 
                        string.Concat(projectReport.ProjectName, "_", metric.GetIdentifier().ToString()), report);
                }
            }
        }
    }
}

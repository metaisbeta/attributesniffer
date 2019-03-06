using System.IO;

namespace AttributeSniffer.analyzer.report
{
    class ReportWriter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Write the project report to a file.
        /// </summary>
        /// <param name="path">Path where to save the report.</param>
        /// <param name="projectName">Project name.</param>
        /// <param name="report">Report already converted.</param>
        public void Write(string path, string projectName, Report report)
        {
            string completePath = path + Path.DirectorySeparatorChar + projectName + report.ReportType.GetExtension();
            Directory.CreateDirectory(path);
            File.WriteAllText(completePath, report.ReportContent);
            logger.Info("Report saved {}.", completePath);

        }
    }
}

using System.IO;

namespace AttributeSniffer.analyzer.report
{
    class ReportWriter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        const string FILE_EXTENSION = ".txt";

        public void write(string path, string projectName, string report)
        {
            string completePath = path + Path.DirectorySeparatorChar + projectName + FILE_EXTENSION;
            Directory.CreateDirectory(path);
            File.WriteAllText(completePath, report);
            logger.Info("Report saved {}.", completePath);

        }
    }
}

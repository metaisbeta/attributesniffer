using System;
using System.IO;
using AttributeSniffer.analyzer;
using AttributeSniffer.analyzer.model;
using AttributeSniffer.analyzer.report;

namespace AttributeSniffer
{
    /// <summary>
    /// Attribute sniffer starter.
    /// </summary>
    class AttributeSnifferRunner
    {
        /// <summary>
        /// Analyze a single project.
        /// </summary>
        /// <param name="pathToAnalyze">path to the classes to analyze</param>
        /// <param name="reportPath">path to save the report</param>
        /// <param name="reportType">report's file type</param>
        public void AnalyzeSingle(string pathToAnalyze, string reportPath, string reportType)
        {
            // Analyze project
            Console.WriteLine("Starting to analyze project at path: " + pathToAnalyze);
            ProjectReport projectReport = new Sniffer().Sniff(pathToAnalyze);

            // Process report and print to console
            new Reporter().Report(projectReport, reportType, reportPath);
            Console.WriteLine("Finished analyzing project: " + projectReport.ProjectName);
        }

        /// <summary>
        /// Analyze multiple projects.
        /// </summary>
        /// <param name="pathToAnalyze">path to the classes to analyze</param>
        /// <param name="reportPath">path to save the report</param>
        /// <param name="reportType">report's file type</param>
        public void AnalyzeMulti(string pathToAnalyze, string reportPath, string reportType)
        {
            // Analyze multiple projects
            string[] projectDirectories = Directory.GetDirectories(pathToAnalyze);

            foreach (string projectDirectory in projectDirectories)
            {
                AnalyzeSingle(projectDirectory, reportPath, reportType);
            }
        }


    }
}

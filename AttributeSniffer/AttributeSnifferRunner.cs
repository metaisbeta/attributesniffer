﻿using System;
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
        /// To be used by external access.
        /// </summary>
        /// <param name="pathToAnalyze">path to the classes to analyze</param>
        /// <param name="reportPath">path to save the report</param>
        /// <param name="reportType">report's file type</param>
        public void Analyze(string pathToAnalyze, string reportPath, string reportType)
        {
            // Analyse project
            ProjectReport projectReport = new Sniffer().Sniff(pathToAnalyze);

            // Process report and print to console
            string report = new Reporter().Report(projectReport, reportType, reportPath);
            Console.WriteLine(report);
            Console.ReadLine();
        }

        /// <summary>
        /// To be used by unit testes.
        /// </summary>
        /// <param name="reportType">report's file type</param>
        public void Run(string reportType)
        {
            // Get the current working directory
            string currentWd = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\.."));
            string pathToAnalyze = currentWd + Path.DirectorySeparatorChar + "example";
            string reportPath = currentWd + Path.DirectorySeparatorChar + "report";

            Analyze(pathToAnalyze, reportPath, reportType);
        }
    }
}

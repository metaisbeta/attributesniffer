using System;

namespace AttributeSniffer
{
    /// <summary>
    /// Program runner.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string pathToAnalyze = "";
            string reportPath = "";
            string projectType = "";
            string reportType = "json";

            if (args.Length >= 2)
            {
                pathToAnalyze = args[0];
                reportPath = args[1];
                projectType = args[2];

                if (args.Length > 2)
                {
                    reportType = args[2];
                }
            }
            else
            {
                Console.WriteLine("Path to analyze: ");
                pathToAnalyze = Console.ReadLine();

                Console.WriteLine("Path to save the report: ");
                reportPath = Console.ReadLine();

                Console.WriteLine("Project type [single/multi]:");
                projectType = Console.ReadLine();

                Console.WriteLine("File type of the report [xml/json]:");
                reportType = Console.ReadLine();
            }


            if (!String.IsNullOrEmpty(pathToAnalyze) && !String.IsNullOrEmpty(reportPath) && !String.IsNullOrEmpty(projectType))
            {
                AttributeSnifferRunner runner = new AttributeSnifferRunner();
                if (projectType.Equals("single", StringComparison.OrdinalIgnoreCase)) {
                    runner.AnalyzeSingle(pathToAnalyze, reportPath, reportType);
                } else
                {
                    runner.AnalyzeMulti(pathToAnalyze, reportPath, reportType);
                }
            }
            else
            {
                Console.WriteLine("Provide a path to analyze the code, to save the report and the project type!");
                Console.ReadLine();
            }
        }
    }
}

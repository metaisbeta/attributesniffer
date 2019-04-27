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
            string reportType = "";

            if (args.Length >= 2)
            {
                pathToAnalyze = args[0];
                reportPath = args[1];

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

                Console.WriteLine("File type of the report [xml/json]:");
                reportType = Console.ReadLine();
            }


            if (!String.IsNullOrEmpty(pathToAnalyze) && !String.IsNullOrEmpty(reportPath))
            {
                new AttributeSnifferRunner().Analyze(pathToAnalyze, reportPath, reportType);
            }
            else
            {
                Console.WriteLine("Provide a path to analyze the code and to save the report!");
                Console.ReadLine();
            }
        }
    }
}

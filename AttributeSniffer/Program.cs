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
            Console.WriteLine("Path to analyze: ");
            string pathToAnalyze = Console.ReadLine();

            Console.WriteLine("Path to save the report: ");
            string reportPath = Console.ReadLine();

            Console.WriteLine("File type of the report [xml/json]:");
            string reportType = Console.ReadLine();

            new AttributeSnifferRunner().Analyze(pathToAnalyze, reportPath, reportType);
        }
    }
}

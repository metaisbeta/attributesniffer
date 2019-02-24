using System;

namespace AttributeSniffer
{
    class Program
    {

        static void Main(string[] args)
        {
            //string pathToAnalyze = args[0];
            //string reportPath = args[1];
            //string reportType = args[2];

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

using System;
using System.Diagnostics;
using System.IO;
using AttributeSniffer.analyzer;
using AttributeSniffer.analyzer.model;
using AttributeSniffer.analyzer.report;

namespace AttributeSniffer
{
    class Program{
        static void Main(string[] args)
        {
            //Get the current working directory. 
            string currentWd = Directory.GetCurrentDirectory();
            string pathToAnalyze = currentWd + 
                Path.DirectorySeparatorChar + "example";

            ProjectReport result = new Sniffer().Sniff(pathToAnalyze);
            //Convert to string
            string resultConverted = new ReportConverter().convert(result); 
            Console.WriteLine(resultConverted);
            //Console.ReadLine();
        }
    }
}

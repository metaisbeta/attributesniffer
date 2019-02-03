using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using AttributeSniffer.analyzer;

namespace AttributeSniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Path:");
            //String pathToAnalyze = Console.ReadLine();

            String pathToAnalyze = "C:\\Users\\Lydia\\source\\repos\\AttributeSniffer\\AttributeSniffer\\example\\classes";

            String result = new Sniffer().Sniff(pathToAnalyze);

            Console.WriteLine(result);

            Console.ReadLine();
        }
    }
}

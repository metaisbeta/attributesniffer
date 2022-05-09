using System;
using Xunit;
using AttributeSniffer.analyzer;
using System.IO;
using AttributeSniffer.analyzer.model;

namespace AttributeSnifferTest
{
    public class SnifferTest
    {

        public string Path { get; set; }
        public SnifferTest()
        {
            //Path = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\test";
            Path = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent}\\PowerShell-master\\src";
        }

        [Fact]
        public void SniffSucess()
        {
            
            Sniffer sniffer = new Sniffer();
            ProjectReport projectReport = sniffer.Sniff(Path);
            Assert.Equal(37, projectReport.MetricsResults.Count);
            Assert.Equal(3, projectReport.ClassesNumber);
            Assert.Equal(10, projectReport.AttributesNumber);
        }
        
    }
}

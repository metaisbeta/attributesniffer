using AttributeSniffer.analyzer;
using AttributeSniffer.analyzer.model;
using System;
using System.IO;
using Xunit;

namespace AttributeSnifferTest
{
    public class SnifferTest
    {

        public string Path { get; set; }
        public SnifferTest()
        {
            Path = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}/test";
  
        }

        [Fact]
        public void SniffSucess()
        {            
            Sniffer sniffer = new Sniffer();
            ProjectReport projectReport = sniffer.Sniff(Path);
            Assert.Equal(52, projectReport.MetricsResults.Count);
            Assert.Equal(5, projectReport.NamespaceMetricsAllProject.Count);
            Assert.Equal(14, projectReport.NamespacesPerClassResults.Count);
            Assert.Equal(3, projectReport.ClassesNumber);
            Assert.Equal(14, projectReport.AttributesNumber);
        }
        
    }
}

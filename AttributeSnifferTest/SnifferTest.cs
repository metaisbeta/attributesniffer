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
            //Path = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\test";
            //Path = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent}\\PowerShell-master\\src";
            Path = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.Parent.Parent}\\Área de Trabalho\\TCC\\shadowsocks-windows-main\\shadowsocks-windows-main";

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

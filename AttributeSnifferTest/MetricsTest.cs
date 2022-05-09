using AttributeSniffer.analyzer;
using System;
using Xunit;
using System.IO;
using AttributeSniffer.analyzer.model;
using System.Collections.Generic;
using System.Linq;
using AttributeSniffer.analyzer.metrics;

namespace AttributeSnifferTest
{
    public class MetricsTest
    {
        private readonly MetricsCollector metricsCollector;
        public string Path { get; set; }

        public MetricsTest()
        {
            this.metricsCollector = new MetricsCollector();
            Path = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\test";
        }
        [Fact]
        public void MetricCollectACAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Single(acMetrics.Where(a => a.Metric == Metric.ATTRIBUTES_IN_CLASS.GetIdentifier()));
        }

        [Fact]
        public void MetricCollectUACAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Single(acMetrics.Where(a => a.Metric == Metric.UNIQUE_ATTRIBUTES_IN_CLASS.GetIdentifier()));
        }
        [Fact]
        public void MetricCollectAEDAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Equal(6, acMetrics.Where(a => a.Metric == Metric.ATTRIBUTES_IN_ELEMENT_DECLARATION.GetIdentifier()).Count());
        }
        [Fact]
        public void MetricCollectAAAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Equal(8, acMetrics.Where(a => a.Metric == Metric.ARGUMENTS_IN_ATTRIBUTE.GetIdentifier()).Count());
        }
        [Fact]
        public void MetricCollectLocadAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Equal(8, acMetrics.Where(a => a.Metric == Metric.LOC_IN_ATTRIBUTE_DECLARATION.GetIdentifier()).Count());
        }
    }
}

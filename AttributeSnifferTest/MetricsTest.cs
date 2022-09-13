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
        public void MetricCollectMCAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Single(acMetrics.Where(a => a.Metric == Metric.METADATA_DECLARATION_IN_CLASS.GetIdentifier()));
        }

        [Fact]
        public void MetricCollectUMCAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Single(acMetrics.Where(a => a.Metric == Metric.UNIQUE_METADATA_DECLARATION_IN_CLASS.GetIdentifier()));
        }
        [Fact]
        public void MetricCollectMDEAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Equal(8, acMetrics.Where(a => a.Metric == Metric.METADATA_DECLARATION_IN_ELEMENT.GetIdentifier()).Count());
        }
        [Fact]
        public void MetricCollectAMAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Equal(10, acMetrics.Where(a => a.Metric == Metric.ARGUMENTS_IN_METADATA.GetIdentifier()).Count());
        }
        [Fact]
        public void MetricCollectMSCAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Equal(4, acMetrics.Where(a => a.Metric == Metric.METADATA_SCHEMA_IN_CLASS.GetIdentifier()).Distinct(x => x.ElementIdentifier.ElementName).Count());
        }
        [Fact]
        public void MetricCollectLocmdAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Equal(10, acMetrics.Where(a => a.Metric == Metric.LOC_IN_METADATA_DECLARATION.GetIdentifier()).Count());
        }
        [Fact]
        public void MetricCollectNEMCAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Equal(10, acMetrics.Where(a => a.Metric == Metric.NUMBER_OF_ELEMENTS_WITH_METADATA_IN_CLASS.GetIdentifier()).FirstOrDefault().Result);
        }
        [Fact]
        public void MetricCollectNECAssert()
        {
            string fullPath = $"{Path}\\classes\\ClassTeste2.cs";
            string classContent = File.ReadAllText(fullPath);
            List<MetricResult> acMetrics = metricsCollector.Collect(fullPath, classContent);
            Assert.Equal(21, acMetrics.Where(a => a.Metric == Metric.NUMBER_OF_ELEMENTS_IN_CLASS.GetIdentifier()).FirstOrDefault().Result);
        }

    }
}

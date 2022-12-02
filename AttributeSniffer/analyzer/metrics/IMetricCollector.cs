using System;
using System.Collections.Generic;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;

namespace AttributeSniffer.analyzer.metrics
{
    /// <summary>
    /// IMetric Collector interface. Should be implemented by all the metrics to be analyzed.
    /// </summary>
    interface IMetricCollector : IDisposable
    {
        void SetSemanticModel(SemanticModel semanticModel);
        void SetResult(List<MetricResult> metricResults);
        void SetFilePath(string filePath);
    }
}

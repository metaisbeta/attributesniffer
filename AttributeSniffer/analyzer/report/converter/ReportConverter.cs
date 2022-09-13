using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AttributeSniffer.analyzer.model;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace AttributeSniffer.analyzer.report
{
    /// <summary>
    /// Report Converter component.
    /// </summary>
    class ReportConverter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Convert the project report to a specific type.
        /// </summary>
        /// <param name="projectReport">Project report.</param>
        /// <param name="reportType">Report type.</param>
        /// <returns>The project report converted to the report type.</returns>
        public Report Convert(ProjectReport projectReport, string reportType)
        {
            return Convert(projectReport, reportType, null);
        }
        public Report Convert(ProjectReport projectReport, string reportType, string metric)
        {
            if (ReportType.JSON.GetIdentifier().Equals(reportType))
            {
                logger.Info("Converting project report to JSON.");
                return new Report(CreateJSON(projectReport), ReportType.JSON);
            }
            else if (ReportType.XML.GetIdentifier().Equals(reportType))
            {
                logger.Info("Converting project report to XML.");
                return new Report(CreateXML(projectReport), ReportType.XML);
            }
            else if (ReportType.CSV.GetIdentifier().Equals(reportType))
            {
                logger.Info($"Converting project report to CSV - Metric:{metric}.");
                return new Report(CreateCSV(projectReport, metric), ReportType.CSV);
            }
            else
            {
                logger.Info("A report type could not be found. Converting project report to JSON.");
                return new Report(CreateJSON(projectReport), ReportType.JSON);
            }
        }

        private string CreateCSV(ProjectReport projectReport, string metric)
        {
            List<int> valuesPerMetric = projectReport.MetricsResults.Where(x => x.Metric.Equals(metric)).Select(x=> x.Result).ToList();
            valuesPerMetric.Sort();
            return string.Join(",", valuesPerMetric);
        }

        private string CreateJSON(ProjectReport projectReport)
        {
            return JsonConvert.SerializeObject(projectReport, Formatting.Indented);
        }

        private string CreateXML(ProjectReport projectReport)
        {
            IExtendedXmlSerializer serializer = new ConfigurationContainer().EnableReferences().Create();
            return serializer.Serialize(new XmlWriterSettings { Indent = true }, projectReport);
        }
    }
}

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
            if (ReportType.JSON.GetIdentifier().Equals(reportType))
            {
                logger.Info("Converting project report to JSON.");
                return new Report(CreateJSON(projectReport), ReportType.JSON);
            } else if (ReportType.XML.GetIdentifier().Equals(reportType))
            {
                logger.Info("Converting project report to XML.");
                return new Report(CreateXML(projectReport), ReportType.XML);
            } else
            {
                logger.Info("A report type could not be found. Converting project report to JSON.");
                return new Report(CreateJSON(projectReport), ReportType.JSON);
            }  
        }

        private string CreateJSON(ProjectReport projectReport)
        {
            return JsonConvert.SerializeObject(projectReport, Formatting.Indented);
        }

        private string CreateXML(ProjectReport projectReport)
        {
            IExtendedXmlSerializer serializer = new ConfigurationContainer()
                .ConfigureType<ProjectReport>()
                .EnableImplicitTyping(typeof(ProjectReport))
                .Create();
            return serializer.Serialize(new XmlWriterSettings { Indent = true }, projectReport);
        }
    }
}

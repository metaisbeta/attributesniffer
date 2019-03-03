using System.Xml;
using AttributeSniffer.analyzer.model;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace AttributeSniffer.analyzer.report
{
    class ReportConverter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string convert(ProjectReport projectReport, string reportType)
        {
            if (ReportType.JSON.GetIdentifier().Equals(reportType))
            {
                logger.Info("Converting project report to JSON.");
                return CreateJSON(projectReport);
            } else if (ReportType.XML.GetIdentifier().Equals(reportType))
            {
                logger.Info("Converting project report to XML.");
                return CreateXML(projectReport);
            } else
            {
                logger.Info("A report type could not be found. Converting project report to JSON.");
                return CreateJSON(projectReport);
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

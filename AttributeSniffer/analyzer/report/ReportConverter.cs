using AttributeSniffer.analyzer.model;
using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.report
{
    class ReportConverter
    {
        public string convert(ProjectReport projectReport)
        {
            return JsonConvert.SerializeObject(projectReport);
        }
    }
}

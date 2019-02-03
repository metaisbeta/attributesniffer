using AttributeSniffer.analyzer.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

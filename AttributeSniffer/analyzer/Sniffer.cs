using AttributeSniffer.analyzer.classMetrics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AttributeSniffer.analyzer
{
    class Sniffer
    {
        public String Sniff(string folderPath)
        {
            List<ClassMetrics> collectedMetrics = new List<ClassMetrics>();
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.cs"))
            {
                FileStream classContent = File.OpenRead(file);

                // Coletar métricas
            }

            return JsonConvert.SerializeObject(collectedMetrics);
        }
    }
}

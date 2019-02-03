using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeSniffer.analyzer.metrics
{
    interface MetricCollector
    {
        string getName();
        int getResult();
    }
}

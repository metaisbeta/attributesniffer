using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeSniffer.analyzer.classMetrics
{
    class ClassMetrics
    {
        private string className { get; set; }
        private int numberOfAttributes { get; set; }

        public ClassMetrics(string className, int numberOfAttributes)
        {
            this.className = className;
            this.numberOfAttributes = numberOfAttributes;
        }
    }
}

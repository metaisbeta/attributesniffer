using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeSniffer.example.customAttribute
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    class Attribute1 : System.Attribute
    {
    }
}

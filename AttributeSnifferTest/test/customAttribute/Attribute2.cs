using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeSniffer.example.customAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class Attribute2 : System.Attribute
    {
    }
}

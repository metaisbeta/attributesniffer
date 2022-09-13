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
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = true)]
    class Attribute3 : System.Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Enum)]
    class Attribute4 : System.Attribute
    {
    }
}

using System;

namespace AttributeSniffer.example.customAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class Attribute2 : Attribute
    {
    }
}

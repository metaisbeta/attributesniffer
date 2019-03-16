using System;
using System.Collections.Generic;
using System.Text;

namespace AttributeSniffer.example.customAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class Attribute3 : Attribute
    {
        public int[] Values { get; set; } = new int[] { 1, 2, 3 };

        public Attribute3(params int[] values)
        {
            this.Values = values;
        }
    }
}

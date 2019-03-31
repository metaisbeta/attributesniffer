using System;
using System.Collections.Generic;
using System.Text;
using AttributeSniffer.example.customAttribute;

namespace AttributeSniffer.example.classe
{
    [Attribute1]
    class ClassTest
    {
        [Attribute1]
        public string property { get; set; }

        [Attribute1]
        public string field1, field2, field3;

        [Attribute1]
        public enum enumField
        {
            enum1,
            enum2
        }

        [Attribute2]
        [Attribute3(
            2,
            3,
            4
        )]
        public void test1()
        {
        }

        [return: Attribute1]
        [Attribute3(2, 3, 4)]
        public string test2([Attribute1] string param)
        {
            return "";
        }

        [Attribute3(new int[] { 2, 3, 4 })]
        public void test3()
        {
        }

        [Attribute3(Values = new int[] { 2, 3, 4 })]
        public void test4()
        {
        }
    }
}

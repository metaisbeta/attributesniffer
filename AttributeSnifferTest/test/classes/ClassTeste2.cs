using AttributeSniffer.example.customAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeSniffer.example.classes
{
    [Attribute1]
    class ClassTeste2
    {
        [Attribute3]
        public ClassTeste2()
        {

        }

        [Attribute2]
        [Attribute2]
        public void teste1()
        {
        }

        [Attribute2]
        public void teste2()
        {

        }
    }
    [Attribute1]
    class ClassTeste1
    {
        [Attribute4]
        enum MyEnum
        {
            a, b, c, d, e
        }

        [Attribute2]
        [Attribute2]
        public void teste3()
        {
            ClassTeste2 classTeste2 = new ClassTeste2();
        }

        [Attribute2]
        public int teste4()
        { 
            return 0;
        }
    }
}

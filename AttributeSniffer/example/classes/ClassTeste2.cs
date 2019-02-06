using AttributeSniffer.example.customAttribute;

namespace AttributeSniffer.example.classes
{
    [Attribute1]
    class ClassTeste2
    {
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
}

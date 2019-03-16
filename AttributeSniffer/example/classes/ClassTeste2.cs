using AttributeSniffer.example.customAttribute;

namespace AttributeSniffer.example.classes
{
    [Attribute1]
    class ClassTeste2
    {
        [Attribute2]
        [Attribute2]
        [Attribute3(1, 2, 3)]
        public void teste1()
        {
        }

        [Attribute2]
        [Attribute3(Values = new int[] { 1, 2, 3 })]
        [Attribute3(1, 2, 3)]
        public void teste2()
        {
        }
    }
}

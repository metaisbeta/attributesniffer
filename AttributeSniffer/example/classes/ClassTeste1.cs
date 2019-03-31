using AttributeSniffer.example.customAttribute;

namespace AttributeSniffer.example.classes
{
    [Attribute1]
    class ClassTeste1
    {
        [Attribute1]
        public string property { get; set; }

        [Attribute1]
        public string field1, field2, field3;

        [Attribute1]
        public enum enumTest
        {
            test
        }
    }
}

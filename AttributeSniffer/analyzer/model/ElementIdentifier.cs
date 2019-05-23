using System;
using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.model
{
    /// <summary>
    /// Represents a element identifier. Includes the element name (with its namespace), type, the line number and the file declaration's path.
    /// </summary>
    public class ElementIdentifier
    {
        [JsonProperty]
        public string ElementName { get; set; }

        [JsonProperty]
        public string ElementType { get; set; }

        [JsonProperty]
        public int LineNumber { get; set; }

        [JsonProperty]
        public string FileDeclarationPath { get; set; }

        public ElementIdentifier()
        {
            // For serialization
        }

        public ElementIdentifier(string elementName, string elementType, int lineNumber, string fileDeclarationPath)
        {
            ElementName = elementName;
            ElementType = elementType;
            LineNumber = lineNumber;
            FileDeclarationPath = fileDeclarationPath;
        }

        public override bool Equals(object obj)
        {
            var identifier = obj as ElementIdentifier;
            return identifier != null &&
                   ElementName == identifier.ElementName &&
                   ElementType == identifier.ElementType &&
                   LineNumber == identifier.LineNumber &&
                   FileDeclarationPath == identifier.FileDeclarationPath;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ElementName, ElementType, LineNumber, FileDeclarationPath);
        }
    }
}

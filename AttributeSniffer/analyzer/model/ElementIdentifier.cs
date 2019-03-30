using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AttributeSniffer.analyzer.model
{
    /// <summary>
    /// Represents a element identifier. Includes the element name (with its namespace), type and the line number.
    /// </summary>
    public class ElementIdentifier
    {
        [JsonProperty]
        public string ElementName { get; set; }

        [JsonProperty]
        public string ElementType { get; set; }

        [JsonProperty]
        public int LineNumber { get; set; }

        public ElementIdentifier()
        {
            // For serialization
        }

        public ElementIdentifier(string elementName, string elementType, int lineNumber)
        {
            ElementName = elementName;
            ElementType = elementType;
            LineNumber = lineNumber;
        }

        public override bool Equals(object obj)
        {
            var identifier = obj as ElementIdentifier;
            return identifier != null &&
                   ElementName == identifier.ElementName &&
                   ElementType == identifier.ElementType &&
                   LineNumber == identifier.LineNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ElementName, ElementType, LineNumber);
        }
    }
}

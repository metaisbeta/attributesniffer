using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor.util
{
    class ElementIdentifierType
    {
        // Element types
        public static ElementIdentifierType STRUCT_TYPE
        {
            get { return new ElementIdentifierType(typeof(StructDeclarationSyntax), "Struct"); }
        }

        public static ElementIdentifierType CLASS_TYPE
        {
            get { return new ElementIdentifierType(typeof(ClassDeclarationSyntax), "Class"); }
        }

        public static ElementIdentifierType INTERFACE_TYPE
        {
            get { return new ElementIdentifierType(typeof(InterfaceDeclarationSyntax), "Interface"); }
        }

        public static ElementIdentifierType ENUM_TYPE
        {
            get { return new ElementIdentifierType(typeof(EnumDeclarationSyntax), "Enum"); }
        }

        public static ElementIdentifierType METHOD_TYPE
        {
            get { return new ElementIdentifierType(typeof(MethodDeclarationSyntax), "Method"); }
        }       

        public static ElementIdentifierType EVENT_TYPE
        {
            get { return new ElementIdentifierType(typeof(EventDeclarationSyntax), "Event"); }
        }

        public static ElementIdentifierType PROPERTY_TYPE
        {
            get { return new ElementIdentifierType(typeof(PropertyDeclarationSyntax), "Property"); }
        }

        public static ElementIdentifierType FIELD_TYPE
        {
            get { return new ElementIdentifierType(typeof(FieldDeclarationSyntax), "Field"); }
        }

        public static ElementIdentifierType PARAMETER_TYPE
        {
            get { return new ElementIdentifierType(typeof(ParameterSyntax), "Parameter"); }
        }

        public static ElementIdentifierType RETURN_TYPE
        {
            get { return new ElementIdentifierType(typeof(ReturnStatementSyntax), "Return"); }
        }

        private Type type;
        private string typeIdentifier;

        public string GetTypeIdentifier() { return this.typeIdentifier; }
        public Type GetElementType() { return this.type; }

        public ElementIdentifierType(Type type, string typeIdentifier)
        {
            this.type = type;
            this.typeIdentifier = typeIdentifier;
        }

        public static List<ElementIdentifierType> GetElementIdentifierTypes()
        {
            return new List<ElementIdentifierType>
            {
                STRUCT_TYPE,
                CLASS_TYPE,
                INTERFACE_TYPE,
                ENUM_TYPE,
                METHOD_TYPE,
                EVENT_TYPE,
                PROPERTY_TYPE,
                FIELD_TYPE,
                PARAMETER_TYPE,
                RETURN_TYPE
            };
        }

        public static ElementIdentifierType GetElementType(Type type)
        {
            return GetElementIdentifierTypes().Find(elementType => type.Equals(elementType.GetElementType()));
        }
    }
}

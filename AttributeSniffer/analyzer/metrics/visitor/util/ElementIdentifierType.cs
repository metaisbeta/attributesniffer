using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor.util
{
    struct ElementIdentifierType
    {
        // Element types
        public static ElementIdentifierType CS_FILE_TYPE
        {
            get { return new ElementIdentifierType(null, "csFile", "csFile"); }
        }

        public static ElementIdentifierType ASSEMBLY_TYPE
        {
            get { return new ElementIdentifierType(null, "assembly", "Assembly"); }
        }

        public static ElementIdentifierType MODULE_TYPE
        {
            get { return new ElementIdentifierType(null, "module", "Module"); }
        }
        public static ElementIdentifierType NUMBER_OF_ELEMENTS
        {
            get { return new ElementIdentifierType(null, "number", "numberOfElements"); }
        }

        public static ElementIdentifierType STRUCT_TYPE
        {
            get { return new ElementIdentifierType(typeof(StructDeclarationSyntax), "type", "Struct"); }
        }

        public static ElementIdentifierType CLASS_TYPE
        {
            get { return new ElementIdentifierType(typeof(ClassDeclarationSyntax), "type", "Class"); }
        }

        public static ElementIdentifierType INTERFACE_TYPE
        {
            get { return new ElementIdentifierType(typeof(InterfaceDeclarationSyntax), "type", "Interface"); }
        }

        public static ElementIdentifierType ENUM_TYPE
        {
            get { return new ElementIdentifierType(typeof(EnumDeclarationSyntax), "type", "Enum"); }
        }

        public static ElementIdentifierType DELEGATE_TYPE
        {
            get { return new ElementIdentifierType(typeof(DelegateDeclarationSyntax), "type", "Delegate"); }
        }

        public static ElementIdentifierType METHOD_TYPE
        {
            get { return new ElementIdentifierType(typeof(MethodDeclarationSyntax), "method", "Method"); }
        }       

        public static ElementIdentifierType EVENT_TYPE
        {
            get { return new ElementIdentifierType(typeof(EventDeclarationSyntax), "event", "Event"); }
        }

        public static ElementIdentifierType PROPERTY_TYPE
        {
            get { return new ElementIdentifierType(typeof(PropertyDeclarationSyntax), "property", "Property"); }
        }

        public static ElementIdentifierType FIELD_TYPE
        {
            get { return new ElementIdentifierType(typeof(FieldDeclarationSyntax), "field", "Field"); }
        }

        public static ElementIdentifierType PARAMETER_TYPE
        {
            get { return new ElementIdentifierType(typeof(ParameterSyntax), "param", "Parameter"); }
        }

        public static ElementIdentifierType TYPE_PARAMETER_TYPE
        {
            get { return new ElementIdentifierType(typeof(TypeParameterSyntax), "typevar", "Typevar"); }
        }

        public static ElementIdentifierType RETURN_TYPE
        {
            get { return new ElementIdentifierType(typeof(ReturnStatementSyntax), "return", "Return"); }
        }        
        public static ElementIdentifierType NAMESPACE_TYPE
        {
            get { return new ElementIdentifierType(typeof(UsingDirectiveSyntax), "namespace", "Namespace"); }
        }

        public static ElementIdentifierType CONSTRUCTOR_TYPE
        {
            get { return new ElementIdentifierType(typeof(ConstructorDeclarationSyntax), "constructor", "Constructor"); }
        }

        private Type type;
        private string target;
        private string typeIdentifier;

        public Type GetElementType() { return this.type; }
        public string GetElementTarget() { return this.target; }
        public string GetTypeIdentifier() { return this.typeIdentifier; }

        public ElementIdentifierType(Type type, string target, string typeIdentifier)
        {
            this.type = type;
            this.target = target;
            this.typeIdentifier = typeIdentifier;
        }

        public static List<ElementIdentifierType> GetElementIdentifierTypes()
        {
            return new List<ElementIdentifierType>
            {
                CS_FILE_TYPE,
                ASSEMBLY_TYPE,
                MODULE_TYPE,
                STRUCT_TYPE,
                CLASS_TYPE,
                INTERFACE_TYPE,
                ENUM_TYPE,
                METHOD_TYPE,
                EVENT_TYPE,
                PROPERTY_TYPE,
                FIELD_TYPE,
                PARAMETER_TYPE,
                TYPE_PARAMETER_TYPE,
                RETURN_TYPE,
                NAMESPACE_TYPE,
                NUMBER_OF_ELEMENTS,
                CONSTRUCTOR_TYPE
            };
        }

        public static ElementIdentifierType GetElementByType(Type type)
        {
            return GetElementIdentifierTypes().Find(elementType => type.Equals(elementType.GetElementType()));
        }

        public static List<ElementIdentifierType> GetElementsByTarget(string target)
        {
            return GetElementIdentifierTypes().FindAll(elementType => target.Equals(elementType.GetElementTarget()));
        }
    }
}

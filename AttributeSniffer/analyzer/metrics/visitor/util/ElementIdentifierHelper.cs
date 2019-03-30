using System.Collections.Generic;
using System.Linq;
using AttributeSniffer.analyzer.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor.util
{
    public class ElementIdentifierHelper
    {
        private const string attributeIdentifierFormat = "{0}#{1}";
        private const string attributeType = "Attribute";
        private const string methodType = "Method";
        private const string eventType = "Event";
        private const string propertyType = "Property";
        private const string fieldType = "Field";
        private const string parameterType = "Parameter";
        private const string returnType = "Return";

        public static ElementIdentifier getElementIdentifierForClassMetrics(SemanticModel semanticModel, IEnumerable<SyntaxNode> ancestorsAndASelfNodes)
        {
            SyntaxNode targetElement = ancestorsAndASelfNodes
                .Where(node => node.GetType() == typeof(StructDeclarationSyntax)
                    || node.GetType() == typeof(ClassDeclarationSyntax)
                    || node.GetType() == typeof(InterfaceDeclarationSyntax)
                    || node.GetType() == typeof(EnumDeclarationSyntax))
                .Last();

            ITypeSymbol targetElementSymbol = (ITypeSymbol)semanticModel.GetDeclaredSymbol(targetElement);

            string elementType = ElementIdentifierType.GetElementType(targetElement.GetType()).GetTypeIdentifier();
            string elementIdentifier = targetElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(targetElement.Span).StartLinePosition.Line + 1;

            return new ElementIdentifier(elementIdentifier, elementType, lineNumber);
        }

        public static ElementIdentifier getElementIdentifierForAttributeMetrics(SemanticModel semanticModel, AttributeSyntax attribute)
        {
            ISymbol parentElementSymbol = semanticModel.GetDeclaredSymbol(attribute.Parent.Parent);
            string parentIdentifier = parentElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            string elementIdentifier = string.Format(attributeIdentifierFormat, parentIdentifier, attribute.Name.ToString());
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(attribute.Span).StartLinePosition.Line + 1;

            return new ElementIdentifier(elementIdentifier, attributeType, lineNumber);
        }

        public static ElementIdentifier getElementIdentifierForElementMetrics(SemanticModel semanticModel, AttributeSyntax attribute)
        {
            SyntaxNode targetElement = attribute.Parent.Parent;
            ISymbol targetElementSymbol = semanticModel.GetDeclaredSymbol(targetElement);

            string elementType = ElementIdentifierType.GetElementType(targetElement.GetType()).GetTypeIdentifier();
            string elementIdentifier = targetElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(targetElement.Span).StartLinePosition.Line + 1;

            return new ElementIdentifier(elementIdentifier, elementType, lineNumber);
        }
    }
}

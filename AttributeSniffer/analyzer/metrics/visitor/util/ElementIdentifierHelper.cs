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

        public static ElementIdentifier getTargetElementForClassMetrics(SemanticModel semanticModel, IEnumerable<SyntaxNode> ancestorsAndASelfNodes)
        {
            SyntaxNode targetElement = ancestorsAndASelfNodes
                .Where(node => node.GetType() == typeof(StructDeclarationSyntax)
                    || node.GetType() == typeof(ClassDeclarationSyntax)
                    || node.GetType() == typeof(InterfaceDeclarationSyntax)
                    || node.GetType() == typeof(EnumDeclarationSyntax))
                .First();

            ITypeSymbol targetElementSymbol = (ITypeSymbol)semanticModel.GetDeclaredSymbol(targetElement);

            string elementType = targetElementSymbol.TypeKind.ToString();
            string elementIdentifier = targetElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(targetElement.Span).StartLinePosition.Line + 1;

            return new ElementIdentifier(elementIdentifier, elementType, lineNumber);
        }

        public static ElementIdentifier getTargetElementForAttributeMetrics(SemanticModel semanticModel, AttributeSyntax attribute)
        {
            ISymbol parentElementSymbol = semanticModel.GetDeclaredSymbol(attribute.Parent.Parent);
            string parentIdentifier = parentElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            string elementIdentifier = string.Format(attributeIdentifierFormat, parentIdentifier, attribute.Name.ToString());
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(attribute.Span).StartLinePosition.Line + 1;

            return new ElementIdentifier(elementIdentifier, attributeType, lineNumber);
        }

        public static ITypeSymbol getTargetElementForElementMetrics(SemanticModel semanticModel, IEnumerable<SyntaxNode> ancestorsAndASelfNodes)
        {
            SyntaxNode targetElement = ancestorsAndASelfNodes
                .Where(node => node.GetType() == typeof(MethodDeclarationSyntax)
                    || node.GetType() == typeof(PropertyDeclarationSyntax)
                    || node.GetType() == typeof(FieldDeclarationSyntax)
                    || node.GetType() == typeof(ParameterSyntax)
                    || node.GetType() == typeof(ReturnStatementSyntax))
                .First();

            return (ITypeSymbol)semanticModel.GetDeclaredSymbol(targetElement);
        }

    }
}

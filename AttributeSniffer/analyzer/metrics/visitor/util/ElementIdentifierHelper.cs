using System;
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
        private const string identifierFormat = "{0}#{1}";
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
            string parentIdentifier = "";
            int lineNumber = 0;
            List<ElementIdentifier> elementIdentifiers = new List<ElementIdentifier>();
            SyntaxNode targetElement = attribute.Parent.Parent;

            if (targetElement.GetType() == typeof(FieldDeclarationSyntax))
            {
                SeparatedSyntaxList<VariableDeclaratorSyntax> variables = ((FieldDeclarationSyntax)targetElement).Declaration.Variables;
                List<string> varNames = variables.Select(variable => variable.Identifier.Text).ToList();

                ISymbol elementSymbol = semanticModel.GetDeclaredSymbol(variables[0]);
                string identifier = elementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
                parentIdentifier = identifier.Remove(identifier.LastIndexOf(".") + 1) + String.Join(".", varNames.ToArray()); ;
            }
            else
            {
                ISymbol parentElementSymbol = semanticModel.GetDeclaredSymbol(attribute.Parent.Parent);
                parentIdentifier = parentElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            }

            string elementIdentifier = string.Format(identifierFormat, parentIdentifier, attribute.Name.ToString());
            lineNumber = semanticModel.SyntaxTree.GetLineSpan(attribute.Span).StartLinePosition.Line + 1;

            return new ElementIdentifier(elementIdentifier, attributeType, lineNumber);
        }

        public static List<ElementIdentifier> getElementIdentifiersForElementMetrics(SemanticModel semanticModel, AttributeSyntax attribute)
        {
            string elementType = "";
            string elementIdentifier = "";
            int lineNumber = 0;
            List<ElementIdentifier> elementIdentifiers = new List<ElementIdentifier>();

            SyntaxNode targetElement = attribute.Parent.Parent;

            if (targetElement.GetType() == typeof(FieldDeclarationSyntax))
            {
                SeparatedSyntaxList<VariableDeclaratorSyntax> variables = ((FieldDeclarationSyntax)targetElement).Declaration.Variables;

                variables.ForEach(variable =>
                {
                    ISymbol varElementSymbol = semanticModel.GetDeclaredSymbol(variable);
                    elementIdentifier = varElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
                    lineNumber = semanticModel.SyntaxTree.GetLineSpan(variable.Span).StartLinePosition.Line + 1;

                    elementIdentifiers.Add(new ElementIdentifier(elementIdentifier, attributeType, lineNumber));
                });
            } else
            {
                ISymbol targetElementSymbol = semanticModel.GetDeclaredSymbol(targetElement);
                string parentIdentifier = targetElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);

                if (targetElement.GetType() == typeof(ParameterSyntax))
                {
                    parentIdentifier = string
                        .Format(identifierFormat, targetElementSymbol.ContainingSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), targetElementSymbol.Name);
                }

                AttributeListSyntax attributeList = (AttributeListSyntax)attribute.Parent;
                AttributeTargetSpecifierSyntax attributeTarget = attributeList.Target;

                if (attributeTarget != null && attributeTarget.Identifier.Text == "return")
                {
                    elementType = ElementIdentifierType.RETURN_TYPE.GetTypeIdentifier();
                    elementIdentifier = string.Format(identifierFormat, parentIdentifier, elementType);
                    lineNumber = semanticModel.SyntaxTree.GetLineSpan(attribute.Span).StartLinePosition.Line + 1;
                }
                else
                {
                    elementType = ElementIdentifierType.GetElementType(targetElement.GetType()).GetTypeIdentifier();
                    elementIdentifier = parentIdentifier;
                    lineNumber = semanticModel.SyntaxTree.GetLineSpan(targetElement.Span).StartLinePosition.Line + 1;
                }

                elementIdentifiers.Add(new ElementIdentifier(elementIdentifier, elementType, lineNumber));
            }

            return elementIdentifiers;
        }
    }
}

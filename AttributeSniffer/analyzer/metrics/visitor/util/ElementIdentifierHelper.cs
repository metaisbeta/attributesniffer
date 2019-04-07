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

        public static ElementIdentifier getElementIdentifierForClassMetrics(SemanticModel semanticModel, IEnumerable<SyntaxNode> ancestorsAndASelfNodes)
        {
            SyntaxNode targetElement = ancestorsAndASelfNodes
                .Where(node => node.GetType() == typeof(StructDeclarationSyntax)
                    || node.GetType() == typeof(ClassDeclarationSyntax)
                    || node.GetType() == typeof(InterfaceDeclarationSyntax)
                    || node.GetType() == typeof(EnumDeclarationSyntax))
                .Last();

            ITypeSymbol targetElementSymbol = (ITypeSymbol)semanticModel.GetDeclaredSymbol(targetElement);

            string elementType = ElementIdentifierType.GetElementByType(targetElement.GetType()).GetTypeIdentifier();
            string elementIdentifier = targetElementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(targetElement.Span).StartLinePosition.Line + 1;

            return new ElementIdentifier(elementIdentifier, elementType, lineNumber);
        }

        public static ElementIdentifier getElementIdentifierForAttributeMetrics(SemanticModel semanticModel, AttributeSyntax node)
        {
            ElementIdentifier targetElementIdenfier = getElementIdentifierForElementMetrics(semanticModel, (AttributeListSyntax)node.Parent);
            targetElementIdenfier.ElementName += "#" + node.Name;
            targetElementIdenfier.ElementType = attributeType;
            targetElementIdenfier.LineNumber = semanticModel.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line + 1;

            return targetElementIdenfier;
        }

        public static ElementIdentifier getElementIdentifierForElementMetrics(SemanticModel semanticModel, AttributeListSyntax node)
        {
            string elementType = "";
            string elementIdentifier = "";
            int lineNumber = 0;
            SyntaxNode attributeTargetNode = null;
            List<ElementIdentifier> elementIdentifiers = new List<ElementIdentifier>();
            AttributeTargetSpecifierSyntax target = node.Target;

            if (target != null)
            {
                List<ElementIdentifierType> elementPossibleTypes = ElementIdentifierType.GetElementsByTarget(target.Identifier.Text);

                if (elementPossibleTypes.Count > 1)
                {
                    // Possible targets for type target: class, struct, interface or enum.
                    attributeTargetNode = node.Ancestors()
                        .Where(ancestor => elementPossibleTypes.Select(type => type.GetElementType()).Contains(ancestor.GetType()))
                        .First();
                    ElementIdentifierType elementIdentifierType = elementPossibleTypes
                        .Where(type => type.GetElementType() == attributeTargetNode.GetType())
                        .First();
                    elementType = elementIdentifierType.GetTypeIdentifier();
                }
                else
                {
                    ElementIdentifierType elementIdentifierType = elementPossibleTypes.First();
                    elementType = elementIdentifierType.GetTypeIdentifier();

                    if (elementIdentifierType.GetTypeIdentifier() != null)
                    {
                        if (elementIdentifierType.GetElementTarget() == "return")
                        {
                            Tuple<string, int> fieldInformation = GetIdentifierForReturnStatement(semanticModel, (MethodDeclarationSyntax)node.Parent);
                            elementIdentifier = fieldInformation.Item1;
                            lineNumber = fieldInformation.Item2;
                            elementType = elementIdentifierType.GetTypeIdentifier();
                        } else
                        {
                            attributeTargetNode = node.Ancestors().Where(ancestor => elementIdentifierType.GetElementType().Equals(ancestor.GetType())).First();
                        }
                    }
                    else
                    {
                        // Possible targets for nullable type: assembly or module.
                        MetadataReference reference = semanticModel.Compilation.References.First();
                        ISymbol assemblyOrModule = semanticModel.Compilation.GetAssemblyOrModuleSymbol(reference);

                        if (assemblyOrModule.GetType() == typeof(IAssemblySymbol))
                        {
                            elementIdentifier = ((IAssemblySymbol)assemblyOrModule).Identity.Name;
                        }
                        else
                        {
                            elementIdentifier = ((IModuleSymbol)assemblyOrModule).GetMetadata().Name;
                        }
                    }
                }
            }

            if (elementType != ElementIdentifierType.RETURN_TYPE.GetTypeIdentifier())
            {
                if (attributeTargetNode == null)
                {
                    attributeTargetNode = node.Parent;
                }

                if (attributeTargetNode.GetType() == typeof(FieldDeclarationSyntax))
                {
                    Tuple<string, int> fieldInformation = GetIdentifierForFieldDeclaration(semanticModel, (FieldDeclarationSyntax)attributeTargetNode);
                    elementIdentifier = fieldInformation.Item1;
                    lineNumber = fieldInformation.Item2;
                }
                else
                {
                    ISymbol targetSymbol = semanticModel.GetDeclaredSymbol(attributeTargetNode);

                    if (attributeTargetNode.GetType() == typeof(ParameterSyntax))
                    {
                        elementIdentifier = GetIdentifierForParameterSyntax((IParameterSymbol)targetSymbol);
                    }
                    else
                    {
                        elementIdentifier = targetSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
                    }
                    lineNumber = semanticModel.SyntaxTree.GetLineSpan(attributeTargetNode.Span).StartLinePosition.Line + 1;
                }
            }

            return new ElementIdentifier(elementIdentifier, elementType, lineNumber);
        }

        private static string GetIdentifierForParameterSyntax(IParameterSymbol parameterSymbol)
        {
            return string.Format(identifierFormat, parameterSymbol.ContainingSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), parameterSymbol.Name);
        }

        private static Tuple<string, int> GetIdentifierForReturnStatement(SemanticModel semanticModel, MethodDeclarationSyntax returnParent)
        {
            IMethodSymbol methodSymbol = semanticModel.GetDeclaredSymbol(returnParent);
            string parentIdentifier = methodSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            string elementIdentifier = string.Format(identifierFormat, parentIdentifier, ElementIdentifierType.RETURN_TYPE.GetTypeIdentifier());
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(returnParent.Span).StartLinePosition.Line + 1;

            return Tuple.Create(elementIdentifier, lineNumber);
        }

        private static Tuple<string, int> GetIdentifierForFieldDeclaration(SemanticModel semanticModel, FieldDeclarationSyntax fieldNode)
        {
            SeparatedSyntaxList<VariableDeclaratorSyntax> variables = fieldNode.Declaration.Variables;
            List<string> varNames = variables.Select(variable => variable.Identifier.Text).ToList();

            ISymbol elementSymbol = semanticModel.GetDeclaredSymbol(variables[0]);
            string identifier = elementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            string elementIdentifier = identifier.Remove(identifier.LastIndexOf(".") + 1) + String.Join(".", varNames.ToArray());
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(variables.First().Span).StartLinePosition.Line + 1;

            return Tuple.Create(elementIdentifier, lineNumber);
        }

    }
}

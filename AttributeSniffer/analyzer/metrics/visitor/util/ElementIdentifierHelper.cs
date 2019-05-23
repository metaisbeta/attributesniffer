using System;
using System.Collections.Generic;
using System.Linq;
using AttributeSniffer.analyzer.model;
using AttributeSniffer.analyzer.model.exception;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor.util
{
    public class ElementIdentifierHelper
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private const string identifierFormat = "{0}#{1}";
        private const string attributeType = "Attribute";

        public static ElementIdentifier getElementIdentifierForClassMetrics(string filePath, SemanticModel semanticModel, AttributeSyntax attribute)
        {
            string elementType = ElementIdentifierType.CS_FILE_TYPE.GetTypeIdentifier();
            return new ElementIdentifier("", elementType, 0, filePath);
        }

        public static List<ElementIdentifier> getElementIdentifiersForAttributeMetrics(string filePath, SemanticModel semanticModel, AttributeSyntax node)
        {
            List<ElementIdentifier> targetElementIdentifiers = getElementIdentifiersForElementMetrics(filePath, semanticModel, (AttributeListSyntax)node.Parent);
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line + 1;
            targetElementIdentifiers.ForEach(identifier =>
            {
                identifier.ElementName += "#" + node.Name;
                identifier.ElementType = attributeType;
                identifier.LineNumber = lineNumber;
            });

            return targetElementIdentifiers;
        }

        public static List<ElementIdentifier> getElementIdentifiersForElementMetrics(String filePath, SemanticModel semanticModel, AttributeListSyntax node)
        {
            List<ElementIdentifier> elementIdentifiers = new List<ElementIdentifier>();
            string elementType = "";
            string elementIdentifier = "";
            int lineNumber = 0;
            SyntaxNode attributeTargetNode = null;
            AttributeTargetSpecifierSyntax target = node.Target;

            try {
                if (target != null)
                {
                    List<ElementIdentifierType> elementPossibleTypes = ElementIdentifierType.GetElementsByTarget(target.Identifier.Text);

                    if (elementPossibleTypes.Count > 1)
                    {
                        // Possible targets for type target: class, struct, interface, enum or delegate.
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

                        if (elementIdentifierType.GetElementTarget() == ElementIdentifierType.RETURN_TYPE.GetElementTarget())
                        {
                            Tuple<string, int> fieldInformation = GetIdentifierForReturnStatement(semanticModel, node.Parent);
                            elementIdentifier = fieldInformation.Item1;
                            lineNumber = fieldInformation.Item2;
                            elementIdentifiers.Add(new ElementIdentifier(elementIdentifier, elementType, lineNumber, filePath));
                        }
                        else if (elementIdentifierType.GetElementTarget() == ElementIdentifierType.ASSEMBLY_TYPE.GetElementTarget())
                        {
                            elementIdentifier = semanticModel.Compilation.AssemblyName;
                            elementIdentifiers.Add(new ElementIdentifier(elementIdentifier, elementType, lineNumber, filePath));
                        }
                        else if (elementIdentifierType.GetElementTarget() == ElementIdentifierType.MODULE_TYPE.GetElementTarget())
                        {
                            elementIdentifier = semanticModel.Compilation.SourceModule.Name;
                            elementIdentifiers.Add(new ElementIdentifier(elementIdentifier, elementType, lineNumber, filePath));
                        }
                        else
                        {
                            var typeAncestors = node.Ancestors().Where(ancestor => elementIdentifierType.GetElementType().Equals(ancestor.GetType())).ToList();

                            if (typeAncestors.IsEmpty())
                            {
                                attributeTargetNode = node.Parent;
                            }
                            else
                            {
                                attributeTargetNode = node.Ancestors().Where(ancestor => elementIdentifierType.GetElementType().Equals(ancestor.GetType())).First();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Error while extracting target at file {0}", filePath);
                throw new FatalElementIdentifierException("Error extracting target of attribute.");
            }

            if (elementType != ElementIdentifierType.RETURN_TYPE.GetTypeIdentifier()
                && elementType != ElementIdentifierType.ASSEMBLY_TYPE.GetTypeIdentifier()
                && elementType != ElementIdentifierType.MODULE_TYPE.GetTypeIdentifier())
            {
                if (attributeTargetNode == null)
                {
                    attributeTargetNode = node.Parent;
                }

                if (attributeTargetNode.GetType() == typeof(IncompleteMemberSyntax)
                    || attributeTargetNode.GetType() == typeof(AccessorDeclarationSyntax))
                {
                    logger.Error("File {0} contains an {1} at its struct.", filePath, attributeTargetNode.GetType().ToString());
                    throw new IgnoreElementIdentifierException("Syntax error");
                }

                if (attributeTargetNode.GetType() == typeof(FieldDeclarationSyntax) 
                    || attributeTargetNode.GetType() == typeof(EventFieldDeclarationSyntax))
                {
                    elementType = ElementIdentifierType.GetElementByType(attributeTargetNode.GetType()).GetTypeIdentifier();
                    Dictionary<string, int> fieldInformations = GetIdentifierForFieldDeclaration(semanticModel, (BaseFieldDeclarationSyntax)attributeTargetNode);

                    fieldInformations.ForEach(info => elementIdentifiers.Add(new ElementIdentifier(info.Key, elementType, info.Value, filePath)));                   
                }
                else
                {
                    ISymbol targetSymbol = semanticModel.GetDeclaredSymbol(attributeTargetNode);

                    if (targetSymbol == null)
                    {
                        logger.Fatal("Error getting symbol in file {0} for {1} type", filePath, attributeTargetNode.GetType().ToString());
                        throw new FatalElementIdentifierException("Error getting semantic model symbol.");
                    }

                    if (attributeTargetNode.GetType() == typeof(ParameterSyntax))
                    {
                        elementIdentifier = GetIdentifierForParameterSyntax((IParameterSymbol)targetSymbol);
                    }
                    else
                    {
                        elementIdentifier = targetSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
                    }
                    elementType = ElementIdentifierType.GetElementByType(attributeTargetNode.GetType()).GetTypeIdentifier();
                    lineNumber = semanticModel.SyntaxTree.GetLineSpan(attributeTargetNode.Span).StartLinePosition.Line + 1;
                    elementIdentifiers.Add(new ElementIdentifier(elementIdentifier, elementType, lineNumber, filePath));
                }
            }

            return elementIdentifiers;
        }

        private static string GetIdentifierForParameterSyntax(IParameterSymbol parameterSymbol)
        {
            return string.Format(identifierFormat, parameterSymbol.ContainingSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), parameterSymbol.Name);
        }

        private static Tuple<string, int> GetIdentifierForReturnStatement(SemanticModel semanticModel, SyntaxNode returnParent)
        {

            ISymbol symbol = semanticModel.GetDeclaredSymbol(returnParent);
            string parentIdentifier = symbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
            string elementIdentifier = string.Format(identifierFormat, parentIdentifier, ElementIdentifierType.RETURN_TYPE.GetTypeIdentifier());
            int lineNumber = semanticModel.SyntaxTree.GetLineSpan(returnParent.Span).StartLinePosition.Line + 1;

            return Tuple.Create(elementIdentifier, lineNumber);
        }

        private static Dictionary<string, int> GetIdentifierForFieldDeclaration(SemanticModel semanticModel, BaseFieldDeclarationSyntax fieldNode)
        {
            Dictionary<string, int> fieldIdentifiers = new Dictionary<string, int>();
            SeparatedSyntaxList<VariableDeclaratorSyntax> variables = fieldNode.Declaration.Variables;

            foreach(VariableDeclaratorSyntax variable in variables)
            {
                ISymbol elementSymbol = semanticModel.GetDeclaredSymbol(variable);
                string identifier = elementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
                string elementIdentifier = identifier.Remove(identifier.LastIndexOf(".") + 1) + variable.Identifier.Text;
                int lineNumber = semanticModel.SyntaxTree.GetLineSpan(variables.First().Span).StartLinePosition.Line + 1;
                fieldIdentifiers.Add(identifier, lineNumber);
            }

            return fieldIdentifiers;
        }

    }
}

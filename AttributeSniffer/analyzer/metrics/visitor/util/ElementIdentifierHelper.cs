using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public static int getElementIdentifiersNumberWithMetadataInClass(SyntaxNode node)
        {
            int elementsCount = 0;

            foreach (var item in node.DescendantNodes().Where(x => x is AttributeSyntax).ToList())
            {
                if (item.Ancestors().Any(x=> x is ConstructorDeclarationSyntax || x is MethodDeclarationSyntax || x is VariableDeclarationSyntax || 
                x is PropertyDeclarationSyntax || x is DelegateDeclarationSyntax || x is EventDeclarationSyntax || x is FieldDeclarationSyntax ||
                x is InterfaceDeclarationSyntax || x is ReturnStatementSyntax || x is StructDeclarationSyntax ||
                x is ClassDeclarationSyntax || x is EnumDeclarationSyntax || x is ParameterSyntax))
                {
                    elementsCount++;
                }
            }

            return elementsCount;
        }

        public static int getElementIdentifiersNumberInClass(SyntaxNode node)
        {
            int elementsCount = 0;

            elementsCount += node.DescendantNodes().Where(x => x is MethodDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is VariableDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is PropertyDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is ClassDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is EnumDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is ParameterSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is AttributeSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is ConstructorDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is NamespaceDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is ReturnStatementSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is FieldDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is EventDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is DelegateDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is InterfaceDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is StructDeclarationSyntax).ToList().Count;
            elementsCount += node.DescendantNodes().Where(x => x is TypeParameterSyntax).ToList().Count;

            return elementsCount;
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

            try
            {
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

        public static List<ElementIdentifier> getElementIdentifiersForNamespaceMetrics(string filePath, SemanticModel semanticModel, List<SyntaxNode> listNode, AttributeSyntax attribute,
           Dictionary<string, (string, int)> NamespacesSaved)
        {
            List<ElementIdentifier> targetElementIdentifiers = new List<ElementIdentifier>();

            try
            {
                string attributeDefinitionClass = string.Empty;
                var typeCompilation = semanticModel.GetTypeInfo(attribute);
                var attributeName = typeCompilation.Type?.MetadataName;
                int lineNumber = 0;

                if (NamespacesSaved.ContainsKey(attributeName)) (attributeDefinitionClass, lineNumber) = NamespacesSaved[attributeName];
                else
                {
                    var compilationOriginalDefinition = typeCompilation.Type.OriginalDefinition;
                    attributeDefinitionClass = compilationOriginalDefinition.ToString().Replace("." + attributeName, "");

                    var namespaceDefinition = string.Empty;
                    UsingDirectiveSyntax specifiedNode = null;

                    foreach (var node in listNode)
                    {
                        string pat = ((UsingDirectiveSyntax)node).Name.ToString();

                        Match possibleMatch = null;
                        if (!attributeDefinitionClass.ToString().Contains("."))
                            possibleMatch = Regex.Match(attributeName.ToString(), pat);
                        else possibleMatch = Regex.Match(attributeDefinitionClass, pat);

                        if ((possibleMatch.Success) && possibleMatch?.Value?.Split(".").Count() >= namespaceDefinition.Split(".").Count())
                        {
                            namespaceDefinition = possibleMatch.Value;
                            specifiedNode = (UsingDirectiveSyntax)listNode.FirstOrDefault(x => ((UsingDirectiveSyntax)x).Name.ToString() == namespaceDefinition);
                        }
                    }

                    if (specifiedNode is not null) lineNumber = semanticModel.SyntaxTree.GetLineSpan(specifiedNode.Span).StartLinePosition.Line + 1;

                    if (compilationOriginalDefinition.Kind != SymbolKind.ErrorType || attributeDefinitionClass.Contains("."))
                        NamespacesSaved.Add(attributeName, (attributeDefinitionClass, lineNumber));
                }

                targetElementIdentifiers.Add(new ElementIdentifier
                {
                    FileDeclarationPath = filePath,
                    ElementName = "#" + attributeDefinitionClass + "." + attributeName,
                    ElementType = ElementIdentifierType.NAMESPACE_TYPE.GetTypeIdentifier()
                });

                targetElementIdentifiers.ForEach(identifier =>
                {
                    identifier.LineNumber = lineNumber;
                });

            }
            catch (Exception e)
            {
                logger.Info("Ignoring attribute's namespace due to semantic model compilation error {0}.", attribute.Name.ToString());
            }

            return targetElementIdentifiers;
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

            foreach (VariableDeclaratorSyntax variable in variables)
            {
                ISymbol elementSymbol = semanticModel.GetDeclaredSymbol(variable);
                string identifier = elementSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
                int lineNumber = semanticModel.SyntaxTree.GetLineSpan(variables.First().Span).StartLinePosition.Line + 1;
                fieldIdentifiers.Add(identifier, lineNumber);
            }

            return fieldIdentifiers;
        }

    }
}

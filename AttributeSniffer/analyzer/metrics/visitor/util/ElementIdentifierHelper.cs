using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics.visitor.util
{
    public class ElementIdentifierHelper
    {
        public static ITypeSymbol getTargetElementForClassMetrics(SemanticModel semanticModel, IEnumerable<SyntaxNode> ancestorsAndASelfNodes)
        {
            SyntaxNode targetElement = ancestorsAndASelfNodes
                .Where(node => node.GetType() == typeof(StructDeclarationSyntax)
                    || node.GetType() == typeof(ClassDeclarationSyntax)
                    || node.GetType() == typeof(InterfaceDeclarationSyntax))
                .First();

            return (ITypeSymbol)semanticModel.GetDeclaredSymbol(targetElement);
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

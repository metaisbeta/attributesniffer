using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

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
    }
}

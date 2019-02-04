using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeSniffer.analyzer.metrics {
   
   public class ClassInfo : CSharpSyntaxWalker {

        public string FullClassName { get; private set; }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
            NamespaceDeclarationSyntax nameSpaceNode = (NamespaceDeclarationSyntax)node.Parent;
            FullClassName = string.Concat(nameSpaceNode.Name, node.Identifier);
        }

    }
}

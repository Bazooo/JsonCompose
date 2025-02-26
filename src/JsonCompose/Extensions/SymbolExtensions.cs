using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JsonCompose.Extensions;

public static class SymbolExtensions
{
    public static IEnumerable<IPropertySymbol> GetProperties(this INamedTypeSymbol classSymbol) => classSymbol.GetMembers().OfType<IPropertySymbol>().Concat(classSymbol.BaseType?.GetProperties() ?? []);
    
    public static IEnumerable<UsingDirectiveSyntax> GetUsings(this INamedTypeSymbol classSymbol) => classSymbol.DeclaringSyntaxReferences
        .SelectMany(sr => sr.SyntaxTree.GetRoot().ChildNodes().OfType<UsingDirectiveSyntax>())
        .Concat([SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(' ' + classSymbol.ContainingNamespace.ToDisplayString()))])
        .Concat(classSymbol.BaseType?.GetUsings() ?? []);
}
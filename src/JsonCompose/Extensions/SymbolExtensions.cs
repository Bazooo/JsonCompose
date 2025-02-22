using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JsonCompose.Extensions;

public static class SymbolExtensions
{
    public static TypeSyntax ToTypeSyntax(this ITypeSymbol symbol) => SyntaxFactory.ParseTypeName(symbol.ToDisplayString());
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace JsonCompose.Extensions;

public static class SymbolExtensions
{
    public static IEnumerable<IPropertySymbol> GetProperties(this INamedTypeSymbol classSymbol) => classSymbol.GetMembers().OfType<IPropertySymbol>().Concat(classSymbol.BaseType?.GetProperties() ?? []);
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonCompose;

public static class UtilityExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        where T : class
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return source.Where(item => item is not null)!;
    } 
}
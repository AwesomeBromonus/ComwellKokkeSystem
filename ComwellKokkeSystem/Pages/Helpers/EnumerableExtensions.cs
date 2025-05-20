using System;
using System.Collections.Generic;
using System.Linq;

// This namespace should match your project structure
namespace ComwellKokkeSystem.Pages.Helpers

{
    /// Extension methods for IEnumerable<T>
    public static class EnumerableExtensions
    {
        /// Dynamically sorts a collection by a property name.
        /// Usage: list.OrderByDynamic("Navn", true)
        public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, string propertyName, bool ascending)
        {
            var prop = typeof(T).GetProperty(propertyName);
            if (prop == null) return source;
            return ascending
                ? source.OrderBy(x => prop.GetValue(x, null))
                : source.OrderByDescending(x => prop.GetValue(x, null));
        }
    }
}
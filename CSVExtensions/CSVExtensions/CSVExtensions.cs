using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MBW
{
    public static class CSVExtensions
    {
        public static IEnumerable<string> ToCsv<T>(this IEnumerable<T> items, bool withHeader = true)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            if (withHeader) yield return string.Join(",", properties.Select(p => p.Name)) + Environment.NewLine;

            foreach (var item in items)
            {
                yield return item.ToCsvRow(properties) + Environment.NewLine;
            }
        }

        public static string ToCsvRow<T>(this T item, PropertyInfo[] properties = null)
        {
            properties = properties ?? typeof(T).GetProperties();

            return string.Join(",", properties.Select(p => (p.GetValue(item) ?? "").ToString().EscapeForCsv()));
        }

        public static string EscapeForCsv(this string value)
        {
            if (value.Contains(',') || value.Contains('"') || value.Contains('\r') || value.Contains('\n'))
            {
                return '"' + value.Replace("\"", "\"\"") + '"';
            }

            return value;
        }
    }
}

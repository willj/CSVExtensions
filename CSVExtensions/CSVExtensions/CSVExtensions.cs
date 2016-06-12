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

            if (properties.Count() == 0)    // if there are no properties, let's see if it's dynamic/ExpandoObject
            {
                foreach (var line in ToCsvFromDynamic(items, withHeader))
                {
                    yield return line;
                }
                yield break;
            }

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

        private static IEnumerable<string> ToCsvFromDynamic<T>(IEnumerable<T> items, bool withHeader)
        {
            IDictionary<string, object> properties = items.FirstOrDefault() as IDictionary<string, object>;

            if (properties == null) yield break;

            if (withHeader) yield return string.Join(",", properties.Keys) + Environment.NewLine;

            foreach (var item in items)
            {
                IDictionary<string, object> dict = item as IDictionary<string, object>;

                yield return string.Join(",", dict.Select(d => (d.Value ?? "").ToString().EscapeForCsv())) + Environment.NewLine;
            }
        }

    }
}

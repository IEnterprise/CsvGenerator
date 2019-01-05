namespace CsvGenerator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;

    public static class Generator
    {
        private static char[] quotedCharacters = { ',', '"', '\n' };
        private const string quote = "\"";
        private const string escapedQuote = "\"\"";

        public static string ToCsv(this IEnumerable enumerable, char seperator)
        {
            Type type = enumerable.GetItemType();
            bool isRefType = type.IsClass | type.IsInterface;

            string result = "";
            using (StringWriter sw = new StringWriter())
            {
                if (isRefType && type != typeof(string))
                {
                    var properties = type.GetProperties()
                                         .Where(prop => prop.PropertyType.IsValueType
                                                || prop.PropertyType == typeof(string));

                    string header = properties
                        .Select(n => n.Name)
                        .Aggregate((a, b) => a + seperator + b);

                    sw.WriteLine(header);

                    foreach (var item in enumerable)
                    {
                        string row = properties
                            .Select(n => n.GetValue(item, null))
                            .Select(n => n == null ? "null" : QuoteCheck(n.ToString()))
                            .Aggregate((a, b) => a + seperator + b);

                        sw.WriteLine(row);
                    }
                }
                else
                {
                    string header = "Values";
                    sw.WriteLine(header);

                    foreach (var item in enumerable)
                    {
                        sw.WriteLine(QuoteCheck(item.ToString()));
                    }

                }

                result = sw.ToString().TrimEnd('\r', '\n');
            }
            return result;
        }

        private static Type GetItemType(this IEnumerable enumerable)
        {
            Contract.Ensures(Contract.Result<Type>() != null);
            var itemType =
                enumerable.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GenericTypeArguments.Length == 1)
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (itemType != null) return itemType.GenericTypeArguments[0];
            if (!enumerable.GetEnumerator().MoveNext()) return null;

            return enumerable.GetEnumerator().Current.GetType();
        }

        private static string QuoteCheck(string value)
        {
            if (value == null) return string.Empty;
            if (value.Contains(quote)) value = value.Replace(quote, escapedQuote);
            if (value.IndexOfAny(quotedCharacters) > 1) value = quote + value + quote;
            return value;
        }
    }
}

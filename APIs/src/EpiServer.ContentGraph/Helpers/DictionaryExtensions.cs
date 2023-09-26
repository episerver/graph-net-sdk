using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPiServer.ContentGraph.Helpers
{
    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> dictionary)
        {
            source.ValidateNotNullArgument("source");
            dictionary.ValidateNotNullArgument("dictionary");

            foreach (var item in dictionary)
            {
                source.Add(item.Key, item.Value);
            }
        }

        public static IDictionary<TKey, TValue> AppendRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> dictionary)
        {
            source.ValidateNotNullArgument("source");
            dictionary.ValidateNotNullArgument("dictionary");

            foreach (var item in dictionary)
            {
                source.Add(item.Key, item.Value);
            }

            return source;
        }
    }
}

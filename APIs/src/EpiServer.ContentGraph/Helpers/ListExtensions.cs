using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;
using System.Diagnostics;

namespace EPiServer.ContentGraph.Helpers
{
    public static class ListExtensions
    {
        public static string[] ToStringArray<T>(this IEnumerable<T> value)
        {
            return (from v in value select v.ToString()).ToArray();
        }

        public static void ForEach<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (T v in value)
            {
                action(v);
            }
        }

        public static void ValidateNotNullOrEmptyArgument(this IEnumerable argument, string paramName)
        {
            argument.ValidateNotNullArgument(paramName);

            if (!argument.GetEnumerator().MoveNext())
            {
                StackTrace stackTrace = new StackTrace();
                var method = stackTrace.GetFrame(1).GetMethod();
                var message =
                    string.Format(
                        "The method {0} in class {1} has been invoked with an empty IEnumerable as value for the argument {2}.",
                        method.Name,
                        method.DeclaringType,
                        paramName);
                throw new ArgumentException(paramName, message);
            }
        }

        public static IEnumerable<TSource> DistinctByImpl<TSource, TKey>
        (this IEnumerable<TSource> source,
         Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector,
                                     EqualityComparer<TKey>.Default);
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source,
         Func<TSource, TKey> keySelector)
        {
            return source.DistinctByImpl(keySelector);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source,
             Func<TSource, TKey> keySelector,
             IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            return DistinctByImpl(source, keySelector, comparer);
        }

        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>
            (IEnumerable<TSource> source,
             Func<TSource, TKey> keySelector,
             IEqualityComparer<TKey> comparer)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}

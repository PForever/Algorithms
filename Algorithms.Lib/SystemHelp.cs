using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Lib
{
    public static class SystemHelp
    {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            if (dictionary.ContainsKey(key)) return dictionary[key];
            var value = factory(key);
            dictionary.Add(key, value);
            return value;
        }
        public static IEnumerable<TValue> GetListOrEmpty<TKey, TValue>(this IDictionary<TKey, IList<TValue>> edges, TKey node)
            => edges.ContainsKey(node) ? edges[node] : Enumerable.Empty<TValue>();
        public static IEnumerable<T> In<T>(this IEnumerable<T> src, HashSet<T> hashSet) => src.Where(e => hashSet.Contains(e));
        public static IEnumerable<T> NotIn<T>(this IEnumerable<T> src, HashSet<T> hashSet) => src.Where(e => !hashSet.Contains(e));
        public static bool NotIn<T>(this T e, HashSet<T> hashSet) => !hashSet.Contains(e);
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> src) where T : class => src.Where(s => s is not null);
    }
    public class EventArgs<T> : EventArgs
    {
        public T Value { get; }
        public EventArgs(T value)
        {
            Value = value;
        }
    }
}
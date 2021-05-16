using System;
using System.Collections.Concurrent;
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
        public static T MinElement<T>(this IEnumerable<T> src, Func<T, int> func)
        {
            var min = (Item: default(T), Value: int.MaxValue);
            foreach (var item in src)
            {
                var value = func(item);
                if (min.Value <= value) continue;
                min = (item, value);
            }
            return min.Item;
        }
        public static void ForEach<T>(this IEnumerable<T> src, Action<T> action)
        {
            if (src is null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            foreach (var i in src) action(i);
        }
        public static IEnumerable<T> Transform<T>(this IEnumerable<T> src, Func<T, T> transform)
        {
            if (src is null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            if (transform is null)
            {
                throw new ArgumentNullException(nameof(transform));
            }
            static IEnumerable<T> Transform(IEnumerable<T> src, Func<T, T> transform)
            {
                foreach (var i in src) yield return transform(i);
            }
            return Transform(src, transform);
        }
    }
    public class EventArgs<T> : EventArgs
    {
        public T Value { get; }
        public EventArgs(T value)
        {
            Value = value;
        }
    }
    public class Pool<T>
    {
        public Pool(Func<T> factory, int capacity = 8)
        {
            _queue = new();
            for (int i = 0; i < capacity; i++)
                _queue.Enqueue(factory());
        }
        private readonly ConcurrentQueue<T> _queue;

        public int Capacity { get; private set; }

        struct PoolObject : IDisposable
        {
            public T Value { get; }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
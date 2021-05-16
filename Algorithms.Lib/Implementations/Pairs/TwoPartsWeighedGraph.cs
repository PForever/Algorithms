using Algorithms.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Lib.Implementations.Pairs
{
    class TwoPartsWeighedGraph : ITwoPartsWeighedGraph
    {
        public IEnumerable<IPairNode> NodeXs { get => _nodeXs.Keys; }
        public IEnumerable<IPairNode> NodeYs { get => _nodeYs.Keys; }
        public IEnumerable<IPairNode> Nodes { get => _nodeXs.Keys.Concat(_nodeYs.Keys); }
        private readonly IDictionary<IPairNode, IList<IWeighedEdge>> _nodeXs;
        private readonly IDictionary<IPairNode, IList<IWeighedEdge>> _nodeYs;
        private readonly Dictionary<EdgeKey, IWeighedEdge> _edges = new();

        public TwoPartsWeighedGraph(IEnumerable<IPairNode> nodeXs, IEnumerable<IPairNode> nodeYs)
        {
            _nodeXs = nodeXs.ToDictionary(k => k, _ => (IList<IWeighedEdge>)new List<IWeighedEdge>());
            _nodeYs = nodeYs.ToDictionary(k => k, _ => (IList<IWeighedEdge>)new List<IWeighedEdge>());
        }

        public void AddEdge(IWeighedEdge edge)
        {
            _edges.Add(new EdgeKey(edge.Node1, edge.Node2), edge);
            GetEdgesListOrThrow(edge.Node1).Add(edge);
            GetEdgesListOrThrow(edge.Node2).Add(edge);
        }

        public IWeighedEdge GetEdge(IPairNode nodeX, IPairNode nodeY) => _edges[new EdgeKey(nodeX, nodeY)];

        public IEnumerable<IWeighedEdge> GetEdges(IPairNode node) => GetEdgesListOrThrow(node).AsEnumerable();
        private IList<IWeighedEdge> GetEdgesListOrThrow(IPairNode node) => _nodeXs.TryGetValue(node, out var value) || _nodeYs.TryGetValue(node, out value) ? value : throw new ArgumentOutOfRangeException("Graph doesn't contains this node");

        public string Print() => $"<X = {string.Join(", ", NodeXs.Select(n => n.Print()))}, Y = {string.Join(", ", NodeYs.Select(n => n.Print()))}, E = {string.Join(", ", _edges.Values.Distinct().Select(n => n.Print()))}>";
        public override string ToString() => Print();
        struct EdgeKey : IEquatable<EdgeKey>
        {
            private readonly IPairNode Node1;
            private readonly IPairNode Node2;

            public EdgeKey(IPairNode node1, IPairNode node2)
            {
                Node1 = node1 ?? throw new ArgumentNullException(nameof(node1));
                Node2 = node2 ?? throw new ArgumentNullException(nameof(node2));
            }

            public override bool Equals(object obj)
            {
                return obj is EdgeKey key && Equals(key);
            }

            public bool Equals(EdgeKey other) =>
                       EqualityComparer<IPairNode>.Default.Equals(Node1, other.Node1) &&
                       EqualityComparer<IPairNode>.Default.Equals(Node2, other.Node2)
                       ||
                       EqualityComparer<IPairNode>.Default.Equals(Node1, other.Node2) &&
                       EqualityComparer<IPairNode>.Default.Equals(Node2, other.Node1);

            public override int GetHashCode() => HashCode.Combine(Node1.GetHashCode() ^ Node2.GetHashCode());
        }
    }
}
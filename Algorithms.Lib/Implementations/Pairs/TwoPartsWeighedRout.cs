using Algorithms.Lib.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Lib.Implementations.Pairs
{
    class TwoPartsWeighedRout : ITwoPartsWeighedRout
    {
        public TwoPartsWeighedRout(IPairNode start)
        {
            _nodes = new() { start };
        }
        private readonly HashSet<IPairNode> _nodes;
        private readonly List<IWeighedEdge> _edges = new();
        public IEnumerable<IPairNode> NodeXs => _nodes.Where(n => n.IsX);
        public IEnumerable<IPairNode> NodeYs => _nodes.Where(n => !n.IsX);

        public int NodeCount { get; }

        public IEnumerable<IWeighedEdge> Edges => _edges.AsEnumerable();

        public void AddEdge(IWeighedEdge edge)
        {
            _edges.Add(edge);
            _nodes.Add(edge.Node1);
            _nodes.Add(edge.Node2);

        }

        public string Print() => $"{{{string.Join(", ", _nodes.Select(n => n.Print()))}}}";
        public override string ToString() => Print();
    }
}
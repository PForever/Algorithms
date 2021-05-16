using Algorithms.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Lib.Implementations.Pairs
{
    class WeighedPairsGraph : IWeighedPairsGraph
    {
        public WeighedPairsGraph(IEnumerable<IPairNode> nodeXs, IEnumerable<IPairNode> nodeYs)
        {
            _freeNodes = new HashSet<IPairNode>(nodeXs.Concat(nodeYs));

        }
        public IEnumerable<IWeighedEdge> Edges => _edges.AsEnumerable();
        private readonly HashSet<IPairNode> _freeNodes;
        private readonly HashSet<IWeighedEdge> _edges = new();
        public void AddEdge(IWeighedEdge edge)
        {
            _edges.Add(edge);
            _freeNodes.Remove(edge.Node1);
            _freeNodes.Remove(edge.Node2);
        }

        public IEnumerable<IPairNode> GetFree() => _freeNodes.AsEnumerable();

        public bool IsFree(INode last) => _freeNodes.Contains(last);

        public string Print() => $"<{string.Join(", ", _edges.Select(p => p.Print()))}>";
        public override string ToString() => Print();
        public void RemoveEdge(IWeighedEdge edge)
        {
            _edges.Remove(edge);
            _freeNodes.Add(edge.Node1);
            _freeNodes.Add(edge.Node2);
        }
    }
}
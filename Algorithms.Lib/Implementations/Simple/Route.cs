using Algorithms.Lib.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Algorithms.Lib.Implementations.Simple
{
    class Route : IRoute
    {
        private readonly List<IEdge> _edges = new();
        public IReadOnlyCollection<IEdge> Edges => _edges;

        public void Append(IEdge edge)
        {
            _edges.Add(edge);
        }

        public string Print()
        {
            if (_edges.Count == 0) return "Ø";
            if (_edges.Count == 1) return _edges[0].Print();
            var first = _edges[0];
            var second = _edges[1];
            var node = second.Contains(first.Node1) ? first.Node2 : first.Node1;
            var sb = new StringBuilder(_edges.Count + 1);
            for (int i = 0; i < _edges.Count; i++)
            {
                _ = sb.Append($"{node}, ");
                node = node.GetNext(_edges[i]);
            }
            _ = sb.Append($"{node}");
            return $"{{{sb}}}";
        }
        public override string ToString() => Print();
    }
}
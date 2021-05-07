using Algorithms.Lib.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.Lib.Implementations.Simple
{
    class Path : IPath
    {
        private readonly List<IArcs> _edges = new();
        public IReadOnlyCollection<IArcs> Arcs => _edges;

        public void Append(IArcs edge)
        {
            //_edges.Add(edge);
            _edges.Insert(0, edge);
        }

        public string Print()
        {
            if (_edges.Count == 0) return "Ø";
            if (_edges.Count == 1) return _edges[0].Print();
            var first = _edges[0];
            var second = _edges[1];
            var node = second.From == first.To ? first.From : first.To;
            var sb = new StringBuilder(_edges.Count + 1);
            for (int i = 0; i < _edges.Count; i++)
            {
                _ = sb.Append($"{node}, ");
                node = _edges[i].To;
            }
            _ = sb.Append($"{node}");
            return $"<{sb}>";
        }
        public override string ToString() => Print();
    }
}
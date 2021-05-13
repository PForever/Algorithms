using Algorithms.Lib.GraphsAlghorithms;
using Algorithms.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Lib.Implementations.Simple
{
    class SimpleOrgraph : IOrgraph
    {

        public SimpleOrgraph(IEnumerable<INode> nodes, IEnumerable<IArcs> edges)
        {
            _nodes = new(nodes ?? throw new ArgumentNullException(nameof(nodes)));
            _arcs = new(edges ?? throw new ArgumentNullException(nameof(edges)));
        }
        private readonly HashSet<INode> _nodes = new();
        private readonly HashSet<IArcs> _arcs = new();
        public IReadOnlyCollection<INode> Nodes => _nodes;

        public IReadOnlyCollection<IArcs> Arcs => _arcs;

        public string Print() => $"<V = {{{string.Join(", ", _nodes.Select(n => n.Print()))}}}, E = {{{string.Join(", ", _arcs.Select(n => n.Print()))}}}>";

        public void RemoveNode(INode node)
        {
            _ = _arcs.RemoveWhere(e => e.Contains(node));
            _ = _nodes.Remove(node);
        }
        public override string ToString() => Print();
        public void RemoveArc(IArcs edge) => _arcs.Remove(edge);
    }
}
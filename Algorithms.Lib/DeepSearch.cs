using Algorithms.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Lib
{
    public class DeepSearch
    {
        private readonly HashSet<INode> _passed = new();
        public IEnumerable<INode> Search(IGraph graph, INode root) => Search(graph.Edges, graph.Nodes, root);
        public IEnumerable<INode> Search(IOrgraph graph, INode root) => Search(graph.Arcs, graph.Nodes, root);

        public IEnumerable<INode> Search(IEnumerable<IEdge> edges, INode root)
        {
            var nodes = edges.GetNodes();
            return Search(edges, nodes, root);
        }
        public IEnumerable<INode> Search(IEnumerable<IArcs> edges, INode root)
        {
            var nodes = edges.GetNodes();
            return Search(edges, nodes, root);
        }
        private IEnumerable<INode> Search(IEnumerable<IEdge> edges, IEnumerable<INode> nodes, INode root)
        {
            if (_passed.Count != 0) throw new Exception("Enumerator wasn't disposed");
            var nodesEdges = nodes.ToDictionary(n => n, n => (IList<IEdge>)edges.Where(e => e.Contains(n)).ToList());
            foreach (var node in InternalSearch(nodesEdges, root)) yield return node;
            _passed.Clear();
        }
        private IEnumerable<INode> Search(IEnumerable<IArcs> edges, IEnumerable<INode> nodes, INode root)
        {
            if (_passed.Count != 0) throw new Exception("Enumerator wasn't disposed");
            var nodesEdges = nodes.ToDictionary(n => n, n => (IList<IEdge>)edges.Where(e => e.From == n).ToList());
            foreach (var node in InternalSearch(nodesEdges, root)) yield return node;
            _passed.Clear();
        }

        private IEnumerable<INode> InternalSearch(IDictionary<INode, IList<IEdge>> outputEdges , INode root)
        {
            _ = _passed.Add(root);
            yield return root;
            foreach (var node in outputEdges.GetListOrEmpty(root).Select(e => root.GetNext(e)).Where(n => !_passed.Contains(n)))
            {
                foreach (var n in InternalSearch(outputEdges, node)) yield return n;
            }
        }
        private IEnumerable<INode> InternalSearch(IDictionary<INode, IList<IArcs>> outputEdges, INode root)
        {
            _ = _passed.Add(root);
            yield return root;
            foreach (var node in outputEdges.GetListOrEmpty(root).Select(e => e.To).Where(n => !_passed.Contains(n)))
            {
                foreach (var n in InternalSearch(outputEdges, node)) yield return n;
            }
        }
    }
}
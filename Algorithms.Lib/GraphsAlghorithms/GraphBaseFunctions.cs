using Algorithms.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Algorithms.Lib.GraphsAlghorithms
{
    public static class GraphBaseFunctions
    {
        public static bool Contains(this IEdge edge, INode node) => edge.Node1 == node || edge.Node2 == node;
        public static bool Contains(this IArcs edge, INode node) => edge.From == node || edge.To == node;
        public static IEnumerable<INode> GetNodes(this IEnumerable<IEdge> edges) => edges.GetAllNodes().Distinct();
        public static IEnumerable<INode> GetNodes(this IEnumerable<IArcs> edges) => edges.GetAllNodes().Distinct();
        private static IEnumerable<INode> GetAllNodes(this IEnumerable<IEdge> edges)
        {
            foreach (var edge in edges)
            {
                yield return edge.Node1;
                yield return edge.Node2;
            }
        }
        private static IEnumerable<INode> GetAllNodes(this IEnumerable<IArcs> edges)
        {
            foreach (var edge in edges)
            {
                yield return edge.From;
                yield return edge.To;
            }
        }
        public static IEnumerable<IEdge> GetEdges(this INode node, IGraph graph) => node.GetEdges(graph.Edges);
        public static IEnumerable<IEdge> GetEdges(this INode node, IReadOnlyCollection<IEdge> edges) => edges.Where(e => e.Contains(node));
        public static IEnumerable<IArcs> GetOutputArcs(this INode node, IOrgraph orgraph) => node.GetOutputArcs(orgraph.Arcs);
        public static IEnumerable<IArcs> GetOutputArcs(this INode node, IReadOnlyCollection<IArcs> edges) => edges.Where(e => e.From == node);
        public static IEnumerable<IArcs> GetInputArcs(this INode node, IReadOnlyCollection<IArcs> edges) => edges.Where(e => e.To == node);
        public static IEnumerable<IArcs> GetArcs(this INode node, IReadOnlyCollection<IArcs> edges) => edges.Where(e => e.Contains(node));
        public static INode GetFirstNode(this IOrgraph graph) => graph.Nodes.FirstOrDefault();
        public static IEnumerable<INode> DeepSearch(this IGraph graph, INode root) => _deepSearch.Search(graph, root);
        public static IEnumerable<INode> DeepSearch(this IEnumerable<IEdge> edges, INode root) => _deepSearch.Search(edges, root);
        public static INode GetNext(this INode node, IEdge edge) => node switch
        {
            _ when node == edge.Node1 => edge.Node2,
            _ when node == edge.Node2 => edge.Node1,
            _ => throw new ArgumentException("Node is not belong edge")
        };
        private static readonly DeepSearch _deepSearch = new();
        public static bool IsBrige(this IEdge edge, INode root, IGraph graph)
        {
            var newEdges = graph.Edges.Where(e => e != edge).ToList();
            if (newEdges.Count == 0) return false;
            var treeNodes = newEdges.DeepSearch(root).ToHashSet();
            return !newEdges.GetNodes().All(n => treeNodes.Contains(n));
        }
    }
}
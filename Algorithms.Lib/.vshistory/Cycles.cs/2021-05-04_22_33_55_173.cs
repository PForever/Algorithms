using Algorithms.Lib.Implementations.Simple;
using Algorithms.Lib.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Lib
{
    public static class Cycles
    {
        public static IEnumerable<string> ApplayFleury(this IGraph graph)
        {
            var factory = new GraphAtoFactory();
            var cycle = factory.CreatePoute();
            var node = graph.GetFirstNode();
            int i = 0;
            do
            {
                var edges = node.GetEdges(graph).ToList();
                var p = edges.ToDictionary(e => e, e => !e.IsBrige(node.GetNext(e), graph));
                var edge = edges.FirstOrDefault(e => !e.IsBrige(node.GetNext(e), graph)) ?? edges.FirstOrDefault();
                yield return $"Step {i}, Cycle: {cycle.Print()}, Graph: {graph.Print()}, Current node: {node}, Current edge: {edge}";
                if (edge is null) break;
                node = node.GetNext(edge);
                graph.RemoveEdge(edge);
                cycle.Append(edge);
                i++;
            } while (true);
        }
        public static IEnumerable<IPath> ApplayRobertsFlores(this IOrgraph orgraph)
        {
            var factory = new GraphAtoFactory();
            var node = orgraph.GetFirstNode();
            return GetAllCircuit(node, orgraph, new HashSet<INode>(orgraph.Nodes.Count), node, factory, s => Console.WriteLine(s));
        }

        private static IPath GetCircuit(INode node, IOrgraph orgraph, HashSet<INode> notAllowed, INode root, IGraphAtoFactory factory, Action<string> OnStep)
        {
            notAllowed.Add(node);
            //OnStep($"{string.Join(", ", notAllowed)}");
            if (notAllowed.Count == orgraph.Nodes.Count && node.GetOutputArcs(orgraph).FirstOrDefault(a => a.To == root) is { } finalArc)
            {
                var circuit = factory.CreatePath();
                OnStep($"Add {finalArc}");
                circuit.Append(finalArc);
                return circuit;
            }
            foreach (var arc in node.GetOutputArcs(orgraph).Where(a => a.To.NotIn(notAllowed)))
            {
                var circuit = GetCircuit(arc.To, orgraph, notAllowed, root, factory, OnStep);
                if (circuit is null) continue;
                OnStep($"Add {arc}");
                circuit.Append(arc);
                return circuit;
            }
            notAllowed.Remove(node);
            return null;
        }
        private static IEnumerable<IPath> GetAllCircuit(INode node, IOrgraph orgraph, HashSet<INode> notAllowed, INode root, IGraphAtoFactory factory, Action<string> OnStep)
        {
            notAllowed.Add(node);
            //OnStep($"{string.Join(", ", notAllowed)}");
            if (notAllowed.Count == orgraph.Nodes.Count && node.GetOutputArcs(orgraph).FirstOrDefault(a => a.To == root) is { } finalArc)
            {
                OnStep($"S = {string.Join(", ", notAllowed)}, Current node: {node}, Current arc: ");
                var circuit = factory.CreatePath();
                OnStep($"Add {finalArc}");
                circuit.Append(finalArc);
                yield return circuit;
                yield break;
            }
            foreach (var arc in node.GetOutputArcs(orgraph).Where(a => a.To.NotIn(notAllowed)))
            {
                OnStep($"S = {string.Join(", ", notAllowed)}, Current node: {node}, Current arc: {arc}");
                var circuits = GetAllCircuit(arc.To, orgraph, new HashSet<INode>(notAllowed), root, factory, OnStep);
                foreach (var circuit in circuits)
                {
                    OnStep($"Add {arc}");
                    circuit.Append(arc);
                    yield return circuit;
                }
            }
        }
    }
}
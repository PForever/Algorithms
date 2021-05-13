using Algorithms.Lib.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Lib.GraphsAlghorithms
{
    public static class Cycles
    {
        public static IEnumerable<string> ApplayFleury(this IGraph graph, IGraphAtoFactory factory)
        {
            var cycle = factory.CreatePoute();
            var node = graph.GetFirstNode();
            int i = 0;
            string removed = "";
            do
            {
                var edges = node.GetEdges(graph).ToList();
                var edgesListInfo = string.Join(", ", edges.Select(e => $"{e.Print()} - {(e.IsBrige(node.GetNext(e), graph) ? "Мост" : "Не мост")}"));
                var edge = edges.FirstOrDefault(e => !e.IsBrige(node.GetNext(e), graph)) ?? edges.FirstOrDefault();
                yield return $"{(removed == "" ? "" : $"\tУдаляем из графа грань {removed}\n")}Шаг {i}\n\tЦикл: {cycle.Print()}\n\tГафф: {graph.Print()}\n\tТекущий узел: {node}, Нерассмотренные грани: {edgesListInfo}\n\tПервая не рассмотренная грань, не являющаяся мостом: {edge}";
                if (edge is null) break;
                node = node.GetNext(edge);
                removed = edge.Print();
                graph.RemoveEdge(edge);
                cycle.Append(edge);
                i++;
            } while (true);
        }
        public static IEnumerable<IPath> ApplayRobertsFlores(this IOrgraph orgraph, IGraphAtoFactory factory)
        {
            var node = orgraph.GetFirstNode();
            int i = 0;
            return GetAllCircuit(node, orgraph, new HashSet<INode>(orgraph.Nodes.Count), node, factory, s => Console.WriteLine($"Шаг {i++}.\n{s}"));
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
            var step = $"\tS = {{{string.Join(", ", notAllowed)}}}\n\tТекущий узел: {node}, Рассматриваема дуга: ";
            //OnStep($"{string.Join(", ", notAllowed)}");
            if (notAllowed.Count == orgraph.Nodes.Count && node.GetOutputArcs(orgraph).FirstOrDefault(a => a.To == root) is { } finalArc)
            {
                OnStep($"{step} {finalArc}\n\tцикл найден");
                var circuit = factory.CreatePath();
                circuit.Append(finalArc);
                yield return circuit;
                yield break;
            }
            foreach (var arc in node.GetOutputArcs(orgraph).Where(a => a.To.NotIn(notAllowed)))
            {
                OnStep($"{step} {arc}");
                var circuits = GetAllCircuit(arc.To, orgraph, new HashSet<INode>(notAllowed), root, factory, OnStep);
                foreach (var circuit in circuits)
                {
                    circuit.Append(arc);
                    yield return circuit;
                }
            }
        }
    }
}
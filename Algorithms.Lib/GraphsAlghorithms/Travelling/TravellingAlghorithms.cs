using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.Lib.GraphsAlghorithms.Travelling
{
    public class TravellingAlghorithms
    {
        private readonly Action<string> _log;

        public TravellingAlghorithms(Action<string> log)
        {
            _log = log;
        }
        public void Applay(int[,] incidence, params string[] names)
        {
            var nodeCount = incidence.GetLength(0);
            if (nodeCount != incidence.GetLength(1)) throw new ArgumentException("matrix must be square");
            if (names.Length == 0) names = Enumerable.Range(1, nodeCount).Select(i => i.ToString()).ToArray();
            else if (names.Length != nodeCount) throw new ArgumentException("count of names must be eaquals count of nodes");
            var nodes = new Dictionary<string, Node>(nodeCount);
            var arcs = new List<Arc>(nodeCount);
            for (int i = 0; i < nodeCount; i++)
            {
                var node1 = nodes.GetOrAdd(names[i], name => new Node(name));
                for (int j = 0; j < nodeCount; j++)
                {
                    if (incidence[i, j] == -1) continue;
                    var node2 = nodes.GetOrAdd(names[j], name => new Node(name));
                    arcs.Add(new Arc(node1, node2, incidence[i, j]));
                }
            }
            GraphWithPath graph = new GraphWithPath(arcs, _log);
            Path path = Find(graph);
            _log(path.ToString());
        }

        private Path Find(GraphWithPath graph)
        {
            _log(graph.ToString());
            var arc = graph.GetArc();
            if (arc is null) return graph.GetPath();

            //var marked = arc.To;

            var branch = graph.Clone();
            branch.Remove(arc);
            var main = graph.Clone();
            main.AddToPath(arc);
            //main.Remove(marked);

            return GetMin(branch, main);
        }

        private Path GetMin(GraphWithPath branch, GraphWithPath main)
        {
            var resultA = Find(main);
            if (resultA.Weight <= branch.Potential) return resultA;
            var resultB = Find(branch);
            return resultA.Weight > resultB.Weight ? resultB : resultA;
        }
    }

    internal class Path
    {
        public int Weight { get; internal set; }
        private List<Arc> _arcs = new();
        public void Append(Arc arc)
        {
            _arcs.Add(arc);
            Weight += arc.Weigth;
        }

        internal Path Clone() => new() { _arcs = new(_arcs), Weight = Weight };
        public override string ToString() => $"{_arcs.StrJoin()}";
    }

    internal class GraphWithPath
    {
        private Action<string> _log;
        private HashSet<Arc> _markedArcs = new();
        private HashSet<Node> _markedNodes = new();
        Dictionary<Node, (List<Arc> In, List<Arc> Out)> _nodes;
        public int Potential { get; private set; }
        private Path _path = new();
        private void Log(string message) => _log?.Invoke(message);
        private GraphWithPath()
        {

        }
        public GraphWithPath(IEnumerable<Arc> arcs, Action<string> log)
        {
            var arcsList = arcs.ToList();
            var @in = arcsList.ToLookup(a => a.To);
            var @out = arcsList.ToLookup(a => a.From);
            _nodes = @in.Select(k => k.Key).Union(@out.Select(k => k.Key)).ToDictionary(n => n, n => (new List<Arc>(@in[n]), new List<Arc>(@out[n])));
            _log = log;
            @in.Select(n => n.Key).ForEach(ChangePotential);

            //Могуть быть ноды, не имеющие входных дуг
            //@in.Select(n => n.First()).ForEach(ChangePotential);
        }
        internal void AddToPath(Arc arc)
        {
            Log($"\tДобавим {arc} к пути");
            _path.Append(arc);
            _markedNodes.Add(arc.From);
            _markedNodes.Add(arc.To);
        }

        internal GraphWithPath Clone() => new() { _markedArcs = new(_markedArcs), _markedNodes = new(_markedNodes), _nodes = _nodes, Potential = Potential, _path = _path.Clone(), _log = _log };
        internal Arc GetArc()
        {
            return _nodes.Where(n => !_markedNodes.Contains(n.Key)).SelectMany(n => n.Value.In).Where(a => !_markedArcs.Contains(a)).FirstOrDefault();
            //var min = (Arc: default(Arc), Potential: int.MaxValue);
            //Dictionary<Node, (int MinIn, int MinOut)> f;
            //foreach (var node in _nodes.Where(n => !_markedNodes.Contains(n.Key)).SelectMany(n => n.Value.In).Where(a => !_markedArcs.Contains(a)).Where())
            //{

            //}
            //foreach (var node in _nodes.Keys.Where(n => !_markedNodes.Contains(n)))
            //{
            //    var current = GetPotential(node);
            //    if (min.Potential > current.Potential) min = current;
            //}
            //Log($"Минимальный потенциал для {min.Potential} для {min.Arc}");
            //return min.Arc;
        }

        private (Arc Arc, int Potential) GetPotential(Node node)
        {
            Log($"\tВычисляем поенциал для {node}");
            Log($"\t\tВходящие дуги:");
            var arcIn = _nodes[node].In.Where(a => !_markedNodes.Contains(a.From) && !_markedArcs.Contains(a))
                                     .Meanwhile(a => Log($"\t\t\t{a}"))
                                     .MinElement(a => a.Weigth);
            Log($"\t\tМинимальная входящая: {arcIn}");
            Log($"\t\tИсходящие дуги:");
            var arcOut = _nodes[node].Out.Where(a => !_markedNodes.Contains(a.To) && !_markedArcs.Contains(a))
                                     .Meanwhile(a => Log($"\t\t\t{a}"))
                                     .MinElement(a => a.Weigth);
            Log($"\t\tМинимальная исходящая: {arcOut}");
            var arc = arcIn.Weigth < arcOut.Weigth ? arcIn : arcOut;
            return (arc, arc.Weigth - Potential);
        }

        internal Path GetPath() => _path;

        internal void Remove(Arc arc)
        {
            Log($"\tВычёркиваем {arc}");
            ChangePotential(arc);
            _markedArcs.Add(arc);
        }

        private void ChangePotential(Node node)
        {
            var minIn = _nodes[node].In.Where(a => !_markedNodes.Contains(a.From) && !_markedArcs.Contains(a)).Select(a => a.Weigth).Min();
            var minOut = _nodes[node].Out.Where(a => !_markedNodes.Contains(a.To) && !_markedArcs.Contains(a)).Select(a => a.Weigth).Min();

            Potential += minIn + minOut;
            Log($"\tНовый потенциал {Potential}");
        }

        private void ChangePotential(Arc arc)
        {
            var minIn = _nodes[arc.To].In.Where(a => !_markedNodes.Contains(a.From) && !_markedArcs.Contains(a)).Select(a => a.Weigth).Min();
            var minOut = _nodes[arc.From].Out.Where(a => !_markedNodes.Contains(a.To) && !_markedArcs.Contains(a)).Select(a => a.Weigth).Min();

            Potential += minIn + minOut;
            Log($"\tНовый потенциал {Potential}");
        }
        public override string ToString() => $"<{_nodes.Where(n => !_markedNodes.Contains(n.Key)).Select(s => s.Value).SelectMany(n => n.In).Where(a => !_markedArcs.Contains(a)).StrJoin(", ")}>";
    }

    internal class Arc
    {
        public Arc(Node from, Node to, int weigth)
        {
            From = from;
            To = to;
            Weigth = weigth;
        }
        public Node To { get; }
        public Node From { get; }
        public int Weigth { get; }
        public override string ToString() => $"({From},{To}:{Weigth})";

    }

    public class Node
    {

        public Node(string name) => Name = name;

        public string Name { get; }
        public override string ToString() => Name;
    }
}

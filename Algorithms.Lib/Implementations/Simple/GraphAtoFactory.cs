using Algorithms.Lib.Implementations.Pairs;
using Algorithms.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Lib.Implementations.Simple
{
    public class GraphAtoFactory : IGraphAtoFactory
    {
        public IRoute CreatePoute() => new Route();
        public IPath CreatePath() => new Path();
        public IGraph CreateGraph(params (string Node1, string Node2)[] edges)
        {
            var nodes = new Dictionary<string, INode>(edges.Length);
            var edgs = new List<IEdge>(edges.Length);
            foreach (var (n1, n2) in edges)
            {
                INode node1 = nodes.GetOrAdd(n1, CreateNode);
                INode node2 = nodes.GetOrAdd(n2, CreateNode);
                edgs.Add(CreateEdge(node1, node2));
            }
            return CreateGraph(nodes.Values, edgs);
        }
        public IGraph CreateGraph(IEnumerable<INode> nodes, IEnumerable<IEdge> edges) => new SimpleGraph(nodes, edges);
        public IOrgraph CreateOrgraph(IEnumerable<INode> nodes, IEnumerable<IArcs> edges) => new SimpleOrgraph(nodes, edges);
        public IEdge CreateEdge(INode node1, INode node2) => new Edge(node1, node2);
        public IArcs CreateArc(INode node1, INode node2) => new Arc(node1, node2);
        public INode CreateNode(string name) => new Node(name);
        public IPairNode CreatePairNode(string name, bool isX) => new PairNode(name, isX);

        public IOrgraph CreateOrgraph(int[,] incidence, params string[] names)
        {
            var nodeCount = incidence.GetLength(0);
            if (nodeCount != incidence.GetLength(1)) throw new ArgumentException("matrix must be square");
            if (names.Length == 0) names = Enumerable.Range(1, nodeCount).Select(i => i.ToString()).ToArray();
            else if(names.Length != nodeCount) throw new ArgumentException("count of names must be eaquals count of nodes");
            var nodes = new Dictionary<string, INode>(nodeCount);
            var arcs = new List<IArcs>(nodeCount);
            for (int i = 0; i < nodeCount; i++)
            {
                var node1 = nodes.GetOrAdd(names[i], CreateNode);
                for (int j = 0; j < nodeCount; j++)
                {
                    if (incidence[i, j] == 0) continue;
                    var node2 = nodes.GetOrAdd(names[j], CreateNode);
                    arcs.Add(CreateArc(node1, node2));
                }
            }
            return CreateOrgraph(nodes.Values, arcs);
        }
        public IOrgraph CreateOrgraph(params (string From, string To)[] edges)
        {
            var nodes = new Dictionary<string, INode>(edges.Length);
            var edgs = new List<IArcs>(edges.Length);
            foreach (var (n1, n2) in edges)
            {
                INode node1 = nodes.GetOrAdd(n1, CreateNode);
                INode node2 = nodes.GetOrAdd(n2, CreateNode);
                edgs.Add(CreateArc(node1, node2));
            }
            return CreateOrgraph(nodes.Values, edgs);
        }

        public ITwoPartsWeighedGraph CreateTwoPartsWeighedGraph(int[,] pair, string[] xNames, string[] yNames)
        {
            int xLength = pair.GetLength(0);
            int yLength = pair.GetLength(1);
            var nodeXs = new Dictionary<string, IPairNode>(xLength);
            var nodeYs = new Dictionary<string, IPairNode>(yLength);
            var edges = new List<IWeighedEdge>();
            for (int xIndex = 0; xIndex < xLength; xIndex++)
            {
                var node1 = nodeXs.GetOrAdd(xNames[xIndex], name => CreatePairNode(name, true));
                for (int yIndex = 0; yIndex < yLength; yIndex++)
                {
                    var node2 = nodeYs.GetOrAdd(yNames[yIndex], name => CreatePairNode(name, false));
                    var edge = CreateWeighedEdge(node1, node2, pair[xIndex, yIndex]);
                    edges.Add(edge);
                }
            }
            var graph = CreateTwoPartsWeighedGraph(nodeXs.Values, nodeYs.Values);
            edges.ForEach(e => graph.AddEdge(e));
            return graph;
        }

        public IWeighedEdge CreateWeighedEdge(IPairNode node1, IPairNode node2, int weight) => new WeightEdge(node1, node2, weight);

        public IWeighedPairsGraph CreateWeighedPairsGraph(IEnumerable<IPairNode> nodeXs, IEnumerable<IPairNode> nodeYs) => new WeighedPairsGraph(nodeXs, nodeYs);

        public ITwoPartsWeighedGraph CreateTwoPartsWeighedGraph(IEnumerable<IPairNode> nodeXs, IEnumerable<IPairNode> nodeYs) => new TwoPartsWeighedGraph(nodeXs, nodeYs);

        public ITwoPartsWeighedRout CreateTwoPartsWeighedRout(IPairNode start) => new TwoPartsWeighedRout(start);
    }
}
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

        public IOrgraph CreateWeighedGraph(int[,] incidence, params string[] names)
        {
            throw new NotImplementedException();
        }

        public IOrgraph CreateWeighedPairGraph(int[,] pair, string[] xNames, string[] yNames)
        {
            throw new NotImplementedException();
        }
    }
}
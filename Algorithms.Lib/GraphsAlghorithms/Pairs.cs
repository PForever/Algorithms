using Algorithms.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.Lib.GraphsAlghorithms
{
    public static class Pairs
    {
        public static IWeighedGraph ApplayHungarian(this IWeighedPairGraph graph, IGraphAtoFactory factory, Action<string> onStep)
        {
            if (graph.XNodes.Count > graph.YNodes.Count) throw new ArgumentException("число x элементов должно быть меньше или равно числу y элементов");
            int[] potentialX = new int[graph.XNodes.Count];
            int[] potentialY = new int[graph.YNodes.Count];

        }
    }

    class Hungarian
    {
        public void Applay(ITwoPartsGraph graph, IGraphAtoFactory factory)
        {
            var pairs = factory.CreatePairGraph(graph.NodeXs, graph.NodeYs);
            var hard = factory.CreateGraph(graph.Nodes);
            var potentials = CreatePotential(pairs);
            while (pairs.IsNotComplited())
            {
                var node = pairs.GetFree().First();
                if (!TryIncreasePairs(pairs, node, hard, out INode[] chainsNodesX, out INode[] chainsNodeZ))
                {
                    var (delta, edge) = GetMinDelta(graph, potentials, chainsNodesX, chainsNodeZ, factory);
                    hard.AddEdge(edge);
                }
            }
        }

        private bool TryIncreasePairs(IPirsGraph pairs, INode node, IGraph hard, out INode[] chainsNodesX, out INode[] chainsNodeZ)
        {
            IRoute chain = hard.GetChain(start: node);
            if (chain.IsNotMChainOf(pair))
            {
                chainsNodesX = GetX(chain);
                chainsNodesX = GetY(chain);
                return false;
            }
            pairs.ChangeByChain(chain);
            return true;
        }

        private (int Delta, IWeighedEdge Edge) GetMinDelta(ITwoPartsGraph graph, IDictionary<INode, int> potentials, INode[] chainsNodesX, INode[] notChainsNodeY)
        {
            int minDelta = 0;
            IWeighedEdge currentEdge = null;
            foreach (var nodeX in chainsNodesX)
            {
                foreach (var nodeY in notChainsNodeY)
                {
                    var (delta, edge) = GetDelta(graph, potentials, nodeX, nodeY);
                    if(delta < minDelta)
                    {
                        minDelta = delta;
                        currentEdge = edge;
                    }
                }
            }
            return (minDelta, currentEdge);
        }

        private (int Delta, IWeighedEdge Edge) GetDelta(ITwoPartsGraph graph, IDictionary<INode, int> potentials, INode nodeX, INode nodeY)
        {
            var edge = graph.WeighedEdges.Contains(nodeX, nodeY);
            return (edge.Weight - potentials[nodeX] - potentials[nodeY], edge);
        }

        private static IDictionary<INode, int> CreatePotential(IEnumerable<INode> nodes) => nodes.ToDictionary(n => n, _ => 0);
    }
}

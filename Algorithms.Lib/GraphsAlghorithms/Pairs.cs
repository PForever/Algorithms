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
        public static IWeighedPairsGraph ApplayHungarian(this ITwoPartsWeighedGraph graph, IGraphAtoFactory factory, Action<string> onStep)
        {
            var hungarian = new Hungarian(factory);
            return hungarian.Applay(graph);
        }
    }

    class Hungarian
    {
        private readonly IGraphAtoFactory _factory;

        public Hungarian(IGraphAtoFactory factory)
        {
            _factory = factory;
        }
        public IWeighedPairsGraph Applay(ITwoPartsWeighedGraph graph)
        {
            var pairs = _factory.CreateWeighedPairsGraph(graph.NodeXs, graph.NodeYs);
            var hard = _factory.CreateTwoPartsWeighedGraph(graph.Nodes, graph.NodeYs);
            var potentials = CreatePotential(graph.Nodes);
            while (pairs.GetFree().FirstOrDefault() is { } node)
            {
                if (!TryIncreasePairs(pairs, node, hard, out IPairNode[] chainsNodesX, out IPairNode[] chainsNodeY))
                {
                    var (delta, edge) = GetMinDelta(graph, potentials, chainsNodesX, chainsNodeY);
                    ChagePotential(potentials, delta, chainsNodesX, chainsNodeY);
                    /*if(edge != null) */hard.AddEdge(edge);
                }
            }
            return pairs;
        }

        private static void ChagePotential(IDictionary<IPairNode, int> potentials, int delta, IPairNode[] chainsNodesX, IPairNode[] chainsNodeY)
        {
            chainsNodesX.ForEach(n => potentials[n] += delta);
            chainsNodeY.ForEach(n => potentials[n] -= delta);
        }

        private bool TryIncreasePairs(IWeighedPairsGraph pairs, IPairNode node, ITwoPartsWeighedGraph hard, out IPairNode[] chainsNodeXs, out IPairNode[] chainsNodeYs)
        {
            var (chain, isMChain) = GetChain(hard, pairs, node, new HashSet<IPairNode>());
            if (!isMChain)
            {
                chainsNodeXs = chain.NodeXs.ToArray();
                chainsNodeYs = chain.NodeYs.ToArray();
                return false;
            }
            chainsNodeXs = chainsNodeYs = null;
            ChangePairsByChain(pairs, chain);
            return true;
        }

        private static void ChangePairsByChain(IWeighedPairsGraph pairs, ITwoPartsWeighedRout chain)
        {
            var pairsEdges = pairs.Edges.ToHashSet();
            var contais = new List<IWeighedEdge>(pairsEdges.Count);
            var notContais = new List<IWeighedEdge>(pairsEdges.Count);
            foreach(var edge in chain.Edges)
            {
                if (pairsEdges.Contains(edge)) contais.Add(edge);
                else notContais.Add(edge);
            }
            foreach (var edge in contais) pairs.RemoveEdge(edge);
            foreach (var edge in notContais) pairs.AddEdge(edge);
        }

        //private (ITwoPartsWeighedRout Chain, bool IsMChain) GetChain(ITwoPartsWeighedGraph hard, IWeighedPairsGraph pairs, PairNode start)
        //{
        //    var edges = hard.GetEdges(start.Node).ToList();
        //    if (edges.Count == 0) return (_factory.CreateTwoPartsWeighedRout(start), false);
        //    var notMChains = new List<ITwoPartsWeighedRout>();
        //    var mChains = new List<ITwoPartsWeighedRout>();
        //    foreach (var edge in edges)
        //    {
        //        var next = start.Node.GetNext(edge);
        //        var (chain, isMChain) = GetChainTail(hard, pairs, new PairNode(next, !start.IsX));
        //        if (isMChain) mChains.Add(chain);
        //        else notMChains.Add(chain);
        //        chain.AddEdge(edge);
        //    }
        //    return mChains.Count != 0 ? (mChains.MinElement(c => c.NodeCount), true) 
        //                           : (notMChains.MinElement(c => c.NodeCount), false);
        //}
        private (ITwoPartsWeighedRout Chain, bool IsMChain) GetChain(ITwoPartsWeighedGraph hard, IWeighedPairsGraph pairs, IPairNode node, HashSet<IPairNode> used)
        {
            used.Add(node);
            var edges = hard.GetEdges(node).Where(e => !used.Contains(e.GetNext(node))).ToList();
            if (edges.Count == 0) return (_factory.CreateTwoPartsWeighedRout(node), false);
            var notMChains = new List<ITwoPartsWeighedRout>();
            var mChains = new List<ITwoPartsWeighedRout>();
            foreach (var edge in edges)
            {
                var next = edge.GetNext(node);
                if (IsWhiteFree(next, pairs))
                {
                    var chain = _factory.CreateTwoPartsWeighedRout(node);
                    mChains.Add(chain);
                    chain.AddEdge(edge);
                }
                else
                {
                    var (chain, isMChain) = GetChain(hard, pairs, next, new HashSet<IPairNode>(used));
                    if (isMChain) mChains.Add(chain);
                    else notMChains.Add(chain);
                    chain.AddEdge(edge);
                }

            }
            return mChains.Count != 0 ? (mChains.MinElement(c => c.NodeCount), true)
                                   : (notMChains.MinElement(c => c.NodeCount), false);
        }

        //private static bool IsTailOfMChain(ITwoPartsWeighedRout chain, IWeighedPairsGraph pairs)
        //{
        //    var last = chain.GetLast();
        //    return IsWhiteFree(last, pairs);
        //}
        private static bool IsWhiteFree(IPairNode node, IWeighedPairsGraph pairs) => /*node.IsX &&*/ pairs.IsFree(node);
        private (int Delta, IWeighedEdge Edge) GetMinDelta(ITwoPartsWeighedGraph graph, IDictionary<IPairNode, int> potentials, IPairNode[] chainsNodesX, IPairNode[] chainsNodeY)
        {
            int minDelta = int.MaxValue;
            var notChainsNodeY = graph.NodeYs.Where(n => !chainsNodeY.Contains(n));
            IWeighedEdge currentEdge = null;
            foreach (var nodeX in chainsNodesX)
                foreach (var nodeY in notChainsNodeY)
                {
                    var (delta, edge) = GetDelta(graph, potentials, nodeX, nodeY);
                    if(delta <= minDelta)
                    {
                        minDelta = delta;
                        currentEdge = edge;
                    }
                }
            if (currentEdge == null) return (0, currentEdge);
            return (minDelta, currentEdge);
        }

        private (int Delta, IWeighedEdge Edge) GetDelta(ITwoPartsWeighedGraph graph, IDictionary<IPairNode, int> potentials, IPairNode nodeX, IPairNode nodeY)
        {
            var edge = graph.GetEdge(nodeX, nodeY);
            return (edge.Weight - potentials[nodeX] - potentials[nodeY], edge);
        }
        private static IDictionary<IPairNode, int> CreatePotential(IEnumerable<IPairNode> nodes) => nodes.ToDictionary(n => n, _ => 0);
    }
}

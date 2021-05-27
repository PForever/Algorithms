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
            var hungarian = new Hungarian(factory, onStep);
            return hungarian.Applay(graph);
        }
    }

    class Hungarian
    {
        private readonly IGraphAtoFactory _factory;
        private readonly Action<string> _onStep;

        public Hungarian(IGraphAtoFactory factory, Action<string> onStep)
        {
            _factory = factory;
            _onStep = onStep;
        }
        public IWeighedPairsGraph Applay(ITwoPartsWeighedGraph graph)
        {
            var pairs = _factory.CreateWeighedPairsGraph(graph.NodeXs, graph.NodeYs);
            var hard = _factory.CreateTwoPartsWeighedGraph(graph.NodeXs, graph.NodeYs);
            var potentials = CreatePotential(graph.Nodes);
            int stepIndex = 0;
            while (pairs.GetFree().FirstOrDefault() is { } node)
            {
                _onStep($"Шаг {stepIndex}");
                _onStep($"\tТекущая свободная вершина {node}");
                _onStep($"\tТекущие рёбра в графе H: {hard}");
                _onStep($"\tТекущие паросочетания: {pairs}");
                if (!TryIncreasePairs(pairs, node, hard, out IPairNode[] chainsNodeXs, out IPairNode[] chainsNodeYs))
                {
                    var (delta, edge) = GetMinDelta(graph, potentials, chainsNodeXs, chainsNodeYs);
                    ChagePotential(potentials, delta, chainsNodeXs, chainsNodeYs);
                    _onStep($"\tДобавим ребро {edge} в граф H");
                    hard.AddEdge(edge);
                }
                stepIndex++;
            }
            return pairs;
        }

        private void ChagePotential(IDictionary<IPairNode, int> potentials, int delta, IPairNode[] chainsNodeXs, IPairNode[] chainsNodeYs)
        {
            _onStep($"\tПересчитаем значения потенциалов");
            chainsNodeXs.ForEach(n => _onStep($"\t\ty({n}) <== {potentials[n]} + {delta} = {potentials[n] += delta}"));
            chainsNodeYs.ForEach(n => _onStep($"\t\ty({n}) <== {potentials[n]} - {delta} = {potentials[n] -= delta}"));
            _onStep($"\t\tТеукущая таблица потенциалов: |{potentials.Select(p => $"({p.Key}) = {p.Value}").StrJoin("|")}|");
        }

        private bool TryIncreasePairs(IWeighedPairsGraph pairs, IPairNode node, ITwoPartsWeighedGraph hard, out IPairNode[] chainsNodeXs, out IPairNode[] chainsNodeYs)
        {
            _onStep($"\tПытаемся посторить цепь из {node}");
            var (chain, isMChain) = GetChain(hard, pairs, node, new HashSet<IPairNode>());
            if (!isMChain)
            {
                chainsNodeXs = chain.NodeXs.ToArray();
                chainsNodeYs = chain.NodeYs.ToArray();
                _onStep($"\tМ-цепь не удалось построить ({chain} -- не М-цепь). Пройденные вершины: {string.Join(", ", chainsNodeXs.Select(n => n.Print()))}, {string.Join(", ", chainsNodeYs.Select(n => n.Print()))}");
                return false;
            }
            chainsNodeXs = chainsNodeYs = null;
            _onStep($"\tМ-цепь {chain}");
            ChangePairsByChain(pairs, chain);
            return true;
        }

        private void ChangePairsByChain(IWeighedPairsGraph pairs, ITwoPartsWeighedRout chain)
        {
            _onStep($"\tизменяем паросочетание {pairs}, удаляя тёмные и добавляя светлые рёбра цепи");
            var pairsEdges = pairs.Edges.ToHashSet();
            var contais = new List<IWeighedEdge>(pairsEdges.Count);
            var notContais = new List<IWeighedEdge>(pairsEdges.Count);
            foreach(var edge in chain.Edges)
            {
                if (pairsEdges.Contains(edge)) contais.Add(edge);
                else notContais.Add(edge);
            }
            _onStep($"\t\tДобавим {notContais.StrJoin()} в цепь");
            if(contais.Count != 0) _onStep($"\t\tУдалим {contais.StrJoin()}");

            foreach (var edge in contais) pairs.RemoveEdge(edge);
            foreach (var edge in notContais) pairs.AddEdge(edge);
            _onStep($"\tТекущее паросочетание {pairs}");
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
            var notChainsNodeY = graph.NodeYs.Where(n => !chainsNodeY.Contains(n)).ToArray();
            _onStep($"\tВычислим значение дельта на интервалах Z1 = {chainsNodesX.Select(n => n.Print()).StrJoin()}, Y\\Z2 = {notChainsNodeY.Select(n => n.Print()).StrJoin()}");
            _onStep($"\t\tТеукущая таблица потенциалов: |{potentials.Select(p => $"({p.Key}) = {p.Value}").StrJoin("|")}|");
            int minDelta = int.MaxValue;
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
            _onStep($"\t\tminDelta = {minDelta}, соответствующее ребру {currentEdge}");
            //if (minDelta == 0) throw new Exception("Дельта не может быть нулём");
            return (minDelta, currentEdge);
        }

        private (int Delta, IWeighedEdge Edge) GetDelta(ITwoPartsWeighedGraph graph, IDictionary<IPairNode, int> potentials, IPairNode nodeX, IPairNode nodeY)
        {
            var edge = graph.GetEdge(nodeX, nodeY);
            var delta = edge.Weight - potentials[nodeX] - potentials[nodeY];
            _onStep($"\t\t w({edge}) - y({nodeX}) - y({nodeY}) : {edge.Weight} - {potentials[nodeX]} - {potentials[nodeY]} = {delta}");
            return (delta, edge);
        }
        private static IDictionary<IPairNode, int> CreatePotential(IEnumerable<IPairNode> nodes) => nodes.ToDictionary(n => n, _ => 0);
    }
}

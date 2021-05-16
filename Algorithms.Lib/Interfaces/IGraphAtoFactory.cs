using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IGraphAtoFactory
    {
        IArcs CreateArc(INode from, INode to);
        IEdge CreateEdge(INode node1, INode node2);
        IOrgraph CreateOrgraph(IEnumerable<INode> nodes, IEnumerable<IArcs> edges);
        IGraph CreateGraph(IEnumerable<INode> nodes, IEnumerable<IEdge> edges);
        IOrgraph CreateOrgraph(params (string From, string To)[] edges);
        IGraph CreateGraph(params (string Node1, string Node2)[] edges);
        INode CreateNode(string name);
        IPairNode CreatePairNode(string name, bool isX);
        IRoute CreatePoute();
        IPath CreatePath();
        IOrgraph CreateOrgraph(int[,] incidence, params string[] names);
        IWeighedEdge CreateWeighedEdge(IPairNode node1, IPairNode node2, int weight);
        ITwoPartsWeighedGraph CreateTwoPartsWeighedGraph(int[,] pair, string[] xNames, string[] yNames);
        IWeighedPairsGraph CreateWeighedPairsGraph(IEnumerable<IPairNode> nodeXs, IEnumerable<IPairNode> nodeYs);
        ITwoPartsWeighedGraph CreateTwoPartsWeighedGraph(IEnumerable<IPairNode> nodeXs, IEnumerable<IPairNode> nodeYs);
        ITwoPartsWeighedRout CreateTwoPartsWeighedRout(IPairNode start);
    }
}
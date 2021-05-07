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
        IRoute CreatePoute();
        IPath CreatePath();
    }
}
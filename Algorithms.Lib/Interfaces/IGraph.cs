using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IGraph : IOrgraph
    {
        IReadOnlyCollection<IEdge> Edges { get; }
        void RemoveEdge(IEdge edge);
    }
    public interface IOrgraph : IPrintable
    {
        IReadOnlyCollection<INode> Nodes { get; }
        IReadOnlyCollection<IArcs> Arcs { get; }
        void RemoveNode(INode node);
        void RemoveArc(IArcs edge);
    }
}
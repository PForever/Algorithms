using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IOrgraph : IPrintable
    {
        IReadOnlyCollection<INode> Nodes { get; }
        IReadOnlyCollection<IArcs> Arcs { get; }
        void RemoveNode(INode node);
        void RemoveArc(IArcs edge);
    }
}
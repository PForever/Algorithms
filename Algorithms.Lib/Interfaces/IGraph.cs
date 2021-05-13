using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IGraph : IOrgraph
    {
        IReadOnlyCollection<IEdge> Edges { get; }
        void RemoveEdge(IEdge edge);
    }
    
}
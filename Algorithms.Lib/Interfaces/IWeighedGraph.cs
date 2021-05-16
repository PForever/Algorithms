using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IWeighedGraph : IWeighedOrgraph, IGraph
    {
        IReadOnlyCollection<IWeighedEdge> WeighedEdges { get; }
        void RemoveEdge(IWeighedEdge edge);
        void AddEdge(IWeighedEdge edge);
    }
}
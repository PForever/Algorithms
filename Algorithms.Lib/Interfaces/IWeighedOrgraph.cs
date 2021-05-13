using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IWeighedOrgraph : IOrgraph
    {
        IReadOnlyCollection<IWeighedArcs> WeighedArcs { get; }
        void RemoveArc(IWeighedArcs edge);
    }
}
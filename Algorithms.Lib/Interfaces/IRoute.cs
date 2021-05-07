using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IRoute : IPrintable
    {
        IReadOnlyCollection<IEdge> Edges { get; }
        void Append(IEdge edge);
    }
}
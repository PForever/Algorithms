using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IPath : IPrintable
    {
        IReadOnlyCollection<IArcs> Arcs { get; }
        void Append(IArcs edge);
    }
}
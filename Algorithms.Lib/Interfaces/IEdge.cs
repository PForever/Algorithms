using System;

namespace Algorithms.Lib.Interfaces
{
    public interface IEdge : IPrintable
    {
        INode Node1 { get; }
        INode Node2 { get; }
    }
}
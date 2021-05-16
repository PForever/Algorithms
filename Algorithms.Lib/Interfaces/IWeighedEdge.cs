using System;

namespace Algorithms.Lib.Interfaces
{
    public interface IWeighedEdge : IPrintable
    {
        int Weight { get; }
        IPairNode Node1 { get; }
        IPairNode Node2 { get; }
        IPairNode GetNext(IPairNode node) => node switch
        {
            _ when node == Node1 => Node2,
            _ when node == Node2 => Node1,
            _ => throw new ArgumentException("Node is not belong edge")
        };
    }
    public interface IPairNode : INode
    {
        bool IsX { get; }
    }
}
namespace Algorithms.Lib.Interfaces
{
    public interface IWeighedArcs : IPrintable
    {
        INode From { get; }
        INode To { get; }
        int Weight { get; }
    }
}
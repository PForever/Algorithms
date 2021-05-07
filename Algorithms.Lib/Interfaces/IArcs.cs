namespace Algorithms.Lib.Interfaces
{
    public interface IArcs : IPrintable
    {
        INode From { get; }
        INode To { get; }
    }
}
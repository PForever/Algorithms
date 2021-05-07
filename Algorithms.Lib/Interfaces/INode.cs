namespace Algorithms.Lib.Interfaces
{
    public interface INode : IPrintable
    {
        string Name { get; }
    }
    //public interface INode
    //{
    //    string Name { get; }
    //    IReadOnlyCollection<INode> OutputNodes { get; }
    //    IReadOnlyCollection<INode> InputNodes { get; }
    //    void AddOutputNodes(INode output);
    //    void RemoveOutputNodes(INode output);
    //    void AddInputNodes(INode input);
    //    void RemoveInputNodes(INode input);
    //}
}
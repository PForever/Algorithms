using Algorithms.Lib.Interfaces;

namespace Algorithms.Lib.Implementations.Simple
{
    public class PairNode : IPairNode
    {
        public PairNode(string name, bool isX)
        {
            Name = name;
            IsX = isX;
        }
        public string Name { get; }
        public bool IsX { get; }

        public string Print() => Name;
        public override string ToString() => Print();
    }
}
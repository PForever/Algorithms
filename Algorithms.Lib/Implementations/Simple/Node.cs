using Algorithms.Lib.Interfaces;

namespace Algorithms.Lib.Implementations.Simple
{
    class Node : INode
    {
        public Node(string name)
        {
            Name = name;
        }
        public string Name { get; }

        public string Print() => Name;
        public override string ToString() => Print();
    }
}
using Algorithms.Lib.Interfaces;
using System;

namespace Algorithms.Lib.Implementations.Simple
{
    class Edge : IEdge
    {

        public Edge(INode node1, INode node2)
        {
            Node1 = node1 ?? throw new ArgumentNullException(nameof(node1));
            Node2 = node2 ?? throw new ArgumentNullException(nameof(node2));
        }

        public INode Node1 { get; }
        public INode Node2 { get; }

        public bool Equals(IEdge other) => ReferenceEquals(other, this) || other is not null && (other.Node1 == Node1 && other.Node2 == Node2 || other.Node2 == Node1 && other.Node1 == Node2);

        public override bool Equals(object obj) => Equals(obj as IEdge);

        public override int GetHashCode() => HashCode.Combine(Node1, Node2);
        public string Print() => $"{{{Node1.Print()}, {Node2.Print()}}}";
        public override string ToString() => Print();
    }
}
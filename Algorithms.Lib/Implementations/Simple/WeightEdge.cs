using Algorithms.Lib.Interfaces;
using System;

namespace Algorithms.Lib.Implementations.Simple
{
    class WeightEdge : IWeighedEdge, IEquatable<IWeighedEdge>
    {
        public WeightEdge(IPairNode node1, IPairNode node2, int weight)
        {
            Node1 = node1 ?? throw new ArgumentNullException(nameof(node1));
            Node2 = node2 ?? throw new ArgumentNullException(nameof(node2));
            Weight = weight;
        }

        public IPairNode Node1 { get; }
        public IPairNode Node2 { get; }

        public int Weight { get; }

        public bool Equals(IWeighedEdge other) => ReferenceEquals(other, this) || other is not null && (other.Node1 == Node1 && other.Node2 == Node2 || other.Node2 == Node1 && other.Node1 == Node2) && other.Weight == Weight;

        public override bool Equals(object obj) => Equals(obj as IWeighedEdge);

        public override int GetHashCode() => HashCode.Combine(Node1.GetHashCode() ^ Node2.GetHashCode(), Weight);
        public string Print() => $"{{{Node1.Print()}, {Node2.Print()}, W = {Weight}}}";
        public override string ToString() => Print();
    }
}
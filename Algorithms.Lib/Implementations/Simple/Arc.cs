using Algorithms.Lib.Interfaces;
using System;

namespace Algorithms.Lib.Implementations.Simple
{
    class Arc : IArcs
    {

        public Arc(INode from, INode to)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
        }

        public INode From { get; }
        public INode To { get; }

        public bool Equals(IArcs other) => ReferenceEquals(other, this) || other is not null && other.From == From && other.To == To;

        public override bool Equals(object obj) => Equals(obj as IArcs);

        public override int GetHashCode() => HashCode.Combine(From, To);
        public string Print() => $"<{From.Print()}, {To.Print()}>";
        public override string ToString() => Print();
    }
}
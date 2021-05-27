using System;
using System.Collections.Generic;

namespace Algorithms.Lib.GraphsAlghorithms.Trees
{
    public class Node<T> where T : IComparable<T>
    {
        private Node<T> _parent;
        readonly List<Node<T>> _children = new();
        public readonly T Value;
        public Node(T value)
        {
            Value = value;
        }

        public Node<T> GetParent() => _parent;
        public void SetParent(Node<T> value)
        {
            if (_parent is not null) _parent._children.Remove(this);
            _parent = value;
            if (value is not null) value._children.Add(this);
        }
        public IReadOnlyCollection<Node<T>> Children => _children;
        public override string ToString() => Value.ToString();
    }
}

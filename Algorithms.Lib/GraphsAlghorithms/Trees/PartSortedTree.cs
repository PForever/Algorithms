using System;
using System.Linq;

namespace Algorithms.Lib.GraphsAlghorithms.Trees
{
    public class PartSortedTree<T> where T : IComparable<T>
    {
        readonly int ChildLimit = 3;
        bool IsFree(Node<T> node) => node.Children.Count < ChildLimit;
        Node<T> NextLast() => LastFree.Children.First();
        Node<T> LastFree { get; set; }
        public void Insert(T value)
        {
            var lastFree = LastFree;
            if (LastFree == null)
            {
                LastFree = new Node<T>(value);
                return;
            }
            Insert(value, lastFree);
            if (IsFree(lastFree)) return;
            LastFree = NextLast();
        }
        
        void Insert(T value, Node<T> node) 
        {
            Node<T> previously = null;
            if(value.CompareTo(node.Value) >= 0)
            {
                new Node<T>(value).SetParent(node);
                return;
            }
            while (node.GetParent() is not null && value.CompareTo(node.GetParent().Value) < 0)
            {
                previously = node;
                node = node.GetParent();
            }
            var newParent = new Node<T>(value);
            newParent.SetParent(node.GetParent());
            node.SetParent(newParent);
            var children = node.Children;
            
            var list = node.Children.Where(n => n != previously).ToList();
            list.ForEach(n => n.SetParent(newParent));
        }
    }
}

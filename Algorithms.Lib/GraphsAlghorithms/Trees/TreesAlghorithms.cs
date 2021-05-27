using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.Lib.GraphsAlghorithms.Trees
{

    public static class TreesAlghorithms
    {
        public static PartSortedTree<T> CreatePartSortTree<T>(params T[] values) where T : IComparable<T>
        {
            var tree = new PartSortedTree<T>();
            values.ForEach(v => tree.Insert(v));
            return tree;
        }
    }
}

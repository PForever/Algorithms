using System;
using System.Collections.Generic;

namespace Algorithms.Lib.GraphsAlghorithms.Trees
{
    public static class PartSortedHeap
    {
        public static HeapTree<T> CreateHeapTree<T>(params T[] array) where T : IComparable<T>
        {
            var tree = new HeapTree<T>();
            array.ForEach(i => tree.Insert(i));
            return tree;
        }
    }

    public class HeapTree<T> where T : IComparable<T>
    {
        private static readonly int StartCapacity = 256;
        private static readonly int RootIndex = 0;
        private static readonly int StopIndex = -1;
        private readonly List<(T Value, bool Initialized)> _heap = new(StartCapacity);
        private int _lastFreeParentIndex = -1;
        private static int ParentIndex(int index) => index == 0 ? -1 : (index - 1) / 2;
        private static int LeftChildIndex(int index) => index * 2 + 1;
        private static int RightChildIndex(int index) => index * 2 + 2;
        private bool IsEmpty(int index) => _heap.Count <= index || !_heap[index].Initialized;
        private void Switch(int indexA, int indexB)
        {
            var temp = _heap[indexA];
            _heap[indexA] = _heap[indexB];
            _heap[indexB] = temp;
        }
        public void Insert(T value)
        {
            var index = Append(value);
            if (index == 0) return;
            Surfacing(index);
        }

        private int Append(T value)
        {
            int parentIndex = _lastFreeParentIndex;
            if (parentIndex == StopIndex) 
            {
                Set(RootIndex, value);
                _lastFreeParentIndex = RootIndex;
                return RootIndex;
            }
            int leftChildIndex = LeftChildIndex(parentIndex);
            int rightChildIndex = RightChildIndex(parentIndex);
            int index;
            if (IsEmpty(leftChildIndex))
            {
                Set(leftChildIndex, value);
                index = leftChildIndex;
            }
            else if (IsEmpty(rightChildIndex))
            {
                Set(rightChildIndex, value);
                _lastFreeParentIndex = index = rightChildIndex;
            }
            else throw new ArgumentOutOfRangeException("Что-то пошло не так и _lastFreeParentIndex вернул несвободного предка");
            return index;
        }
        private void Set(int index, T value)
        {
            if (_heap.Count <= index) 
            {
                long newCapacity = (long)StartCapacity * (_heap.Count + 1);
                if (newCapacity + _heap.Count > int.MaxValue) throw new IndexOutOfRangeException("Куча превысила допустимое пространство");
                for (int i = 0; i < newCapacity; i++) _heap.Add(default);
            }
            _heap[index] = (value, true);
        }
        private void Surfacing(int index)
        {
            int parentIndex = ParentIndex(index);
            if (parentIndex == StopIndex || _heap[index].Value.CompareTo(_heap[parentIndex].Value) >= 0) return;
            Switch(index, parentIndex);
            Surfacing(parentIndex);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace BigSortingAlgorithm.Internal
{
    /// <summary>
    /// Реализация двоичной кучи
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class BinaryHeap<T> : IEnumerable<T>
    {
        private readonly IComparer<T> Comparer;
        private readonly List<T> Items;

        public BinaryHeap()
            : this(0, Comparer<T>.Default)
        {
        }

        public BinaryHeap(int capacity)
            : this(capacity, Comparer<T>.Default)
        {
        }

        public BinaryHeap(int capacity, IComparer<T> comp)
        {
            Comparer = comp;
            Items = new List<T>(capacity);
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)Items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            Items.Clear();
        }

        public void TrimExcess()
        {
            Items.TrimExcess();
        }

        public void Insert(T newItem)
        {
            var i = Count;
            Items.Add(newItem);
            while (i > 0 && Comparer.Compare(Items[(i - 1) / 2], newItem) > 0)
            {
                Items[i] = Items[(i - 1) / 2];
                i = (i - 1) / 2;
            }
            Items[i] = newItem;
        }

        public T Peek()
        {
            ThrowIfHeapEmpty();
            return Items[0];
        }

        public T RemoveRoot()
        {
            ThrowIfHeapEmpty();

            var rslt = Items[0];

            var tmp = Items[Items.Count - 1];
            Items.RemoveAt(Items.Count - 1);
            if (Items.Count > 0)
            {
                var i = 0;
                while (i < Items.Count / 2)
                {
                    var j = 2 * i + 1;
                    if ((j < Items.Count - 1) && (Comparer.Compare(Items[j], Items[j + 1]) > 0))
                    {
                        ++j;
                    }
                    if (Comparer.Compare(Items[j], tmp) >= 0)
                    {
                        break;
                    }
                    Items[i] = Items[j];
                    i = j;
                }
                Items[i] = tmp;
            }
            return rslt;
        }

        private void ThrowIfHeapEmpty()
        {
            if (Items.Count == 0)
            {
                throw new InvalidOperationException("The heap is empty.");
            }
        }
    }
}

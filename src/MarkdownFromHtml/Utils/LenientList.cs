using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.Utils
{
    class LenientList<T> : IList<T>
    {
        private List<T> _list;
        private Func<T> _initializer;

        public T this[int index]
        {
            get
            {
                ExpandForIndex(index);
                return _list[index];
            }
            set
            {
                ExpandForIndex(index);
                _list[index] = value;
            }
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public LenientList(T initializeValue) : this(() => initializeValue)
        { }

        public LenientList(Func<T> initializer)
        {
            _list = new();
            _initializer = initializer;
        }

        public LenientList(IEnumerable<T> array, T initializeValue) : this(array, () => initializeValue)
        { }

        public LenientList(IEnumerable<T> array, Func<T> initializer)
        {
            _list = new(array);
            _initializer = initializer;
        }


        public void Add(T item) => _list.Add(item);

        public void Clear() => _list.Clear();

        public bool Contains(T item) => _list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = 0;
            for (; arrayIndex < array.Length; ++arrayIndex)
            {
                array[arrayIndex] = this[i++];
            }
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        public int IndexOf(T item) => _list.IndexOf(item);

        public void Insert(int index, T item)
        {
            ExpandForIndex(index - 1);
            _list.Insert(index, item);
        }

        public bool Remove(T item) => _list.Remove(item);

        public void RemoveAt(int index)
        {
            ExpandForIndex(index);
            _list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public void Expand(int count)
            => ExpandForIndex(count - 1);

        private void ExpandForIndex(int index)
        {
            int len = index - _list.Count + 1;
            for (var i = 0; i < len; ++i)
                _list.Add(_initializer());
        }
    }

    static class LenientListExt
    {
        public static void SetIfMax<T>(this LenientList<T> list, int at, T value)
            where T : IComparable
        {
            if (list[at].CompareTo(value) < 0)
                list[at] = value;
        }

    }
}

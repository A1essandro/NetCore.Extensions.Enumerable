using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DotNetCross.Memory;
using Extensions.Enumerable.Internal.Helpers;

[assembly: InternalsVisibleToAttribute("Extensions.Enumerable.Tests")]

namespace Extensions.Enumerable.Internal.Collections
{

    internal class AvoidingLargeObjectHeapCollection<T> : IAvoidingLargeObjectHeapCollection<T>
    {

        private static int _LargeObjectHeapThreshold = 85000;

        private readonly int _maxEntriesPartSize;
        private IList<T[]> _entriesParts;
        private int _entryCursor = 0;
        private int _partCursor = 0;

        public AvoidingLargeObjectHeapCollection(IEnumerable<T> source)
        {
            int tSize = Unsafe.SizeOf<T>();

            _maxEntriesPartSize = (_LargeObjectHeapThreshold / tSize / 4) * 3;

            foreach (var item in source)
            {
                Add(item);
            }
        }

        public T this[int index]
        {
            get
            {
                var decomposed = IndexHelper.Decompose(index, _maxEntriesPartSize);
                if (decomposed.Item1 >= _partCursor - 1 && decomposed.Item2 >= _entryCursor)
                    throw new IndexOutOfRangeException();
                return _entriesParts[decomposed.Item1][decomposed.Item2];
            }
            set
            {
                var decomposed = IndexHelper.Decompose(index, _maxEntriesPartSize);
                if (decomposed.Item1 >= _partCursor - 1 && decomposed.Item2 >= _entryCursor)
                    throw new IndexOutOfRangeException();
                _entriesParts[decomposed.Item1][decomposed.Item2] = value;
            }
        }

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count
        {
            get
            {
                return _partCursor == 0
                    ? 0
                    : (_partCursor - 1) * _maxEntriesPartSize + _entryCursor;
            }
        }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
        public bool IsReadOnly => false;

        /// <inheritdoc cref="ICollection{T}.Add"/>
        public void Add(T item)
        {
            if (_partCursor == 0)
            {
                _entriesParts = new List<T[]>(1)
                {
                    new T[_maxEntriesPartSize]
                };
                _partCursor = 1;
            }

            if (_entryCursor >= _maxEntriesPartSize)
            {
                if (_entryCursor > _maxEntriesPartSize)
                    throw new IndexOutOfRangeException();

                _entriesParts.Add(new T[_maxEntriesPartSize]);
                _partCursor++;
                _entryCursor = 0;
            }

            _entriesParts[_partCursor - 1][_entryCursor] = item;
            _entryCursor++;
        }

        /// <inheritdoc cref="ICollection{T}.Clear"/>
        public void Clear() => _entriesParts.Clear();

        /// <inheritdoc cref="ICollection{T}.Contains"/>
        public bool Contains(T item)
        {
            foreach (T entry in this)
            {
                if (EqualityComparer<T>.Default.Equals(entry, item))
                    return true;
            }

            return false;
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo"/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex <= 0)
                throw new ArgumentException("Destination array was not long enough. Check the destination index, length, and the array's lower bounds.");

            var sourceI = 0;
            for (int destI = arrayIndex; sourceI < Count; destI++)
            {
                array[destI] = this[sourceI++];
            }
        }

        /// <inheritdoc cref="ICollection{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator()
        {
            for (int part = 0; part < _partCursor; part++)
            {
                for (int entry = 0; entry < _entryCursor; entry++)
                {
                    yield return _entriesParts[part][entry];
                }
            }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
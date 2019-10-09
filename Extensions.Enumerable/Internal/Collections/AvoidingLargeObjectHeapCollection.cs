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

        private int _maxEntriesPartSize;
        private IList<T[]> _entriesParts;
        private int _entryCursor = -1;
        private int _partCursor = -1;

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
                if (decomposed.Item1 == _partCursor && decomposed.Item2 > _entryCursor)
                    throw new IndexOutOfRangeException();
                return _entriesParts[decomposed.Item1][decomposed.Item2];
            }
            set
            {
                var decomposed = IndexHelper.Decompose(index, _maxEntriesPartSize);
                if (decomposed.Item1 == _partCursor && decomposed.Item2 > _entryCursor)
                    throw new IndexOutOfRangeException();
                _entriesParts[decomposed.Item1][decomposed.Item2] = value;
            }
        }

        /// <inheritdoc cref="IAvoidingLargeObjectHeapCollection{T}.Count"/>
        public int Count => _partCursor * _maxEntriesPartSize + _entryCursor + 1;

        /// <inheritdoc cref="IAvoidingLargeObjectHeapCollection{T}.IsReadOnly"/>
        public bool IsReadOnly => false;

        /// <inheritdoc cref="IAvoidingLargeObjectHeapCollection{T}.Add"/>
        public void Add(T item)
        {
            if (_partCursor < 0)
            {
                _entriesParts = new List<T[]>(1);
                _entriesParts.Add(new T[_maxEntriesPartSize]);
                _partCursor = 0;
            }

            _entryCursor++;
            if (_entryCursor >= _maxEntriesPartSize)
            {
                if (_entryCursor > _maxEntriesPartSize)
                    throw new IndexOutOfRangeException();

                _entriesParts.Add(new T[_maxEntriesPartSize]);
                _partCursor++;
                _entryCursor = 0;
            }

            _entriesParts[_partCursor][_entryCursor] = item;
        }

        /// <inheritdoc cref="IAvoidingLargeObjectHeapCollection{T}.Clear"/>
        public void Clear() => _entriesParts.Clear();

        /// <inheritdoc cref="IAvoidingLargeObjectHeapCollection{T}.Contains"/>
        public bool Contains(T item)
        {
            foreach (T entry in this)
            {
                if (EqualityComparer<T>.Default.Equals(entry, item))
                    return true;
            }

            return false;
        }

        /// <inheritdoc cref="IAvoidingLargeObjectHeapCollection{T}.CopyTo"/>
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

        /// <inheritdoc cref="IAvoidingLargeObjectHeapCollection{T}.GetEnumerator"/>
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
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
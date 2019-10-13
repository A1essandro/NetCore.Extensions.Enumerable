using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DotNetCross.Memory;
using Extensions.Enumerable.Internal.Helpers;

[assembly: InternalsVisibleTo("Extensions.Enumerable.Tests")]

namespace Extensions.Enumerable.Internal.Collections
{

    internal class AvoidingLargeObjectHeapReadOnlyCollection<T> : IAvoidingLargeObjectHeapReadOnlyCollection<T>
    {

        private static int _LargeObjectHeapThreshold = 85000;

        private readonly int _maxEntriesPartSize;
        private List<List<T>> _entriesParts;
        private int _entryCursor = 0;
        private int? _count = null;

        public AvoidingLargeObjectHeapReadOnlyCollection(IEnumerable<T> source)
        {
            int tSize = Unsafe.SizeOf<T>();

            _maxEntriesPartSize = (_LargeObjectHeapThreshold / tSize / 4) * 3;

            foreach (var item in source)
            {
                _add(item);
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index > Count)
                    throw new IndexOutOfRangeException();
                var decomposed = IndexHelper.Decompose(index, _maxEntriesPartSize);
                return _entriesParts[decomposed.Item1][decomposed.Item2];
            }
            set
            {
                if (index < 0 || index > Count)
                    throw new IndexOutOfRangeException();
                var decomposed = IndexHelper.Decompose(index, _maxEntriesPartSize);
                _entriesParts[decomposed.Item1][decomposed.Item2] = value;
            }
        }

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count
        {
            get
            {
                if (_count.HasValue)
                    return _count.Value;
                if (_entriesParts.Count > 0)
                    return (_entriesParts.Count - 1) * _maxEntriesPartSize + _entryCursor;
                return 0;
            }
        }

        /// <inheritdoc cref="ICollection{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator()
        {
            for (int part = 0; part < _entriesParts.Count; part++)
            {
                var countInPart = part == _entriesParts.Count - 1
                    ? _entryCursor
                    : _maxEntriesPartSize;
                for (int entry = 0; entry < countInPart; entry++)
                {
                    yield return _entriesParts[part][entry];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="ICollection{T}.Add"/>
        public void _add(T item)
        {
            if (_entriesParts.Count == 0)
            {
                _entriesParts = new List<List<T>>(1)
                {
                    _getNewPart()
                };
            }

            if (_entryCursor >= _maxEntriesPartSize)
            {
                if (_entryCursor > _maxEntriesPartSize)
                    throw new IndexOutOfRangeException();

                _entriesParts.Add(_getNewPart());
                _entryCursor = 0;
            }

            _entriesParts[_entriesParts.Count - 1][_entryCursor] = item;
            _entryCursor++;
        }

        private List<T> _getNewPart() => new List<T>((_maxEntriesPartSize + 1) / 2);

    }
}
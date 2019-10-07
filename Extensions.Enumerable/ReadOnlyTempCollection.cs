using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Extensions.Enumerable
{

    public readonly struct ReadOnlyTempCollection<T> : IReadOnlyCollection<T>, IDisposable
    {

        private readonly T[] _collection;

        private readonly int _lenght;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyTempCollection(IEnumerable<T> source, int maxSize)
        {
            _collection = ArrayPool<T>.Shared.Rent(maxSize);

            _lenght = 0;
            foreach (T item in source)
            {
                _collection[_lenght++] = item;
            }
        }

        /// <inheritdoc/>
        public int Count => _lenght;

        public T this[int index] => index >= _lenght ? throw new IndexOutOfRangeException() : _collection[index];

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _lenght; i++)
            {
                yield return _collection[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(_collection);
        }
    }
}
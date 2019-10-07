using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Extensions.Enumerable
{

    /// <summary>
    /// Value type which implements <see cref="IReadOnlyCollection{T}"/>, <see cref="IDisposable"/> and access by index.
    /// </summary>
    public readonly struct ReadOnlyTempCollection<T> : IReadOnlyCollection<T>, IDisposable
    {

        private readonly T[] _collection;

        private readonly int _lenght;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="source">Source enumerable</param>
        /// <param name="maxSize">Max size for collection</param>
        /// <exception cref="IndexOutOfRangeException">If maxSize is less then source</exception> 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyTempCollection(IEnumerable<T> source, int maxSize)
        {
            _collection = ArrayPool<T>.Shared.Rent(maxSize);
            _lenght = 0;

            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                bool step = false;
                while (_lenght <= maxSize && (step = enumerator.MoveNext()))
                {
                    _collection[_lenght++] = enumerator.Current;
                }

                if (step)
                {
                    throw new IndexOutOfRangeException();
                }
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
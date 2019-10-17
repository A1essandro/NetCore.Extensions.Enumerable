using System.Collections.Generic;
using Extensions.Enumerable.Internal.Collections;

namespace Extensions.Enumerable
{
    public static class Extensions
    {

        /// <summary>
        /// Getting value type which implements <see cref="IReadOnlyCollection{T}"/>, <see cref="IDisposable"/> and access by index.
        /// This rents collection from pool which leads to less memory allocation.
        /// </summary>
        /// <param name="source">Source enumerable</param>
        /// <param name="maxSize">Max size for collection</param>
        /// <exception cref="IndexOutOfRangeException">If maxSize is less then source</exception>
        public static ReadOnlyTempCollection<T> ToTemp<T>(this IEnumerable<T> source, int maxSize = 512)
        {
            return new ReadOnlyTempCollection<T>(source, maxSize);
        }

        /// <summary>
        /// Getting value type which implements <see cref="IAvoidingLargeObjectHeapCollection{T}"/>
        /// Use for avoiding allocation large collection in the large object heap
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        public static IAvoidingLargeObjectHeapCollection<T> ToAvoidingLohCollection<T>(this IEnumerable<T> source)
        {
            return new AvoidingLargeObjectHeapCollection<T>(source);
        }

        /// <summary>
        /// Getting value type which implements <see cref="IAvoidingLargeObjectHeapReadOnlyCollection{T}"/>
        /// Use for avoiding allocation large collection in the large object heap
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        public static IAvoidingLargeObjectHeapReadOnlyCollection<T> ToAvoidingLohReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            return new AvoidingLargeObjectHeapReadOnlyCollection<T>(source);
        }

    }
}
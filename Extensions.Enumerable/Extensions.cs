using System.Collections.Generic;

namespace Extensions.Enumerable
{
    public static class Extensions
    {

        public static ReadOnlyTempCollection<T> ToTemp<T>(this IEnumerable<T> source, int maxSize = 512)
        {
            return new ReadOnlyTempCollection<T>(source, maxSize);
        }

    }
}
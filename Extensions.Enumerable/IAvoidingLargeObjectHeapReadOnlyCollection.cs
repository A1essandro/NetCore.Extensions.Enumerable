using System.Collections.Generic;

namespace Extensions.Enumerable
{
    public interface IAvoidingLargeObjectHeapReadOnlyCollection<T> : IReadOnlyCollection<T>
    {
        T this[int index]
        {
            get;
        }
    }
}
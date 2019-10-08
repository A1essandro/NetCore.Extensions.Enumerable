using System.Collections.Generic;

namespace Extensions.Enumerable
{
    public interface IAvoidingLargeObjectHeapCollection<T> : ICollection<T>
    {
        T this[int index]
        {
            get;
            set;
        }
    }
}
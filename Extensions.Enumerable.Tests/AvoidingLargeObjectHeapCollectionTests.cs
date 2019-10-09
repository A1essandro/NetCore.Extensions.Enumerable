using System;
using System.Collections.Generic;
using Extensions.Enumerable.Internal;
using Xunit;

namespace Extensions.Enumerable.Tests
{
    public class AvoidingLargeObjectHeapCollectionTests
    {

        private static IEnumerable<int> _getEnumerable(int size)
        {
            for (int i = 0; i < size; i++)
                yield return i;
        }

        [Theory(DisplayName = "AvoidingLargeObjectHeapCollection. Value by index.")]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(1024)]
        [InlineData(25024)]
        [InlineData(49999)]
        public void ValueByIndexTest(int index)
        {
            var collection = new AvoidingLargeObjectHeapCollection<int>(_getEnumerable(50000));

            Assert.Equal(index, collection[index]);
        }

        [Theory(DisplayName = "AvoidingLargeObjectHeapCollection. Value by index.")]
        [InlineData(-1)]
        [InlineData(50000)]
        public void IndexOutOfRangeExceptionTest(int index)
        {
            var collection = new AvoidingLargeObjectHeapCollection<int>(_getEnumerable(50000));

            Assert.Throws<IndexOutOfRangeException>(() => collection[index]);
        }

    }
}
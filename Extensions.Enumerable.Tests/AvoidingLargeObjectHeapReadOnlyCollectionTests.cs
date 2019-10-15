using Extensions.Enumerable.Internal.Collections;
using System;
using System.Collections.Generic;
using Xunit;

namespace Extensions.Enumerable.Tests
{
    public class AvoidingLargeObjectHeapReadOnlyCollectionTests
    {

        private static IEnumerable<int> _getEnumerable(int size)
        {
            for (int i = 0; i < size; i++)
                yield return i;
        }

        [Theory(DisplayName = "AvoidingLargeObjectHeapReadOnlyCollection. Count.")]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(1024)]
        [InlineData(25024)]
        [InlineData(49999)]
        public void CountTest(int count)
        {
            var collection = new AvoidingLargeObjectHeapReadOnlyCollection<int>(_getEnumerable(count));

            Assert.Equal(count, collection.Count);
        }

        [Theory(DisplayName = "AvoidingLargeObjectHeapReadOnlyCollection. Getting value by index.")]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(1024)]
        [InlineData(25024)]
        [InlineData(49999)]
        public void GetValueByIndexTest(int index)
        {
            var collection = new AvoidingLargeObjectHeapReadOnlyCollection<int>(_getEnumerable(50000));

            Assert.Equal(index, collection[index]);
        }

        [Theory(DisplayName = "AvoidingLargeObjectHeapReadOnlyCollection. Getting value by index out of range.")]
        [InlineData(-1)]
        [InlineData(50000)]
        [InlineData(55000)]
        [InlineData(-5000)]
        public void GettingIndexOutOfRangeExceptionTest(int index)
        {
            var collection = new AvoidingLargeObjectHeapReadOnlyCollection<int>(_getEnumerable(50000));

            Assert.Throws<IndexOutOfRangeException>(() => collection[index]);
        }

    }
}
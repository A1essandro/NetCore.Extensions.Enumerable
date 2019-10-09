using System;
using System.Collections.Generic;
using Extensions.Enumerable.Internal.Collections;
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

        [Theory(DisplayName = "AvoidingLargeObjectHeapCollection. Getting value by index.")]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(1024)]
        [InlineData(25024)]
        [InlineData(49999)]
        public void GetValueByIndexTest(int index)
        {
            var collection = new AvoidingLargeObjectHeapCollection<int>(_getEnumerable(50000));

            Assert.Equal(index, collection[index]);
        }

        [Theory(DisplayName = "AvoidingLargeObjectHeapCollection. Setting value by index.")]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(512)]
        [InlineData(25024)]
        [InlineData(49999)]
        public void SetValueByIndexTest(int index)
        {
            var collection = new AvoidingLargeObjectHeapCollection<int>(_getEnumerable(50000));

            collection[index] = index * 2;

            Assert.Equal(index * 2, collection[index]);
            if (index > 0)
                Assert.Equal(index - 1, collection[index - 1]);
        }

        [Theory(DisplayName = "AvoidingLargeObjectHeapCollection. Getting value by index out of range.")]
        [InlineData(-1)]
        [InlineData(50000)]
        public void GettingIndexOutOfRangeExceptionTest(int index)
        {
            var collection = new AvoidingLargeObjectHeapCollection<int>(_getEnumerable(50000));

            int result;
            Assert.Throws<IndexOutOfRangeException>(() => result = collection[index]);
        }

        [Theory(DisplayName = "AvoidingLargeObjectHeapCollection. Setting value by index out of range.")]
        [InlineData(-1)]
        [InlineData(50000)]
        public void SettingIndexOutOfRangeExceptionTest(int index)
        {
            var collection = new AvoidingLargeObjectHeapCollection<int>(_getEnumerable(50000));

            Assert.Throws<IndexOutOfRangeException>(() => collection[index] = 5);
        }

    }
}
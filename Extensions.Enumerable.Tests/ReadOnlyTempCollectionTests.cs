using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Extensions.Enumerable.Tests
{
    public class ReadOnlyTempCollectionTests
    {

        [Theory(DisplayName = "ReadOnlyTempCollection. Sum of int elements.")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        [InlineData(1024)]
        [InlineData(0)]
        public void SumTest(int size)
        {
            var arr = _getEnumerable(size).ToArray();
            using (var temp = _getEnumerable(size).ToTemp(size))
            {
                Assert.Equal(arr.Sum(), temp.Sum());
            }
        }

        [Theory(DisplayName = "ReadOnlyTempCollection. Size of collection.")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        [InlineData(1024)]
        [InlineData(0)]
        public void SizeTest(int size)
        {
            var arr = _getEnumerable(size).ToArray();
            using (var temp = _getEnumerable(size).ToTemp(size))
            {
                Assert.Equal(arr.Length, temp.Count);
            }
        }

        [Theory(DisplayName = "ReadOnlyTempCollection. Same with array.")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        [InlineData(1024)]
        [InlineData(0)]
        public void SameWithArrayTest(int size)
        {
            var arr = _getEnumerable(size).ToArray();
            using (var temp = _getEnumerable(size).ToTemp(size))
            {
                Assert.Equal(arr, temp);
            }
        }

        private IEnumerable<int> _getEnumerable(int size)
        {
            for (int i = 0; i < size; i++)
                yield return i;
        }

    }
}

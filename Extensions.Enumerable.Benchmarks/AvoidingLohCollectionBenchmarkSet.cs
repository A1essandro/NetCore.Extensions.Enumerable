using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Enumerable.Benchmarks
{

    [RankColumn, MemoryDiagnoser]
    [BenchmarkSet]
    public class AvoidingLohCollectionBenchmarkSet
    {

        [Params(8, 1024, 30_000, 1_000_000)]
        public int N = 0;

        [Benchmark]
        public double Array()
        {
            int[] array = _getEnumerable().ToArray();

            var nine = array.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
            var maxOdd = array.Where(x => x % 2 == 1).Select(x => (long)x).Max();

            return nine / maxOdd;
        }

        [Benchmark]
        public double List()
        {
            List<int> list = _getEnumerable().ToList();

            var nine = list.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
            var maxOdd = list.Where(x => x % 2 == 1).Select(x => (long)x).Max();

            return nine / maxOdd;
        }

        [Benchmark]
        public double AvoidingLoh()
        {
            IAvoidingLargeObjectHeapCollection<int> collection = _getEnumerable().ToAvoidingLohCollection();
            var nine = collection.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
            var maxOdd = collection.Where(x => x % 2 == 1).Select(x => (long)x).Max();

            return nine / maxOdd;
        }

        [Benchmark]
        public double AvoidingLohReadOnly()
        {
            IAvoidingLargeObjectHeapReadOnlyCollection<int> collection = _getEnumerable().ToAvoidingLohReadOnlyCollection();
            var nine = collection.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
            var maxOdd = collection.Where(x => x % 2 == 1).Select(x => (long)x).Max();

            return nine / maxOdd;
        }

        private IEnumerable<int> _getEnumerable()
        {
            for (int i = 0; i < N; i++)
                yield return i;
        }

    }
}
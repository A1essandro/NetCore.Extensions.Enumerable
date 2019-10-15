using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Extensions.Enumerable.Benchmarks
{

    [RankColumn, MemoryDiagnoser]
    [BenchmarkSet]
    public class AvoidingLohCollectionIndexAccessBenchmark
    {

        [Params(8, 1024, 30_000, 1_000_000)]
        public int N = 0;

        [Benchmark]
        public double Array()
        {
            int[] array = _getEnumerable().ToArray();

            var half = array[N / 2];
            var third = array[N / 3];

            return half / third;
        }

        [Benchmark]
        public double List()
        {
            List<int> list = _getEnumerable().ToList();

            var half = list[N / 2];
            var third = list[N / 3];

            return half / third;
        }

        [Benchmark]
        public double AvoidingLoh()
        {
            IAvoidingLargeObjectHeapCollection<int> collection = _getEnumerable().ToAvoidingLohCollection();

            var half = collection[N / 2];
            var third = collection[N / 3];

            return half / third;
        }

        [Benchmark]
        public double AvoidingLohReadOnly()
        {
            IAvoidingLargeObjectHeapReadOnlyCollection<int> collection = _getEnumerable().ToAvoidingLohReadOnlyCollection();

            var half = collection[N / 2];
            var third = collection[N / 3];

            return half / third;
        }

        private IEnumerable<int> _getEnumerable()
        {
            for (int i = 0; i < N; i++)
                yield return i;
        }

    }
}
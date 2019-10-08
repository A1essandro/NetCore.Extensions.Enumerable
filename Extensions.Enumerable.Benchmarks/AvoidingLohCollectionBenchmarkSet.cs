using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Enumerable.Benchmarks
{

    [RankColumn, MemoryDiagnoser]
    public class AvoidingLohCollectionBenchmarkSet
    {

        [Params(1024)]
        public int N = 0;

        [Benchmark]
        public double ArrayLinq()
        {
            int[] array = _getEnumerable().ToArray();

            var nine = array.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
            var maxOdd = array.Where(x => x % 2 == 1).Select(x => (long)x).Max();

            return nine / maxOdd;
        }

        [Benchmark]
        public double ListLinq()
        {
            List<int> list = _getEnumerable().ToList();

            var nine = list.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
            var maxOdd = list.Where(x => x % 2 == 1).Select(x => (long)x).Max();

            return nine / maxOdd;
        }

        [Benchmark]
        public double AvoidingLohLinq()
        {
            IAvoidingLargeObjectHeapCollection<int> collection = _getEnumerable().ToAvoidingLohCollection();
            var nine = collection.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
            var maxOdd = collection.Where(x => x % 2 == 1).Select(x => (long)x).Max();

            return nine / maxOdd;
        }

        [Benchmark]
        public double ArrayIndexer()
        {
            int[] array = _getEnumerable().ToArray();

            var half = array[N / 2];
            var third = array[N / 3];

            return half / third;
        }

        [Benchmark]
        public double ListIndexer()
        {
            List<int> list = _getEnumerable().ToList();

            var half = list[N / 2];
            var third = list[N / 3];

            return half / third;
        }

        [Benchmark]
        public double AvoidingLohIndexer()
        {
            IAvoidingLargeObjectHeapCollection<int> collection = _getEnumerable().ToAvoidingLohCollection();

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
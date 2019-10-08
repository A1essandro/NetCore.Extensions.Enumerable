using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Enumerable.Benchmarks
{

    [RankColumn, MemoryDiagnoser]
    public class TempCollectionBenchmarkSet
    {

        [Params(8, 1024, 32768)]
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
            List<int> array = _getEnumerable().ToList();

            var nine = array.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
            var maxOdd = array.Where(x => x % 2 == 1).Select(x => (long)x).Max();

            return nine / maxOdd;
        }

        [Benchmark]
        public double Temp()
        {
            using (ReadOnlyTempCollection<int> array = _getEnumerable().ToTemp(N * 2)) //with a margin
            {
                var nine = array.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
                var maxOdd = array.Where(x => x % 2 == 1).Select(x => (long)x).Max();

                return nine / maxOdd;
            }
        }

        private IEnumerable<int> _getEnumerable()
        {
            for (int i = 0; i < N; i++)
                yield return i;
        }

    }
}
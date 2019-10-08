using BenchmarkDotNet.Running;

namespace Extensions.Enumerable.Benchmarks
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<AvoidingLohCollectionBenchmarkSet>();
        }
    }
}

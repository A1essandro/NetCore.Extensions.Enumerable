using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Running;

namespace Extensions.Enumerable.Benchmarks
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            var sets = GetBenchmarkSets().ToArray();


            Console.WriteLine("Choose a benchmark set to run:");
            Console.WriteLine("==============================" + Environment.NewLine);
            for (int i = 0; i < sets.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {sets[i].Name}");
            }
            while (true)
            {
                Console.Write(Environment.NewLine + "Enter the number: ");
                if (int.TryParse(Console.ReadLine(), out int enter) && enter > 0 && enter <= sets.Length)
                {
                    BenchmarkRunner.Run(sets[enter - 1]);
                }
                else
                {
                    Console.WriteLine("Incorrect data. Try again");
                }
            }
        }

        private static IEnumerable<Type> GetBenchmarkSets()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetCustomAttributes(typeof(BenchmarkSetAttribute), true).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }
}

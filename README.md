# Extensions.Enumerable

## Methods

### ToTemp(maxSize)

#### Usage

```cs

using Extensions.Enumerable;

///....

var maxCollectionSize = 1000; //512 by default
using (ReadOnlyTempCollection<int> temp = _getEnumerable().ToTemp(maxCollectionSize))
{
    var nine = temp.Where(x => x % 9 == 0).Select(x => (long)x).Sum();
    var maxOdd = temp.Where(x => x % 2 == 1).Select(x => (long)x).Max();

    return nine / maxOdd;
}

```

#### Reason

Benchmarks:

```raw
BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Pentium CPU G4560 3.50GHz, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.2.402
  [Host]     : .NET Core 2.2.7 (CoreCLR 4.6.28008.02, CoreFX 4.6.28008.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.7 (CoreCLR 4.6.28008.02, CoreFX 4.6.28008.03), 64bit RyuJIT


| Method |     N |         Mean |         Error |        StdDev | Rank |   Gen 0 |   Gen 1 |   Gen 2 | Allocated |
|------- |------ |-------------:|--------------:|--------------:|-----:|--------:|--------:|--------:|----------:|
|  Array |     8 |     414.0 ns |     1.1605 ns |     0.9691 ns |    1 |  0.2184 |       - |       - |     344 B |
|   List |     8 |     419.2 ns |     0.5261 ns |     0.4663 ns |    2 |  0.3047 |       - |       - |     480 B |
|   Temp |     8 |     505.3 ns |     0.9449 ns |     0.7891 ns |    3 |  0.2794 |       - |       - |     440 B |
|  Array |  1024 |  17,121.7 ns |    53.9027 ns |    50.4206 ns |    4 |  5.6152 |       - |       - |    8840 B |
|   List |  1024 |  19,575.0 ns |    79.5335 ns |    66.4140 ns |    5 |  5.5542 |       - |       - |    8776 B |
|   Temp |  1024 |  26,779.2 ns |    24.1788 ns |    20.1904 ns |    6 |  0.2747 |       - |       - |     440 B |
|  Array | 32768 | 554,164.8 ns | 1,626.7688 ns | 1,442.0881 ns |    7 | 83.0078 | 41.0156 | 41.0156 |  263064 B |
|   List | 32768 | 651,968.4 ns | 1,328.2978 ns | 1,109.1888 ns |    8 | 83.0078 | 41.0156 | 41.0156 |  262848 B |
|   Temp | 32768 | 838,164.1 ns |   780.6017 ns |   691.9831 ns |    9 |       - |       - |       - |     440 B |

```

As you can see, usage of `ToTemp()` method leads to avoiding of allocation memory in heap. But you also can see some increase arithmetic mean time value of measurements. Best scenario of usage is methods which often called and use temporary collections or methods which use large temporary collections.

You can find benchmarks code in this repository.

## Contribute

Contributions to the package are always welcome!

## License

The code base is licensed under the MIT license.

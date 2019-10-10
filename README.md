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


| Method |       N |            Mean |          Error |         StdDev |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|------- |-------- |----------------:|---------------:|---------------:|----------:|----------:|----------:|----------:|
|  Array |       8 |        418.4 ns |      1.1076 ns |      1.0361 ns |    0.2184 |         - |         - |     344 B |
|   List |       8 |        422.6 ns |      0.4046 ns |      0.3587 ns |    0.3047 |         - |         - |     480 B |
|   Temp |       8 |        508.4 ns |      0.7835 ns |      0.6946 ns |    0.2794 |         - |         - |     440 B |
|  Array |    1024 |     17,178.5 ns |     44.5594 ns |     41.6808 ns |    5.6152 |         - |         - |    8840 B |
|   List |    1024 |     19,576.8 ns |     39.2385 ns |     34.7839 ns |    5.5542 |         - |         - |    8776 B |
|   Temp |    1024 |     26,508.3 ns |     29.9429 ns |     26.5436 ns |    0.2747 |         - |         - |     440 B |
|  Array |   32768 |    551,645.9 ns |    737.0543 ns |    689.4410 ns |   83.0078 |   41.0156 |   41.0156 |  263064 B |
|   List |   32768 |    649,464.1 ns |  1,019.6734 ns |    953.8031 ns |   83.0078 |   41.0156 |   41.0156 |  262848 B |
|   Temp |   32768 |    837,984.7 ns |  1,220.6802 ns |  1,019.3233 ns |         - |         - |         - |     440 B |
|  Array | 1048576 | 18,167,376.7 ns | 39,926.3002 ns | 35,393.6252 ns |  968.7500 |  968.7500 |  968.7500 | 8389648 B |
|   List | 1048576 | 22,116,527.1 ns | 44,357.2570 ns | 39,321.5529 ns | 1968.7500 | 1968.7500 | 1968.7500 | 8389432 B |
|   Temp | 1048576 | 27,368,904.1 ns | 57,683.4140 ns | 53,957.1013 ns |  500.0000 |  500.0000 |  500.0000 | 8389072 B |

```

As you can see, usage of `ToTemp()` method leads to avoiding of allocation memory in heap. But you also can see some increase arithmetic mean time value of measurements. Best scenario of usage is methods which often called and use temporary collections or methods which use large temporary collections.

### ToAvoidingLohCollection()

#### Usage

```cs

using Extensions.Enumerable;

///....

IAvoidingLargeObjectHeapCollection<int> collection = _getEnumerable().ToAvoidingLohCollection();

```

#### Reason

```raw

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Pentium CPU G4560 3.50GHz, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.2.402
  [Host]     : .NET Core 2.2.7 (CoreCLR 4.6.28008.02, CoreFX 4.6.28008.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.7 (CoreCLR 4.6.28008.02, CoreFX 4.6.28008.03), 64bit RyuJIT


|             Method |       N |            Mean |          Error |         StdDev |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|------------------- |-------- |----------------:|---------------:|---------------:|----------:|----------:|----------:|----------:|
|          ArrayLinq |       8 |        413.7 ns |      0.4241 ns |      0.3759 ns |    0.2184 |         - |         - |     344 B |
|           ListLinq |       8 |        438.7 ns |      0.9729 ns |      0.8624 ns |    0.3047 |         - |         - |     480 B |
|    AvoidingLohLinq |       8 |      3,532.3 ns |     20.1156 ns |     17.8319 ns |   39.9971 |         - |         - |   64240 B |
|          ArrayLinq |    1024 |     16,627.0 ns |     55.1884 ns |     43.0875 ns |    5.6152 |         - |         - |    8840 B |
|           ListLinq |    1024 |     19,905.5 ns |     35.0370 ns |     31.0594 ns |    5.5542 |         - |         - |    8776 B |
|    AvoidingLohLinq |    1024 |     38,716.8 ns |     69.9900 ns |     62.0443 ns |   39.9780 |         - |         - |   64240 B |
|          ArrayLinq |   30000 |    493,839.4 ns |    605.8608 ns |    537.0798 ns |   74.2188 |   36.1328 |   36.1328 |  251992 B |
|           ListLinq |   30000 |    610,980.4 ns |    902.9199 ns |    800.4149 ns |   83.0078 |   41.0156 |   41.0156 |  262848 B |
|    AvoidingLohLinq |   30000 |  1,022,097.9 ns |  1,206.7626 ns |  1,069.7637 ns |   58.5938 |   13.6719 |         - |  128048 B |
|          ArrayLinq | 1000000 | 15,913,629.5 ns | 25,950.5045 ns | 21,669.8466 ns |  781.2500 |  781.2500 |  781.2500 | 8195344 B |
|           ListLinq | 1000000 | 21,565,980.3 ns | 58,256.4989 ns | 54,493.1652 ns | 1968.7500 | 1968.7500 | 1968.7500 | 8389432 B |
|    AvoidingLohLinq | 1000000 | 29,679,061.3 ns | 35,419.5786 ns | 31,398.5339 ns |  656.2500 |  312.5000 |         - | 4019008 B |
|       ArrayIndexer |       8 |        167.6 ns |      0.3738 ns |      0.3314 ns |    0.0863 |         - |         - |     136 B |
|        ListIndexer |       8 |        147.5 ns |      0.4037 ns |      0.3579 ns |    0.1118 |         - |         - |     176 B |
| AvoidingLohIndexer |       8 |      2,807.8 ns |      4.7727 ns |      4.2309 ns |   39.9971 |         - |         - |   63920 B |
|       ArrayIndexer |    1024 |      7,391.5 ns |     24.9620 ns |     20.8444 ns |    5.4779 |         - |         - |    8632 B |
|        ListIndexer |    1024 |      7,387.1 ns |     13.8395 ns |     11.5566 ns |    5.3711 |         - |         - |    8472 B |
| AvoidingLohIndexer |    1024 |     12,926.6 ns |     22.8055 ns |     20.2165 ns |   39.9933 |         - |         - |   63920 B |
|       ArrayIndexer |   30000 |    233,966.7 ns |    327.6009 ns |    290.4097 ns |   73.9746 |   36.8652 |   36.8652 |  251784 B |
|        ListIndexer |   30000 |    256,562.1 ns |    536.6429 ns |    475.7200 ns |   83.0078 |   41.5039 |   41.5039 |  262544 B |
| AvoidingLohIndexer |   30000 |    317,913.9 ns |    473.6477 ns |    395.5172 ns |   59.0820 |   14.6484 |         - |  127728 B |
|       ArrayIndexer | 1000000 |  7,333,794.3 ns | 14,242.5368 ns | 11,893.1633 ns |  796.8750 |  796.8750 |  796.8750 | 8195136 B |
|        ListIndexer | 1000000 |  9,608,780.7 ns | 24,130.4352 ns | 20,150.0062 ns | 1984.3750 | 1984.3750 | 1984.3750 | 8389128 B |
| AvoidingLohIndexer | 1000000 | 10,789,838.1 ns | 14,154.0180 ns | 12,547.1683 ns |  640.6250 |  312.5000 |         - | 4018688 B |

```

## Benchmarks

You can find benchmarks code in this repository.

## Contribute

Contributions to the package are always welcome!

## License

The code base is licensed under the MIT license.

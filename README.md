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


|              Method |       N |            Mean |          Error |         StdDev |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|-------------------- |-------- |----------------:|---------------:|---------------:|----------:|----------:|----------:|----------:|
|               Array |       8 |        412.7 ns |      0.8442 ns |      0.7049 ns |    0.2184 |         - |         - |     344 B |
|                List |       8 |        431.0 ns |      0.7942 ns |      0.7429 ns |    0.3047 |         - |         - |     480 B |
|         AvoidingLoh |       8 |      3,489.5 ns |      9.0137 ns |      8.4314 ns |   39.9971 |         - |         - |   64240 B |
| AvoidingLohReadOnly |       8 |      3,393.1 ns |     11.8641 ns |     10.5172 ns |   39.9971 |         - |         - |   64296 B |
|               Array |    1024 |     17,303.9 ns |     20.7684 ns |     19.4267 ns |    5.6152 |         - |         - |    8840 B |
|                List |    1024 |     19,819.0 ns |     26.4550 ns |     23.4517 ns |    5.5542 |         - |         - |    8776 B |
|         AvoidingLoh |    1024 |     39,282.3 ns |     53.4893 ns |     41.7609 ns |   39.9780 |         - |         - |   64240 B |
| AvoidingLohReadOnly |    1024 |     33,077.8 ns |     56.3614 ns |     49.9629 ns |   39.9780 |         - |         - |   64296 B |
|               Array |   30000 |    518,600.3 ns |    952.1310 ns |    795.0725 ns |   74.2188 |   36.1328 |   36.1328 |  251992 B |
|                List |   30000 |    607,890.1 ns |    942.9098 ns |    881.9984 ns |   83.0078 |   41.0156 |   41.0156 |  262848 B |
|         AvoidingLoh |   30000 |  1,015,101.7 ns |  1,519.3250 ns |  1,268.7052 ns |   58.5938 |   13.6719 |         - |  128048 B |
| AvoidingLohReadOnly |   30000 |    886,359.3 ns |    600.2929 ns |    532.1441 ns |   58.5938 |   14.6484 |         - |  128104 B |
|               Array | 1000000 | 16,729,188.2 ns | 29,081.7673 ns | 27,203.1032 ns |  781.2500 |  781.2500 |  781.2500 | 8195344 B |
|                List | 1000000 | 21,401,478.8 ns | 31,835.3038 ns | 26,583.9205 ns | 1968.7500 | 1968.7500 | 1968.7500 | 8389432 B |
|         AvoidingLoh | 1000000 | 29,247,121.0 ns | 52,198.9998 ns | 48,826.9768 ns |  656.2500 |  312.5000 |         - | 4019008 B |
| AvoidingLohReadOnly | 1000000 | 29,595,137.6 ns | 60,157.8997 ns | 56,271.7367 ns |  625.0000 |  312.5000 |         - | 4019064 B |

```

## Benchmarks

You can find benchmarks code in this repository.

## Contribute

Contributions to the package are always welcome!

## License

The code base is licensed under the MIT license.

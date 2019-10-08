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

|             Method |     N |         Mean |     Error |    StdDev | Rank |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|------------------- |------ |-------------:|----------:|----------:|-----:|---------:|---------:|---------:|----------:|
|          ArrayLinq |  1024 |    16.695 us | 0.0373 us | 0.0331 us |    3 |   5.6152 |        - |        - |    8840 B |
|           ListLinq |  1024 |    19.957 us | 0.0405 us | 0.0359 us |    4 |   5.5542 |        - |        - |    8776 B |
|    AvoidingLohLinq |  1024 |           NA |        NA |        NA |    ? |        - |        - |        - |         - |
|       ArrayIndexer |  1024 |     7.404 us | 0.0194 us | 0.0181 us |    1 |   5.4779 |        - |        - |    8632 B |
|        ListIndexer |  1024 |     7.442 us | 0.0130 us | 0.0121 us |    1 |   5.3711 |        - |        - |    8472 B |
| AvoidingLohIndexer |  1024 |    13.266 us | 0.0398 us | 0.0372 us |    2 |  39.9933 |        - |        - |   63920 B |
|          ArrayLinq | 32768 |   535.091 us | 1.4197 us | 1.3280 us |   10 |  83.0078 |  41.0156 |  41.0156 |  263064 B |
|           ListLinq | 32768 |   671.357 us | 1.1992 us | 1.0014 us |   11 |  83.0078 |  41.0156 |  41.0156 |  262848 B |
|    AvoidingLohLinq | 32768 |   415.968 us | 1.5146 us | 1.3426 us |    8 |  71.7773 |  14.1602 |        - |  191872 B |
|       ArrayIndexer | 32768 |   257.362 us | 0.5983 us | 0.5597 us |    5 |  83.0078 |  41.5039 |  41.5039 |  262856 B |
|        ListIndexer | 32768 |   271.858 us | 0.7779 us | 0.6896 us |    6 |  83.0078 |  41.5039 |  41.5039 |  262544 B |
| AvoidingLohIndexer | 32768 |   355.341 us | 0.8865 us | 0.7859 us |    7 |  71.7773 |  14.1602 |        - |  191552 B |
|          ArrayLinq | 50000 |   846.879 us | 1.3636 us | 1.2756 us |   13 |  99.6094 |  99.6094 |  99.6094 |  463088 B |
|           ListLinq | 50000 | 1,061.849 us | 2.2693 us | 2.0117 us |   14 | 123.0469 | 123.0469 | 123.0469 |  525016 B |
|    AvoidingLohLinq | 50000 |   715.589 us | 1.7013 us | 1.4207 us |   12 |  69.3359 |  22.4609 |        - |  255640 B |
|       ArrayIndexer | 50000 |   417.664 us | 0.6829 us | 0.6388 us |    8 |  99.6094 |  99.6094 |  99.6094 |  462880 B |
|        ListIndexer | 50000 |   462.874 us | 1.0754 us | 0.8980 us |    9 | 124.5117 | 124.5117 | 124.5117 |  524712 B |
| AvoidingLohIndexer | 50000 |   536.970 us | 0.9142 us | 0.8104 us |   10 |  69.3359 |  22.4609 |        - |  255320 B |

```

## Benchmarks

You can find benchmarks code in this repository.

## Contribute

Contributions to the package are always welcome!

## License

The code base is licensed under the MIT license.

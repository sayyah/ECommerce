using BenchmarkDotNet.Attributes;
using ECommerce.Benchmark.RestAPIs;

namespace ECommerce.Benchmark.APIsBenchmarks;

[HtmlExporter]
public class ColorApiBenchmarks
{
    private readonly ColorsController _colorsController = new();

    [Params(100, 200)] public int IterationCount;

    [Benchmark]
    public async Task RestGetSmallPayloadAsync()
    {
        for (var i = 0; i < IterationCount; i++) await _colorsController.GetAllAsync();
    }
}
using BenchmarkDotNet.Running;
using MeasurementOffsetCalculation.Services;

Console.WriteLine("Hello i am measurement offset calculator.");
Console.WriteLine("Give me break points (order them from lower to higher) with reference values: (Press Q for stop adding breakpoints)");

Console.WriteLine("""
    Select mode:
        1.Your values
        2.Benchmark
    """);

string mod = Console.ReadLine();

if (mod == "1")
{
    List<ValueOffsetSetting> breakPointsOffsets = new List<ValueOffsetSetting>();

    do
    {
        string value = Console.ReadLine();

        if (value.ToUpper() == "Q")
            break;

        int castedValue = int.Parse(value);
        int referenceValue = int.Parse(Console.ReadLine());

        ValueOffsetSetting valueOffsetSetting = new(castedValue, referenceValue);
        breakPointsOffsets.Add(valueOffsetSetting);
    } while (true);

    Console.WriteLine("\n\nI am calculating linear equations for breakpoints\n");

    Console.WriteLine("Breakpoints:");

    foreach (var item in breakPointsOffsets)
        Console.WriteLine(item);


    Console.WriteLine("\nCalculate offsets:");

    OffsetPointsService offsetPointsService = new OffsetPointsService(new LinearFunctionCalculator());
    offsetPointsService.SetBreakPoints(breakPointsOffsets);
    Console.WriteLine("Give me x value (float) to calculate offset: (Press Q for stop calculating)");

    do
    {
        string value = Console.ReadLine();

        if (value.ToUpper() == "Q")
            break;

        if (float.TryParse(value, out float parsedValue))
            WriteAndCalculate(parsedValue);

    } while (true);

    Console.WriteLine("\nI am done with YOU");

    void WriteAndCalculate(float xValue)
    {
        Console.WriteLine($"Value {xValue} offset {offsetPointsService.CalculateValueExperimental(xValue)}");
    }
}
else if (mod == "2")
{
    //BenchmarkTest benchmarkTest = new BenchmarkTest();
    //benchmarkTest.Test1();
    //benchmarkTest.Test2();
    var summary = BenchmarkRunner.Run<BenchmarkTest>();
}


Console.WriteLine("Press anything to exit");
Console.ReadKey();

internal record ValueOffsetSetting(float Value, float ReferenceValue);
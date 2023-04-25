using MeasurementOffsetCalculation.Services;

Console.WriteLine("Hello i am measurement offset calculator.");
Console.WriteLine("Give me break points (order them from lower to higher) with reference values: (Press Q for stop adding breakpoints)");

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
//WriteAndCalculate(6.0f);
//WriteAndCalculate(6.4f);
//WriteAndCalculate(7.0f);
//WriteAndCalculate(8.7f);
//WriteAndCalculate(9.1f);
//WriteAndCalculate(10.0f);
//WriteAndCalculate(11.0f);
//WriteAndCalculate(12.0f);

Console.WriteLine("Give me x value (float) to calculate offset: (Press Q for stop calculating)");

do
{
    string value = Console.ReadLine();

    if (value.ToUpper() == "Q")
        break;

    float castedValue = float.Parse(value);
    WriteAndCalculate(castedValue);
} while (true);

Console.WriteLine("\nI am done with YOU");
Console.ReadKey();


void WriteAndCalculate(float xValue)
{
    Console.WriteLine($"Value {xValue} offset {offsetPointsService.CalculateValue(xValue)}");
}

internal record ValueOffsetSetting(float Value, float ReferenceValue);
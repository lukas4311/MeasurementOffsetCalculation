using BenchmarkDotNet.Attributes;

namespace MeasurementOffsetCalculation.Services
{
    public class BenchmarkTest
    {
        private OffsetPointsService offsetPointsService;
        private List<float> xValues = new List<float>();

        public BenchmarkTest()
        {
            this.offsetPointsService = new OffsetPointsService(new LinearFunctionCalculator());
            this.offsetPointsService.SetBreakPoints(new List<ValueOffsetSetting>() { new ValueOffsetSetting(5, 6), new ValueOffsetSetting(10, 12) });
            this.GeneratePointsToTest();
        }

        [Benchmark]
        public void CalculateValue()
        {
            foreach (var x in xValues)
            {
                offsetPointsService.CalculateValue(x);
            }
        }

        [Benchmark]
        public void CalculateValueExperimental()
        {
            foreach (var x in xValues)
            {
                offsetPointsService.CalculateValueExperimental(x);
            }
        }

        private void GeneratePointsToTest()
        {
            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 10000; i++)
            {
                xValues.Add((float)(random.NextDouble() * 23 - 3));
            }
        }
    }
}

using System.Collections.Immutable;

namespace MeasurementOffsetCalculation.Services
{
    internal class OffsetPointsService
    {
        private readonly LinearFunctionCalculator linearFunctionCalculator;
        private List<ValueOffsetSetting> breakPoints = new List<ValueOffsetSetting>();
        private List<CalculatedEquation> calculatedEquations = new();

        public OffsetPointsService(LinearFunctionCalculator linearFunctionCalculator)
        {
            this.linearFunctionCalculator = linearFunctionCalculator;
        }

        public void SetBreakPoints(IEnumerable<ValueOffsetSetting> breakPoints)
        {
            this.breakPoints = breakPoints.ToList();
            this.CalculateEquationFunctions();
        }

        public void WriteAllBreakpointsWithEquations()
        {
            Console.WriteLine("\n");
            foreach (var item in calculatedEquations)
            {
                Console.WriteLine(item);
            }
        }

        public float CalculateValue(float xValue)
        {
            if (calculatedEquations.Count == 0)
                return xValue;

            CalculatedEquation calculatedEquation = this.calculatedEquations.Where(w => w.valueOffset.Value < xValue).OrderByDescending(f => f.valueOffset.Value).FirstOrDefault() ?? this.calculatedEquations.First();
            return calculatedEquation.equation(xValue);
        }

        private void CalculateEquationFunctions()
        {
            for (int i = 0; i < this.breakPoints.Count() - 1; i++)
            {
                ValueOffsetSetting lowerPoint = this.breakPoints[i];
                ValueOffsetSetting higherPoint = this.breakPoints[i+1];
                float deltaForLowerPoint = lowerPoint.ReferenceValue - lowerPoint.Value;
                float deltaForHigherPoint = higherPoint.ReferenceValue - higherPoint.Value;
                (string equationString, Func<float, float> equationFunction) = this.linearFunctionCalculator.GetLinearFunctionEquation(new(lowerPoint.Value, deltaForLowerPoint), new(higherPoint.Value, deltaForHigherPoint));
                this.calculatedEquations.Add(new (lowerPoint, equationFunction));
            }
        }
    }

    internal record CalculatedEquation(ValueOffsetSetting valueOffset, Func<float, float> equation);
}

using System.Collections.Immutable;

namespace MeasurementOffsetCalculation.Services
{
    internal class OffsetPointsService
    {
        private readonly LinearFunctionCalculator linearFunctionCalculator;
        private List<ValueOffsetSetting> breakPoints = new List<ValueOffsetSetting>();
        private List<CalculatedEquation> calculatedEquations = new();
        private List<CalculatedEquationFilter> calculatedEquations2 = new();

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

        public float CalculateValueExperimental(float xValue)
        {
            if (calculatedEquations.Count == 0)
                return xValue;

            CalculatedEquationFilter calculatedEquation = this.calculatedEquations2.FirstOrDefault(x => x.shouldDo(xValue));
            return calculatedEquation.equation(xValue);
        }

        private void CalculateEquationFunctions()
        {
            if(this.breakPoints.Count() == 0)
            {
                Console.WriteLine("No breakpoint provided");
                return;
            }
                

            for (int i = 0; i < this.breakPoints.Count() - 1; i++)
            {
                ValueOffsetSetting lowerPoint = this.breakPoints[i];
                ValueOffsetSetting higherPoint = this.breakPoints[i+1];
                float deltaForLowerPoint = lowerPoint.ReferenceValue - lowerPoint.Value;
                float deltaForHigherPoint = higherPoint.ReferenceValue - higherPoint.Value;
                (string equationString, Func<float, float> equationFunction) = this.linearFunctionCalculator.GetLinearFunctionEquation(new(lowerPoint.Value, deltaForLowerPoint), new(higherPoint.Value, deltaForHigherPoint));
                this.calculatedEquations.Add(new (lowerPoint, equationFunction));
                this.calculatedEquations2.Add(new(x => x > lowerPoint.Value && x < higherPoint.Value, equationFunction));
            }

            this.calculatedEquations2.Add(new(_ => true, this.calculatedEquations2.First().equation));
        }
    }

    internal record CalculatedEquation(ValueOffsetSetting valueOffset, Func<float, float> equation);
    internal record CalculatedEquationFilter(Func<float, bool> shouldDo, Func<float, float> equation);
}

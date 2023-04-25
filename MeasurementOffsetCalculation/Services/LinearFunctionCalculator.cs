using System.Drawing;

namespace MeasurementOffsetCalculation.Services
{
    internal class LinearFunctionCalculator
    {
        public (string stringEquation, Func<float, float> equationFunction) GetLinearFunctionEquation(PointF point1, PointF point2)
        {
            float m = (point2.Y - point1.Y) / (point2.X - point1.X);
            float b = point1.Y - m * point1.X;
            Func<float, float> equation = (float x) => m * x + b;
            return ($"y = {m}x + {b}", equation);
        }
    }
}

using System;

using fob2d.Abstract;

namespace fob2d.NumericalPostProcessor
{
    internal class Result : IResult
    {
        public bool IsApproximated { get; private set; }
        public double LeftValue { get; private set; }
        public double RightValue { get; private set; }
        public double Difference { get; private set; }

        public Result(bool isApproximated = false, double leftValue = 0, double rightValue = 0)
        {
            this.IsApproximated = isApproximated;
            this.LeftValue = leftValue;

            if (IsApproximated)
            {
                this.RightValue = LeftValue;
                this.Difference = 0;
            }
            else
            {
                this.RightValue = rightValue;
                this.Difference = Math.Abs(this.LeftValue - this.RightValue);
            }
        }
    }
}

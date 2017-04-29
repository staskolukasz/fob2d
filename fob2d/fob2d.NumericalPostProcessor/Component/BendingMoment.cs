using System;
using System.Collections.Generic;
using System.Linq;

using fob2d.Abstract;
using fob2d.MatrixHelper;

namespace fob2d.NumericalPostProcessor
{
    internal static class BendingMoment 
    {
        public static IResult GetExactResult(IScheme scheme, IModel model, Dictionary<int, double[,]> elementBoundaryDisplacements, double x)
        {
            double leftValue;
            leftValue = GetResult(scheme, model, elementBoundaryDisplacements, x - 0.001);

            double rightValue;
            rightValue = GetResult(scheme, model, elementBoundaryDisplacements, x + 0.001);

            return new Result(false, leftValue, rightValue);
        }

        public static IResult GetApproximatedResult(IScheme scheme, IModel model, Dictionary<int, double[,]> elementBoundaryDisplacements, double x)
        {
            double leftValue;
            leftValue = GetResult(scheme, model, elementBoundaryDisplacements, x);

            return new Result(true, leftValue, leftValue);
        }

        public static double GetResult(IScheme scheme, IModel model, Dictionary<int, double[,]> elementBoundaryDisplacements, double x)
        {
            int startNodeNumber, endNodeNumber;
            ResultHelper.GetBoundaryNodes(scheme, model, x, out startNodeNumber, out endNodeNumber);

            int elementNumber = -1;

            if (startNodeNumber >= 1 && endNodeNumber >= 1)
            {
                elementNumber = ResultHelper.GetElementNumberContainingNodes(scheme, startNodeNumber, endNodeNumber);
            }

            if (elementNumber >= 1)
            {
                return GetAlongTheMemberResult(scheme, model, elementBoundaryDisplacements, x, startNodeNumber, endNodeNumber, elementNumber);
            }

            return 0;
        }

        private static double GetAlongTheMemberResult(IScheme scheme, IModel model, Dictionary<int, double[,]> elementBoundaryDisplacements, double x, int startNodeNumber, int endNodeNumber, int elementNumber)
        {
            IBeamProperty beamProperty = model.Beam.Properties.Where(beam => beam.Number == elementNumber).FirstOrDefault();
            double L;
            L = beamProperty.L;

            double s;
            s = ResultHelper.GetScalingParameter(scheme, x, elementNumber, startNodeNumber, endNodeNumber, L);

            double[,] currentShapeFunctionsVector = GetShapeFunctionsVector(s, L);

            double[,] result;
            result = MatrixTransformation.Multiply<double, double>(currentShapeFunctionsVector, elementBoundaryDisplacements[elementNumber]);

            double resultFactor;
            resultFactor = beamProperty.E * beamProperty.Iy / Math.Pow(beamProperty.L, 2);

            result = MatrixTransformation.Multiply(result, resultFactor);
            
            return result[0, 0];
        }

        private static double[,] GetShapeFunctionsVector(double s, double L)
        {
            double[,] currentShapeFunctionVector = new double[1, 4];
            currentShapeFunctionVector[0, 0] = -6 + 12 * s;
            currentShapeFunctionVector[0, 1] = L * (-4 + 6 * s);
            currentShapeFunctionVector[0, 2] = 6 - 12 * s;
            currentShapeFunctionVector[0, 3] = L * (-2 + 6 * s);

            return currentShapeFunctionVector;
        }
    }
}

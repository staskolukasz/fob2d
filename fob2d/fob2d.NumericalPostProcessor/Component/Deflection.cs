using System;
using System.Collections.Generic;
using System.Linq;

using fob2d.Abstract;
using fob2d.MatrixHelper;

namespace fob2d.NumericalPostProcessor
{
    internal static class Deflection
    {
        public static double GetExactResult(Dictionary<int, double[,]> elementBoundaryDisplacements, INode node)
        {
            double[,] currentNodeDisplacement = elementBoundaryDisplacements[node.Number];
            return currentNodeDisplacement[0,0];
        }

        public static double GetApproximatedResult(IScheme scheme, IModel model, Dictionary<int, double[,]> elementBoundaryDisplacements, double x)
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

            return result[0, 0];
        }
        
        private static double[,] GetShapeFunctionsVector(double s, double L)
        {
            double[,] currentShapeFunctionVector = new double[1, 4];
            currentShapeFunctionVector[0, 0] = 1 - 3 * Math.Pow(s, 2) + 2 * Math.Pow(s, 3);
            currentShapeFunctionVector[0, 1] = L * (s - 2 * Math.Pow(s, 2) + Math.Pow(s, 3));
            currentShapeFunctionVector[0, 2] = 3 * Math.Pow(s, 2) - 2 * Math.Pow(s, 3);
            currentShapeFunctionVector[0, 3] = L * (-1 * Math.Pow(s, 2) + Math.Pow(s, 3));

            return currentShapeFunctionVector;
        }
    }
}

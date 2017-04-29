using System;
using System.Linq;
using System.Collections.Generic;

using fob2d.Abstract;
using fob2d.MatrixHelper;

namespace fob2d.NumericalPostProcessor
{
    public class PostProcessor : IPostProcessor
    {
        internal IScheme scheme { get; private set; }
        internal IModel model { get; private set; }
        internal ISolver solver { get; private set; }
        public Dictionary<int, double[,]> ElementBoundaryForces { get; private set; }
        public Dictionary<int, double[,]> ElementBoundaryDisplacements { get; private set; }

        public Dictionary<int, double[,]> NodalDisplacements { get; private set; }

        public PostProcessor(IScheme scheme, IModel model, ISolver solver)
        {
            this.scheme = scheme;
            this.model = model;
            this.solver = solver;
        }

        public void CalculateResults()
        {
            ElementBoundaryForces = CalculateElementBoundaryForces();
            ElementBoundaryDisplacements = CalculateElementBoundaryDisplacements();
            NodalDisplacements = CalculateNodalDisplacements();
        }

        private Dictionary<int, double[,]> CalculateElementBoundaryForces()
        {
            Dictionary<int, double[,]> elementForces = new Dictionary<int, double[,]>();

            for (int i = 0; i < model.le; i++)
            {
                double[,] nodalForces = new double[4, 1];
                nodalForces = MatrixTransformation.Multiply(model.Beam.Matrix[i], model.Boolean.Matrix[i]);
                nodalForces = MatrixTransformation.Multiply(nodalForces, solver.U);

                elementForces.Add(i + 1, nodalForces);
            }

            return elementForces;
        }

        private Dictionary<int, double[,]> CalculateElementBoundaryDisplacements()
        {
            Dictionary<int, double[,]> elementDisplacements = new Dictionary<int, double[,]>();

            for (int i = 0; i < model.le; i++)
            {
                double[,] currentElementDisplacements = new double[4, 1];

                currentElementDisplacements = MatrixTransformation.Multiply(model.Boolean.Matrix[i], solver.U);

                elementDisplacements.Add(i + 1, currentElementDisplacements);
            }

            return elementDisplacements;
        }

        private Dictionary<int, double[,]> CalculateNodalDisplacements()
        {
            Dictionary<int, double[,]> nodalDisplacements = new Dictionary<int, double[,]>();

            for (int i = 0; i < scheme.Nodes.Count; i++)
            {
                double[,] NodeDisplacements = new double[2, 1];
                NodeDisplacements[0, 0] = solver.U[2 * i, 0];
                NodeDisplacements[1, 0] = solver.U[2 * i + 1, 0];

                nodalDisplacements.Add(i + 1, NodeDisplacements);
            }

            return nodalDisplacements;
        }


        public double GetDeflection(double x)
        {
            if (ResultHelper.ExactResultExists(scheme, x) > 0)
            {
                INode currentNode = scheme.Nodes.Where(node => node.X == x).FirstOrDefault();

                return Deflection.GetExactResult(NodalDisplacements, currentNode);
            }
            else
            {
                return Deflection.GetApproximatedResult(scheme, model, ElementBoundaryDisplacements, x);
            }
        }
        public Dictionary<double, double> GetDeflectionCollection(int numberOfPoints)
        {
            double totalLength = scheme.Nodes[scheme.Nodes.Count - 1].X;

            Dictionary<double, double> deflectionCollection = new Dictionary<double, double>();

            double result;

            for (int i = 1; i < numberOfPoints; i++)
            {
                double position = ((double)i / (double)numberOfPoints) * totalLength;
                result = GetDeflection(position);

                deflectionCollection.Add(Math.Round(position, 3), result);
            }

            result = GetDeflection(0);
            deflectionCollection.Add(0, result);

            result = GetDeflection(totalLength);
            deflectionCollection.Add(totalLength, result);

            List<INode> nodeList = scheme.Helper.GetNodesWithLoad();

            foreach (INode node in nodeList)
            {
                result = GetDeflection(node.X - 0.001);
                deflectionCollection.Add(Math.Round(node.X - 0.001, 3), result);

                result = GetDeflection(node.X + 0.001);
                deflectionCollection.Add(Math.Round(node.X + 0.001, 3), result);
            }


            IEnumerable<KeyValuePair<double, double>> sortedDeflectionCollection = deflectionCollection.OrderBy(i => i.Key);

            return sortedDeflectionCollection.ToDictionary(x => x.Key, x => x.Value);
        }

        public IResult GetShearForce(double x)
        {
            if (ResultHelper.ExactResultExists(scheme, x) > 0)
            {
                return ShearForce.GetExactResult(scheme, model, ElementBoundaryDisplacements, x);
            }
            else
            {
                return ShearForce.GetApproximatedResult(scheme, model, ElementBoundaryDisplacements, x);
            }
        }

        public Dictionary<double, double> GetShearForceCollection(int numberOfPoints)
        {
            double totalLength = scheme.Nodes[scheme.Nodes.Count - 1].X;

            Dictionary<double, double> shearForceCollection = new Dictionary<double, double>();

            IResult result;

            for (int i = 1; i < numberOfPoints; i++)
            {
                double position = ((double)i / (double)numberOfPoints) * totalLength;
                result = GetShearForce(position);

                shearForceCollection.Add(Math.Round(position, 3), result.LeftValue);
            }

            result = GetShearForce(0);
            shearForceCollection.Add(0, result.LeftValue);
            shearForceCollection.Add(0.001, result.RightValue);

            result = GetShearForce(totalLength);
            shearForceCollection.Add(totalLength - 0.001, result.LeftValue);
            shearForceCollection.Add(totalLength, result.RightValue);


            List<INode> nodeList = scheme.Helper.GetNodesWithLoad();

            foreach (INode node in nodeList)
            {
                result = GetShearForce(node.X);

                if (shearForceCollection.ContainsKey(node.X))
                {
                    shearForceCollection.Remove(node.X);
                }

                shearForceCollection.Add(node.X, 0);
                shearForceCollection.Add(node.X - 0.001, result.LeftValue);
                shearForceCollection.Add(node.X + 0.001, result.RightValue);
            }

            IEnumerable<KeyValuePair<double, double>> sortedShearForceCollection = shearForceCollection.OrderBy(i => i.Key);

            return sortedShearForceCollection.ToDictionary(x => x.Key, x => x.Value);
        }

        public IResult GetBendingMoment(double x)
        {
            if (ResultHelper.ExactResultExists(scheme, x) > 0)
            {
                return BendingMoment.GetExactResult(scheme, model, ElementBoundaryDisplacements, x);
            }
            else
            {
                return BendingMoment.GetApproximatedResult(scheme, model, ElementBoundaryDisplacements, x);
            }
        }

        public Dictionary<double, double> GetBendingMomentCollection(int numberOfPoints)
        {
            double totalLength = scheme.Nodes[scheme.Nodes.Count - 1].X;

            Dictionary<double, double> bendingMomentCollection = new Dictionary<double, double>();

            IResult result;

            for (int i = 1; i < numberOfPoints; i++)
            {
                double position = ((double)i / (double)numberOfPoints) * totalLength;
                result = GetBendingMoment(position);

                bendingMomentCollection.Add(Math.Round(position, 3), result.LeftValue);
            }

            result = GetBendingMoment(0);
            bendingMomentCollection.Add(0, result.LeftValue);

            result = GetShearForce(totalLength);
            bendingMomentCollection.Add(totalLength, result.RightValue);

            List<INode> nodeList = scheme.Helper.GetNodesWithLoad();

            foreach (INode node in nodeList)
            {
                result = GetBendingMoment(node.X - 0.001);
                bendingMomentCollection.Add(Math.Round(node.X - 0.001, 3), result.LeftValue);

                result = GetBendingMoment(node.X + 0.001);
                bendingMomentCollection.Add(Math.Round(node.X + 0.001, 3), result.LeftValue);
            }

            IEnumerable<KeyValuePair<double, double>> sortedBendingMomentCollection = bendingMomentCollection.OrderBy(i => i.Key);

            return sortedBendingMomentCollection.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}

using System.Linq;

using fob2d.Abstract;

namespace fob2d.NumericalPostProcessor
{
    internal static class ResultHelper
    {
        public static int ExactResultExists(IScheme scheme, double x)
        {
            INode currentNode = scheme.Nodes.Where(node => node.X == x).FirstOrDefault();

            if (currentNode != null)
            {
                return currentNode.Number > 0 ? currentNode.Number : -1;
            }

            return -1;
        }

        public static void GetBoundaryNodes(IScheme scheme, IModel model, double x, out int startNodeNumber, out int endNodeNumber)
        {
            startNodeNumber = -1;
            endNodeNumber = -1;

            for (int i = 0; i < model.ln; i++)
            {
                INode currentNode = scheme.Nodes[i];
                if (currentNode.X > x)
                {
                    endNodeNumber = currentNode.Number;
                    startNodeNumber = currentNode.Number - 1;
                    break;
                }
            }
        }

        public static int GetElementNumberContainingNodes(IScheme scheme, int startNodeNumber, int endNodeNumber)
        {
            IElement currentElement = scheme.Elements
                .Where(element => (element.StartNodeNumber == startNodeNumber && element.EndNodeNumber == endNodeNumber))
                .FirstOrDefault();

            return currentElement.Number > 0 ? currentElement.Number : -1;
        }

        public static double GetScalingParameter(IScheme scheme, double x, int elementNumber, int startNodeNumber, int endNodeNumber, double L)
        {
            INode currentStartNode = scheme.Nodes.Where(node => node.Number == startNodeNumber).FirstOrDefault();
            return (x - currentStartNode.X) / L;
        }
    }
}

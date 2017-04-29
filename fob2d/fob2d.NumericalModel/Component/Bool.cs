using System.Collections.Generic;

using fob2d.Abstract;
using fob2d.MatrixHelper;

namespace fob2d.NumericalModel
{
    internal class Bool : IComponentBuilder<Bool>, IBool
    {
        private Model model;

        public List<int[,]> Matrix { get; private set; }
        public List<int[,]> TransposedMatrix { get; private set; }

        public Bool(Model model)
        {
            this.model = model;

            Matrix = new List<int[,]>();
            TransposedMatrix = new List<int[,]>();
        }

        public void SetComponent()
        {
            for (int i = 0; i < model.le; i++)
            {
                int[,] currentBooleanMatrix = CreateBooleanMatrix(model.Topology.Matrix[i]);
                Matrix.Add(currentBooleanMatrix);
            }
        }

        public void SetDedicatedData()
        {
            for (int i = 0; i < model.le; i++)
            {
                TransposedMatrix.Add(MatrixTransformation.Transpose<int, int>(Matrix[i]));
            }
        }

        public Bool Get()
        {
            return this;
        }

        private int[,] CreateBooleanMatrix(int[] currentTopologyMatrix)
        {
            int[,] currentBoolean = new int[model.LW * model.LQ, model.ln * model.LQ];

            for (int x = 1; x <= 2; x++)
            {
                currentBoolean[x - 1, (2 * (currentTopologyMatrix[0] - 1) + x) - 1] = 1;
                currentBoolean[(x + 2) - 1, (2 * (currentTopologyMatrix[1] - 1) + x) - 1] = 1;
            }

            return currentBoolean;
        }
    }
}

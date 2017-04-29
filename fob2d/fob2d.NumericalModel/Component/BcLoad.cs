using fob2d.Abstract;

namespace fob2d.NumericalModel
{
    internal class BcLoad : IComponentBuilder<BcLoad>, IBcLoad
    {
        private Model model;

        public double[,] Matrix { get; private set; }

        public BcLoad(Model model)
        {
            this.model = model;
        }

        public void SetComponent()
        {
            Matrix = new double[model.ln * model.LQ, 1];

            foreach (ILoad currentLoad in model.scheme.Loads)
            {
                Matrix = ApplyNodalLoad(Matrix, currentLoad);
            }
        }

        public void SetDedicatedData() { }

        public BcLoad Get()
        {
            return this;
        }

        private double[,] ApplyNodalLoad(double[,] matrix, ILoad currentLoad)
        {
            int nodeNumber = currentLoad.NodeNumber;
            int loadDoF_1 = nodeNumber * model.LQ - 2;
            int loadDoF_2 = nodeNumber * model.LQ - 1;
            
            matrix[loadDoF_1, 0] = currentLoad.Pz;
            matrix[loadDoF_2, 0] = currentLoad.My;
           
            return matrix;
        }
    }
}

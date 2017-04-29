using fob2d.Abstract;

namespace fob2d.NumericalModel
{
    internal class BcSupport : IComponentBuilder<BcSupport>, IBcSupport
    {
        private Model model;

        public int[,] Matrix { get; private set; }

        public BcSupport(Model model)
        {
            this.model = model;
        }

        public void SetComponent()
        {
            Matrix = new int[model.ln * model.LQ, 1];
            
            Matrix[0, 0] = 1;
            Matrix[Matrix.GetUpperBound(0) - 1, 0] = 1;
        }

        public void SetDedicatedData() { }

        public BcSupport Get()
        {
            return this;
        }
    }
}

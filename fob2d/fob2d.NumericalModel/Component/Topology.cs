using System.Collections.Generic;
using fob2d.Abstract;

namespace fob2d.NumericalModel
{
    internal class Topology: IComponentBuilder<Topology>, ITopology
    {
        private Model model;
        public List<int[]> Matrix { get; private set; }

        public Topology(Model model)
        {
            this.model = model;

            Matrix = new List<int[]>();
        }

        public void SetComponent()
        {
            foreach(IElement currentElement in model.scheme.Elements)
            {
                Matrix.Add(new int[] { currentElement.StartNodeNumber, currentElement.EndNodeNumber });
            }
        }

        public void SetDedicatedData() { }

        public Topology Get()
        {
            return this;
        }
    }
}

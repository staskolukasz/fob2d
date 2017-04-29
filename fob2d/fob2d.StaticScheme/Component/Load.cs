using fob2d.Abstract;

namespace fob2d.StaticScheme.Component
{
    internal class Load : ComponentBase, ILoad
    {
        public int NodeNumber { get; set; }
        public double Pz { get; set; }
        public double My { get; set; }
    }
}

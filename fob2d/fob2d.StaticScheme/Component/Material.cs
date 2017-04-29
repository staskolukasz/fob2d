using fob2d.Abstract;

namespace fob2d.StaticScheme.Component
{
    internal class Material : ComponentBase, IMaterial
    {
        public double E { get; set; }
        public double v { get; set; }
    }
}

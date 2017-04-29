using fob2d.Abstract;

namespace fob2d.StaticScheme.Component
{
    internal class CrossSection : ComponentBase, ICrossSection
    {
        public double Iy { get; set; }
    }
}

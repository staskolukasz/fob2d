using fob2d.Abstract;

namespace fob2d.NumericalModel
{
    internal class BeamProperty : IBeamProperty
    {
        public double Number { get; private set; }
        public double E { get; private set; }
        public double Iy { get; private set; }
        public double L { get; private set; }

        public BeamProperty(int number, double E, double Iy, double L)
        {
            this.Number = number;
            this.E = E;
            this.Iy = Iy;
            this.L = L;
        }
    }
}

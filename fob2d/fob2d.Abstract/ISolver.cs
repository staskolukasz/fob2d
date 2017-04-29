using System.Collections.Generic;

namespace fob2d.Abstract
{
    public interface ISolver
    {
        double[,] K { get; }
        double[,] KK { get; }
        double[,] F { get; }

        double[,] U { get; }
        double[,] R { get; }

        void AssemblyFiniteElementModel();
        void Run();
    }
}

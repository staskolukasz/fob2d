using System.Collections.Generic;

namespace fob2d.Abstract
{
    public interface IBool
    {
        List<int[,]> Matrix { get; }
        List<int[,]> TransposedMatrix { get; }
    }
}

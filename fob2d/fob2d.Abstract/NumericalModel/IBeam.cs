using System.Collections.Generic;

namespace fob2d.Abstract
{
    public interface IBeam
    {
        List<double[,]> Matrix { get; }
        List<IBeamProperty> Properties { get; }
    }
}

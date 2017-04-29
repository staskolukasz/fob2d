using System.Collections.Generic;

namespace fob2d.Abstract
{
    public interface IScheme
    {
        List<IMaterial> Materials { get; }
        List<ICrossSection> CrossSections { get; }
        List<INode> Nodes { get; }
        List<IElement> Elements { get; }
        List<ILoad> Loads { get; }

        ISchemeHelper Helper { get; }

        IMaterial NewMaterial();
        ICrossSection NewCrossSection();
        INode NewNode();
        IElement NewElement();
        ILoad NewLoad();
    }
}

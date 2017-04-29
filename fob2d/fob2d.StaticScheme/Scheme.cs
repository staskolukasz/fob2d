using System.Collections.Generic;

using fob2d.Abstract;
using fob2d.StaticScheme.Component;

namespace fob2d.StaticScheme
{
    public class Scheme : IScheme
    {
        public List<IMaterial> Materials { get; private set; }
        public List<ICrossSection> CrossSections { get; private set; }
        public List<INode> Nodes { get; private set; }
        public List<IElement> Elements { get; private set; }
        public List<ILoad> Loads { get; private set; }

        public ISchemeHelper Helper { get; private set; }

        public Scheme()
        {
            Materials = new List<IMaterial>();
            CrossSections = new List<ICrossSection>();
            Nodes = new List<INode>();
            Elements = new List<IElement>();
            Loads = new List<ILoad>();

            Helper = new Helper(this);
        }

        public IMaterial NewMaterial()
        {
            return new Material();
        }

        public ICrossSection NewCrossSection()
        {
            return new CrossSection();
        }

        public INode NewNode()
        {
            return new Node();
        }

        public IElement NewElement()
        {
            return new Element();
        }

        public ILoad NewLoad()
        {
            return new Load();
        }
    }
}

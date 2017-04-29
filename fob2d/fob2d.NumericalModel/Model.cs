using System.Linq;
using fob2d.Abstract;

namespace fob2d.NumericalModel
{
    public class Model : IModel
    {
        // Total number of nodes in one finite element
        public int LW { get; private set; }
        // Total number of degrees of freedom in one node of the finite element
        public int LQ { get; private set; }
        // The total number of nodes in model
        public int ln { get; private set; } 
        // The total number of elements in model
        public int le { get; private set; }

        internal IScheme scheme { get; private set; }

        public ITopology Topology { get; private set; }
        public IBool Boolean { get; private set; }
        public IBeam Beam { get; private set; }
        public IBcSupport Support { get; private set; }
        public IBcLoad Load { get; private set; }
        public IIdentity Identity { get; private set; }


        public Model(IScheme scheme)
        {
            this.scheme = scheme;

            this.LW = 2;
            this.LQ = 2;
            ln = scheme.Nodes.Count();
            le = scheme.Elements.Count();
        }

        public void AssembyModel()
        {
            var TopologyCreator = new ComponentBuilder<Topology>(new Topology(this));
            TopologyCreator.Set();
            Topology = TopologyCreator.Get();

            var BoolCreator = new ComponentBuilder<Bool>(new Bool(this));
            BoolCreator.Set();
            Boolean = BoolCreator.Get();

            var BeamCreator = new ComponentBuilder<Beam>(new Beam(this));
            BeamCreator.Set();
            Beam = BeamCreator.Get();

            var SupportCreator = new ComponentBuilder<BcSupport>(new BcSupport(this));
            SupportCreator.Set();
            Support = SupportCreator.Get();

            var LoadCreator = new ComponentBuilder<BcLoad>(new BcLoad(this));
            LoadCreator.Set();
            Load = LoadCreator.Get();

            var IdentityCreator = new ComponentBuilder<Identity>(new Identity(this));
            IdentityCreator.Set();
            Identity = IdentityCreator.Get();
        }

    }
}

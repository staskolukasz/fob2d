namespace fob2d.Abstract
{
    public interface IModel
    {
        int LW { get; }
        int LQ { get; }
        int ln { get; }
        int le { get; }

        ITopology Topology { get; }
        IBool Boolean { get; }
        IBeam Beam { get; }
        IBcSupport Support { get; }
        IBcLoad Load { get; }
        IIdentity Identity { get; }

        void AssembyModel();
    }
}

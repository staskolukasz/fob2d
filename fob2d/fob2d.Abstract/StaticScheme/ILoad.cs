namespace fob2d.Abstract
{
    public interface ILoad : IComponentBase
    {
        int NodeNumber { get; set; }
        double Pz { get; set; }
        double My { get; set; }
    }
}

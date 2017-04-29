namespace fob2d.Abstract
{
    public interface IElement : IComponentBase
    {
        int MaterialNumber { get; set; }
        int CrossSectionNumber { get; set; }
        int StartNodeNumber { get; set; }
        int EndNodeNumber { get; set; }
    }
}

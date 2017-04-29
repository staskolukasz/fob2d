namespace fob2d.Abstract
{
    public interface IResult
    {
        bool IsApproximated { get; }
        double LeftValue { get; }
        double RightValue { get; }
        double Difference { get; }
    }
}

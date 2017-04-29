using System.Collections.Generic;

namespace fob2d.Abstract
{
    public interface IPostProcessor
    {
        Dictionary<int, double[,]> ElementBoundaryForces { get; }
        Dictionary<int, double[,]> ElementBoundaryDisplacements { get; }
        Dictionary<int, double[,]> NodalDisplacements { get; }
        void CalculateResults();
        double GetDeflection(double x);
        Dictionary<double, double> GetDeflectionCollection(int numberOfPoints);
        IResult GetShearForce(double x);
        Dictionary<double, double> GetShearForceCollection(int numberOfPoints);
        IResult GetBendingMoment(double x);
        Dictionary<double, double> GetBendingMomentCollection(int numberOfPoints);
    }
}

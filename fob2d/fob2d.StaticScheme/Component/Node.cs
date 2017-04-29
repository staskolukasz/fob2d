using fob2d.Abstract;

namespace fob2d.StaticScheme.Component
{
    internal class Node : ComponentBase, INode
    {
        public double X
        {
            get
            {
                return Coordinate.X;
            }

            set
            {
                Coordinate = new Coordinates(value, Coordinate.Y);
            }
        }

        public double Y
        {
            get
            {
                return Coordinate.Y;
            }

            set
            {
                Coordinate = new Coordinates(Coordinate.X, value);
            }
        }

        internal Coordinates Coordinate { get; set; }
        internal DoFs DoF { get; set; }

        public Node()
        {
            Coordinate = new Coordinates();
            DoF = new DoFs();
        }

        public override void UpdateProperty()
        {
            DoF = new DoFs(base.Number);
        }
    }

    internal struct Coordinates
    {
        public double X;
        public double Y;

        public Coordinates(double x = 0, double y = 0)
        {
            X = x;
            Y = y;
        }
    }

    internal struct DoFs
    {
        public int nodeDisplacement;
        public int nodeRotation;

        public DoFs(int number)
        {
            nodeDisplacement = 2 * number - 1;
            nodeRotation = 2 * number;
        }
    }
}

using fob2d.Abstract;

namespace fob2d.StaticScheme.Component
{
    internal class Element : ComponentBase, IElement
    {
        public int MaterialNumber { get; set; }
        public int CrossSectionNumber { get; set; }
        public int StartNodeNumber { get; set; }
        public int EndNodeNumber { get; set; }
    }
}

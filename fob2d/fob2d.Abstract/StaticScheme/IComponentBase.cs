namespace fob2d.Abstract
{
    public interface IComponentBase
    {
        int Number { get; set; }

        string Name { get; set; }

        void UpdateProperty();
    }
}

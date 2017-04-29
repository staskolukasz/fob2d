namespace fob2d.Abstract
{
    public interface IComponentBuilder<T>
    {
        void SetComponent();
        void SetDedicatedData();
        T Get();
    }
}

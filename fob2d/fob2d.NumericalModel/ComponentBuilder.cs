using fob2d.Abstract;

namespace fob2d.NumericalModel
{
    internal class ComponentBuilder<T>
    {
        private IComponentBuilder<T> component;

        public ComponentBuilder(IComponentBuilder<T> component)
        {
            this.component = component;
        }

        public void Set()
        {
            component.SetComponent();
            component.SetDedicatedData();
        }

        public T Get()
        {
            return (T)(object)component.Get();
        }
    }
}

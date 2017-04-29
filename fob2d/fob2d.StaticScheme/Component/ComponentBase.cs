namespace fob2d.StaticScheme.Component
{
    internal abstract class ComponentBase
    {
        private int number;
        public string name;

        public int Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
                this.UpdateProperty();
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public virtual void UpdateProperty()
        {

        }
    }
}

using fob2d.Abstract;
using fob2d.MatrixHelper;

namespace fob2d.NumericalModel
{
    public class Identity : IComponentBuilder<Identity>, IIdentity
    {
        private Model model;

        public int[,] Matrix { get; private set; }
        public int[,] Id { get; private set; }
        public int[,] Ip { get; private set; }

        public Identity(Model model)
        {
            this.model = model;
        }

        public void SetComponent()
        {
            Matrix = new int[model.LQ * model.ln, model.LQ * model.ln];
            MatrixTransformation.SetIdentityMatrix(Matrix);
        }

        public void SetDedicatedData()
        {
            Id = new int[model.LQ * model.ln, model.LQ * model.ln];
            MatrixTransformation.SetOnMatrixDiagonal(Id, model.Support.Matrix);

            Ip = new int[model.LQ * model.ln, model.LQ * model.ln];
            Ip = MatrixTransformation.SubstractOnMatrixDiagonal(Matrix, Id);
        }

        public Identity Get()
        {
            return this;
        }
    }
}

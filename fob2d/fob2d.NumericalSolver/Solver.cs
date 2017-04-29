using fob2d.Abstract;
using fob2d.MatrixHelper;

namespace fob2d.NumericalSolver
{
    public class Solver : ISolver
    {
        internal IModel model { get; private set; }

        // Global stiffness matrix
        public double[,] K { get; private set; }

        // Global stiffness matrix with applied boundary conditions
        public double[,] KK { get; private set; }

        // F matrix
        public double[,] F { get; private set; }

        // Results - displacement matrix
        public double[,] U { get; private set; }

        // Results - reactions matrix
        public double[,] R { get; private set; }

        // Helper fields
        private double[,] equation;
        private double[][] rows;

        public Solver(IModel model)
        {
            this.model = model;
        }

        public void AssemblyFiniteElementModel()
        {
            AggregateGlobalStiffnessMatrix();
            ApplyBCs();
            SetSubstitutiveMatrix();
            DetermineParticularEquations();
        }

        public void Run()
        {
            U = Solve(rows);
            R = CalculateReactions();
        }

        private void AggregateGlobalStiffnessMatrix()
        {
            K = new double[model.LQ * model.ln, model.LQ * model.ln];

            for (int i = 0; i < model.le; i++)
            {
                double[,] k = new double[4, 4];
                k = MatrixTransformation.Multiply<int, double>(model.Boolean.TransposedMatrix[i], model.Beam.Matrix[i]);
                k = MatrixTransformation.Multiply<double, int>(k, model.Boolean.Matrix[i]);

                K = MatrixTransformation.Sum(K, k);
            }
        }

        private void ApplyBCs()
        {
            KK = new double[model.LQ * model.ln, model.LQ * model.ln];

            KK = MatrixTransformation.Multiply<int, double>(model.Identity.Ip, K);
            KK = MatrixTransformation.Multiply<double, int>(KK, model.Identity.Ip);
            KK = MatrixTransformation.Sum(KK, MatrixTransformation.ToDouble<int>(model.Identity.Id));

            F = new double[model.LQ * model.ln, 1];
            F = MatrixTransformation.Multiply<int, double>(model.Identity.Ip, model.Load.Matrix);
        }

        private void SetSubstitutiveMatrix()
        {
            equation = new double[model.LQ * model.ln, model.LQ * model.ln + 1];
            equation = MatrixTransformation.Sum(equation, KK);

            for(int i = 0; i < equation.GetLength(0); i++)
            {
                equation[i, equation.GetLength(0)] = F[i, 0];
            }
        }

        private void DetermineParticularEquations()
        {
            int rowsCount = model.LQ * model.ln;

            double[][] rows = new double[rowsCount][];

            for (int i = 0; i < rowsCount; i++)
            {
                double[] currentRow = new double[rowsCount + 1];
                for(int y = 0; y < currentRow.GetLength(0); y++)
                {
                    currentRow[y] = equation[i, y];
                }

                rows[i] = currentRow;
            }

            this.rows = rows;
        }

        private double[,] Solve(double[][] rows)
        {
            int length = rows[0].Length;

            for (int i = 0; i < rows.Length - 1; i++)
            {
                if (rows[i][i] == 0 && !Sweep(rows, i, i))
                {
                    return null;
                }

                for (int j = i; j < rows.Length; j++)
                {
                    double[] d = new double[length];
                    for (int x = 0; x < length; x++)
                    {
                        d[x] = rows[j][x];
                        if (rows[j][i] != 0)
                        {
                            d[x] = d[x] / rows[j][i];
                        }
                    }
                    rows[j] = d;
                }

                for (int y = i + 1; y < rows.Length; y++)
                {
                    double[] f = new double[length];
                    for (int g = 0; g < length; g++)
                    {
                        f[g] = rows[y][g];
                        if (rows[y][i] != 0)
                        {
                            f[g] = f[g] - rows[i][g];
                        }

                    }
                    rows[y] = f;
                }
            }

            return GetResult(rows);
        }

        private bool Sweep(double[][] rows, int row, int column)
        {
            bool changed = false;

            for (int z = rows.Length - 1; z > row; z--)
            {
                if (rows[z][row] != 0)
                {
                    double[] temp = new double[rows[0].Length];
                    temp = rows[z];
                    rows[z] = rows[column];
                    rows[column] = temp;
                    changed = true;
                }
            }

            return changed;
        }

        private double[,] GetResult(double[][] rows)
        {
            double val = 0;
            int length = rows[0].Length;
            double[,] result = new double[rows.Length,1];

            for (int i = rows.Length - 1; i >= 0; i--)
            {
                val = rows[i][length - 1];
                for (int x = length - 2; x > i - 1; x--)
                {
                    val -= rows[i][x] * result[x, 0];
                }
                result[i, 0] = val / rows[i][i];
            }

            return result;
        }

        private double[,] CalculateReactions()
        {
            double[,] currentMatrix = new double[model.LQ * model.ln, 1];

            currentMatrix = MatrixTransformation.Multiply<double, double>(K, U);
            currentMatrix = MatrixTransformation.Substract(currentMatrix, model.Load.Matrix);

            return currentMatrix;
        }
    }
}

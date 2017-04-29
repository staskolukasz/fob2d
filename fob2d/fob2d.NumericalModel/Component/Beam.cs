using System;
using System.Linq;
using System.Collections.Generic;
using fob2d.Abstract;

namespace fob2d.NumericalModel
{
    internal class Beam : IComponentBuilder<Beam>, IBeam
    {
        private Model model;

        public List<double[,]> Matrix { get; private set; }
        public List<IBeamProperty> Properties { get; set; }

        public Beam(Model model)
        {
            this.model = model;

            Matrix = new List<double[,]>();
            Properties = new List<IBeamProperty>();
        }

        public void SetComponent()
        {
            for (int i = 0; i < model.le; i++)
            {
                IElement currentElement;
                currentElement = model.scheme.Elements[i];
                
                double E = GetCurrentE(currentElement.MaterialNumber);
                double Iy = GetCurrentIy(currentElement.CrossSectionNumber);
                double L = GetCurrentL(currentElement.StartNodeNumber, currentElement.EndNodeNumber);

                IBeamProperty beamProperty = new BeamProperty(currentElement.Number, E, Iy, L);
                Properties.Add(beamProperty);

                double[,] elementStiffnessMatrix = CalculateElementStiffnessMatrix(E, Iy, L);
                Matrix.Add(elementStiffnessMatrix);
            }
        }

        public void SetDedicatedData() { }

        public Beam Get()
        {
            return this;
        }

        private double[,] CalculateElementStiffnessMatrix(double E, double Iy, double L)
        {
            double[,] currentMatrix = new double[4, 4];

            currentMatrix[0, 0] = 12 * E * Iy / Math.Pow(L, 3);
            currentMatrix[0, 1] = 6 * E * Iy / Math.Pow(L, 2);
            currentMatrix[0, 2] = -12 * E * Iy / Math.Pow(L, 3);
            currentMatrix[0, 3] = 6 * E * Iy / Math.Pow(L, 2);

            currentMatrix[1, 0] = 6 * E * Iy / Math.Pow(L, 2);
            currentMatrix[1, 1] = 4 * E * Iy / Math.Pow(L, 1);
            currentMatrix[1, 2] = -6 * E * Iy / Math.Pow(L, 2);
            currentMatrix[1, 3] = 2 * E * Iy / Math.Pow(L, 1);

            currentMatrix[2, 0] = -12 * E * Iy / Math.Pow(L, 3);
            currentMatrix[2, 1] = -6 * E * Iy / Math.Pow(L, 2);
            currentMatrix[2, 2] = 12 * E * Iy / Math.Pow(L, 3);
            currentMatrix[2, 3] = -6 * E * Iy / Math.Pow(L, 2);

            currentMatrix[3, 0] = 6 * E * Iy / Math.Pow(L, 2);
            currentMatrix[3, 1] = 2 * E * Iy / Math.Pow(L, 1);
            currentMatrix[3, 2] = -6 * E * Iy / Math.Pow(L, 2);
            currentMatrix[3, 3] = 4 * E * Iy / Math.Pow(L, 1);

            return currentMatrix;
        }

        private double GetCurrentE(int materialNumber)
        {
            IMaterial currentMaterial = model.scheme.Materials.Where(mat => mat.Number == materialNumber).FirstOrDefault();
            return currentMaterial.E;
        }

        private double GetCurrentIy(int crossSectionNumber)
        {
            ICrossSection currentCrossSection = model.scheme.CrossSections.Where(cs => cs.Number == crossSectionNumber).FirstOrDefault();
            return currentCrossSection.Iy;
        }

        private double GetCurrentL(int startNodeNumber, int endNodeNumber)
        {
            INode startNode = model.scheme.Nodes.Where(node => node.Number == startNodeNumber).SingleOrDefault();
            INode endNode = model.scheme.Nodes.Where(node => node.Number == endNodeNumber).SingleOrDefault();

            double L = Math.Abs(endNode.X - startNode.X);
            return L;
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;

using fob2d.Abstract;
using fob2d.StaticScheme;
using fob2d.NumericalModel;
using fob2d.NumericalSolver;
using fob2d.NumericalPostProcessor;

namespace fob2d.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Static scheme
             * 
             * 
             *                                       |F=15kN
             *          HEA400            |F=10kN    |
             *       *====================*==========*==========*
             *       /\                                        /\
             *       
             *       |                    |          |          |
             *       /--------5.0m--------/---2.5m---/---2.5m---/
             * 
             * 
             */

            // Proposed consistent units applied in this example are: [Pa], [N], [Nm], [m], [m^4]

            // Create a new instance of Scheme class with static scheme information
            IScheme scheme = new Scheme();

            // Create a new instance of Material class
            IMaterial material = scheme.NewMaterial();
            material.Number = 1;
            material.Name = "Steel";
            material.E = 2.1e11;
            material.v = 0.3;

            // Add an instance of Material class to the list of materials
            scheme.Materials.Add(material);


            // Create a new instance of CrossSection class
            ICrossSection crossSection = scheme.NewCrossSection();
            crossSection.Number = 1;
            crossSection.Name = "HEA400";
            crossSection.Iy = 0.00045070;

            // Add an instance of CrossSection class to the list of cross sections
            scheme.CrossSections.Add(crossSection);


            // Create new instances of Node class
            INode node_1 = scheme.NewNode();
            node_1.Number = 1;
            node_1.Name = "Start node";
            node_1.X = 0;

            INode node_2 = scheme.NewNode();
            node_2.Number = 2;
            node_2.Name = "Load node";
            node_2.X = 5;

            INode node_3 = scheme.NewNode();
            node_3.Number = 3;
            node_3.Name = "Middle node";
            node_3.X = 7.5;

            INode node_4 = scheme.NewNode();
            node_4.Number = 4;
            node_4.Name = "End node";
            node_4.X = 10.00;

            // Add instances of Node class to the list of nodes
            scheme.Nodes.Add(node_1);
            scheme.Nodes.Add(node_2);
            scheme.Nodes.Add(node_3);
            scheme.Nodes.Add(node_4);


            // Create new instances of Element class
            IElement element_1 = scheme.NewElement();
            element_1.Number = 1;
            element_1.Name = "Left element";
            element_1.MaterialNumber = 1;
            element_1.CrossSectionNumber = 1;
            element_1.StartNodeNumber = 1;
            element_1.EndNodeNumber = 2;

            IElement element_2 = scheme.NewElement();
            element_2.Number = 2;
            element_2.Name = "Middle element";
            element_2.MaterialNumber = 1;
            element_2.CrossSectionNumber = 1;
            element_2.StartNodeNumber = 2;
            element_2.EndNodeNumber = 3;

            IElement element_3 = scheme.NewElement();
            element_3.Number = 3;
            element_3.Name = "Right element";
            element_3.MaterialNumber = 1;
            element_3.CrossSectionNumber = 1;
            element_3.StartNodeNumber = 3;
            element_3.EndNodeNumber = 4;

            // Add instances of Element class to the list of elements
            scheme.Elements.Add(element_1);
            scheme.Elements.Add(element_2);
            scheme.Elements.Add(element_3);

            // Create a new instance of Load class
            ILoad load_1 = scheme.NewLoad();
            load_1.Number = 1;
            load_1.Name = "Force 1";
            load_1.NodeNumber = 2;
            load_1.Pz = -10000;
            load_1.My = 0;

            // Create a new instance of Load class
            ILoad load_2 = scheme.NewLoad();
            load_2.Number = 2;
            load_2.Name = "Force 2";
            load_2.NodeNumber = 3;
            load_2.Pz = -15000;
            load_2.My = 0;

            // Add an instance of Material class to the list of materials
            scheme.Loads.Add(load_1);
            scheme.Loads.Add(load_2);


            // Create a new instance of Model class with Finite Element Method model
            IModel model = new Model(scheme);
            model.AssembyModel();

            // ### Display calculated data for model ###
            DisplayResults(model);


            // Create a new instance of Solver class with Finite Element Method solver module
            ISolver solver = new Solver(model);
            solver.AssemblyFiniteElementModel();
            solver.Run();

            // ### Display calculated data for solver ###
            DisplayResults(solver);


            // Create a new instance of PostProcessor class with results of the finite element method calculations
            IPostProcessor postProcessor = new PostProcessor(scheme, model, solver);
            postProcessor.CalculateResults();

            // ### Display calculated data from post processor ###
            DisplayResults(postProcessor);

            Console.ReadKey();
        }

        // ### Display data from IModel class ###
        public static void DisplayResults(IModel model)
        {
            Console.WriteLine("### Boolean Matrix ###");
            foreach(var matrix in model.Boolean.Matrix)
            {
                DisplayMatrix2D<int>(matrix);
            }

            Console.WriteLine("### Transposed Boolean Matrix ###");
            foreach (var matrix in model.Boolean.TransposedMatrix)
            {
                DisplayMatrix2D<int>(matrix);
            }

            Console.WriteLine("### Beam Matrix ###");
            foreach (var matrix in model.Beam.Matrix)
            {
                DisplayMatrix2D<double>(matrix);
            }

            Console.WriteLine("### Support Matrix ###");
            DisplayMatrix2D<int>(model.Support.Matrix);

            Console.WriteLine("### Load Matrix ###");
            DisplayMatrix2D<double>(model.Load.Matrix);

            Console.WriteLine("### Identity Matrix ###");
            DisplayMatrix2D<int>(model.Identity.Matrix);

            Console.WriteLine("### Id Matrix ###");
            DisplayMatrix2D<int>(model.Identity.Id);

            Console.WriteLine("### Ip Matrix ###");
            DisplayMatrix2D<int>(model.Identity.Ip);
        }

        // ### Display data from ISolver class ###
        public static void DisplayResults(ISolver solver)
        {
            Console.WriteLine("### Global Stiffness Matrix ###");
            DisplayMatrix2D<double>(solver.K);

            Console.WriteLine("### KK Matrix ###");
            DisplayMatrix2D<double>(solver.KK);

            Console.WriteLine("### F Matrix ###");
            DisplayMatrix2D<double>(solver.F);

            Console.WriteLine("### Displacement Matrix ###");
            DisplayMatrix2D<double>(solver.U);

            Console.WriteLine("### Reactions Matrix ###");
            DisplayMatrix2D<double>(solver.R);
        }

        // ### Display data from IPostProcessor class ###
        public static void DisplayResults(IPostProcessor postProcessor)
        {
            Console.WriteLine("\n### Nodal Forces Matrix for each element ###");
            for (int i = 1; i <= postProcessor.ElementBoundaryForces.Count; i++)
            {
                DisplayMatrix2D<double>(postProcessor.ElementBoundaryForces[i]);
            }

            Console.WriteLine("\n### Nodal Displacement Matrix for each element ###");
            for (int i = 1; i <= postProcessor.ElementBoundaryDisplacements.Count; i++)
            {
                DisplayMatrix2D<double>(postProcessor.ElementBoundaryDisplacements[i]);
            }


            Console.WriteLine("\n### Nodal Displacement Matrix for each node ###");
            for (int i = 1; i <= postProcessor.NodalDisplacements.Count; i++)
            {
                DisplayMatrix2D<double>(postProcessor.NodalDisplacements[i]);
            }

            Console.WriteLine("\n### Total deflection u[m] ###");
            for (int i = 0; i <= 100; i++)
            {
                double def = postProcessor.GetDeflection(0.1 * i);
                Console.WriteLine(Convert.ToString(0.1 * i) + " " + String.Format("{0:0.0000}", def));
            }

            Console.WriteLine("\n### Total shear force Vz[kN] ###");
            for (int i = 0; i <= 100; i++)
            {
                IResult currentResult = postProcessor.GetShearForce(0.1 * i);

                if (currentResult.IsApproximated == false)
                {
                    Console.WriteLine(
                        Convert.ToString(0.1 * i) +
                        " LEFT: " + String.Format("{0:0.0}", currentResult.LeftValue) +
                        " RIGTH: " + String.Format("{0:0.0}", currentResult.RightValue) +
                        " DIFFERENCE:" + String.Format("{0:0.0}", currentResult.Difference));
                }
                else
                    Console.WriteLine(
                       Convert.ToString(0.1 * i) +
                       " " + String.Format("{0:0.0}", currentResult.LeftValue));

            }

            Console.WriteLine("\n### Total shear force Vz[kN] collection - with characteristic points ###");
            Dictionary<double, double> shearForceCollection = postProcessor.GetShearForceCollection(50);
            foreach(KeyValuePair<double, double> item in shearForceCollection)
            {
                Console.WriteLine(String.Format("For {0:0.000} m : {1:0.000} kN", item.Key, item.Value));
            }

            Console.WriteLine("\n### Total bending moment My ###");
            for (int i = 0; i <= 100; i++)
            {
                IResult currentResult = postProcessor.GetBendingMoment(0.1 * i);

                if (currentResult.IsApproximated == false)
                {
                    Console.WriteLine(
                        Convert.ToString(0.1 * i) +
                        " LEFT: " + String.Format("{0:0.0}", currentResult.LeftValue) +
                        " RIGTH: " + String.Format("{0:0.0}", currentResult.RightValue) +
                        " DIFFERENCE:" + String.Format("{0:0.0}", currentResult.Difference));
                }
                else
                    Console.WriteLine(
                       Convert.ToString(0.1 * i) +
                       " " + String.Format("{0:0.0}", currentResult.LeftValue));
            }
        }

        // Helper method
        public static void DisplayMatrix2D<T>(T[,] matrix)
        {
            for (int i = 0; i <= matrix.GetUpperBound(0); i++)
            {
                StringBuilder sb = new StringBuilder();

                for (int y = 0; y <= matrix.GetUpperBound(1); y++)
                {
                    sb.Append(String.Format("{0:0.000}", matrix[i, y]) + " ");
                }

                Console.WriteLine(sb.ToString());
            }
            Console.WriteLine("\n");
        }
    }
}

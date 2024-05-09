using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Task1;
using static Task1.Programm;

namespace Wpf_splines
{
    public class ViewData : IDataErrorInfo
    {
        public double[] Input_Boundaries { get; set; }
        public int Mesh_Nodes_Number {  get; set; }
        public bool IsUniform {  get; set; }
        public int FunctionInList { get; set; }
        public int Spline_Nodes_Number {  get; set; }
        public int Frequent_Mesh_Nodes_Number { get; set; }
        public double Residual_Norm_Eps {  get; set; }
        public int Function_In_List { get; set; }
        public int Max_Iterations { get; set; }

        public DataArray? dataarray;
        public SplineData? splinedata { get; set; }

        public class FunctionList
        {
            public static void EqualFunction(double x, ref double y1, ref double y2)
            {
                y1 = x;
                y2 = 0;
            }

            public static void SquareFunction(double x, ref double y1, ref double y2)
            {
                y1 = x * x;
                y2 = 0;
            }
            public static void CubeFunction(double x, ref double y1, ref double y2)
            {
                y1 = x * x * x;
                y2 = 0;
            }

            public static void SinFunction(double x, ref double y1, ref double y2)
            {
                y1 = Math.Sin(x);
                y2 = 0;
            }
            public static void CosFunction(double x, ref double y1, ref double y2)
            {
                y1 = Math.Cos(x);
                y2 = 0;
            }
            public static List<FValues> FuncList { get; set; } = new List<FValues>() { EqualFunction, SquareFunction, CubeFunction, CosFunction, SinFunction};
        }
        public void Save(string filename)
        {
            DataArray.Save(filename, ref dataarray);
        }
        public void Load(string filename) 
        {
            dataarray = new DataArray("", new DateTime());
            DataArray.Load(filename, ref dataarray);
        }
        public ViewData(double left_border=0, double right_border=1, int node_number=11, bool is_uniform=true, int spline_node_number=5,
            int frequent_node_number = 10, double residual_norm_eps = 1e-5, int max_iterations = 1000, int function_in_list = 0)
        {
            Input_Boundaries = new double[2] {left_border, right_border};
            Mesh_Nodes_Number = node_number;
            IsUniform = is_uniform;
            Spline_Nodes_Number = spline_node_number;
            Frequent_Mesh_Nodes_Number = frequent_node_number;
            Residual_Norm_Eps = residual_norm_eps;
            Max_Iterations = max_iterations;
            Function_In_List = function_in_list;
            dataarray = null;
            splinedata = null;
        }

        public void GetDataArray()
        {
            if (Input_Boundaries[1] <= Input_Boundaries[0])
            {
                throw new Exception("Неподходящие значения границ для отрезка");
            }

            if (Mesh_Nodes_Number <= 1)
            {
                throw new Exception("В сетке должно быть больше узлов");
            }

            FValues F = FunctionList.FuncList[Function_In_List];
            if (IsUniform)
            {

                dataarray = new DataArray("SomeKey", DateTime.Now, Mesh_Nodes_Number, Input_Boundaries[0], Input_Boundaries[1], F);
            }
            else
            {
                double[] x = new double[Mesh_Nodes_Number];
                double len = Input_Boundaries[1] - Input_Boundaries[0];
                x[0] = Input_Boundaries[0];
                for(int i = 1; i < Mesh_Nodes_Number; i++)
                {
                    x[i] = x[i-1] +  len * Math.Pow(2, -i);
                }
                dataarray = new DataArray("SomeKey", DateTime.Now, x, F);
            }
            
        }
        public void GetSplineData()
        {
            if (Spline_Nodes_Number <= 1 || Spline_Nodes_Number > Mesh_Nodes_Number)
            {
                throw new Exception("Введите корректное число узлов сплайна");
            }
            /*
            if (Frequent_Mesh_Nodes_Number <= 1  || Frequent_Mesh_Nodes_Number >= Mesh_Nodes_Number)
            {
                throw new Exception("Введите корректное число узлов для равномерной сетки, их не должно быть больше чем узлов исходной сетки");
            }
            */
            if (Residual_Norm_Eps <= 0 || Residual_Norm_Eps >= 1)
            {
                throw new Exception("Введите корректную норму невязки для остановки процесса интерполяции");
            }
            if (Max_Iterations <= 1)
            {
                throw new Exception("Введите корректное лимит числа итераций");
            }
            splinedata = new SplineData(dataarray, Spline_Nodes_Number, Max_Iterations, Frequent_Mesh_Nodes_Number, Residual_Norm_Eps);
            
        }
        public void GetSpline()
        {
            SplineData.SplineBuilding(splinedata);
            
        }

        public string this[string arg]
        {
            get
            {
                return arg switch
                {
                    "Mesh_Nodes_Number" => Mesh_Nodes_Number < 3 ? "Узлов сетки должно быть не меньше трех" : null,
                    "FrequentNodesNum" => Spline_Nodes_Number < 2 || Spline_Nodes_Number > Mesh_Nodes_Number ? "Узлов более мелкой сетки должно быть не меньше трех" : null,
                    "Input_Boundaries" => Input_Boundaries[0] >= Input_Boundaries[1] ? "Левая граница должна быть меньше правой" : null,
                    "SplineNodesNum" => Frequent_Mesh_Nodes_Number < 3 ? "Узлов сплайна должно быть не меньше трех и не больше чем узлов сетки" : null,
                    _ => null
                };
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    };
}

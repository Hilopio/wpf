using Microsoft.Win32;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Task1;
using static Task1.Programm;
using static LiveCharts.SeriesCollection;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using LiveCharts.Helpers;

namespace Wpf_splines
{
    public static class MyCommand
    {
        public static RoutedCommand CheckControlsCommand = new RoutedCommand("Check controls command", typeof(MyCommand));
    }
    public class MyAxes
    {
        public AxesCollection MyAxisY { get; set; }
        
        public MyAxes()
        {
            MyAxisY = new AxesCollection();
            MyAxisY.Add(new Axis
            {
                Title = "Values",
                Foreground = Brushes.Gray,
                LabelFormatter = value => value.ToString("N2"),

            });
        }
    }
    public partial class MainWindow : Window
    {
        public ViewData viewdata { get; set; }
        public SeriesCollection SeriesCollection {  get; set; }
        public MyAxes myaxes { get; set; }
        public AxesCollection MyAxisY {  get; set; }
        public MainWindow()
        {
            InitializeComponent();
        
            viewdata = new();
            Bind(viewdata);
            GetAxes();
            myaxes = new MyAxes();
            DataContext = null;
            DataContext = this;


        }
        public void GetAxes()
        {

        }
        public class BordersConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                double[] StrValue = (double[])value;
                string StrBorders = StrValue[0].ToString() + " " + StrValue[1].ToString();
                return StrBorders;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                try
                {
                    double[] borders_double = new double[2];
                    string[] borders_str = ((string)value).Split(' ');

                    if (borders_str.Length != 2)
                    {
                        throw new Exception("В ячейку границ отрезка введите два числа через пробел");
                    }

                    borders_double[0] = double.Parse(borders_str[0]);
                    borders_double[1] = double.Parse(borders_str[1]);
                    return borders_double;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    double[] k = { 0, 0 };
                    return k;
                }
            }
        }
        


        public void Bind(ViewData viewdata) 
        {
            // поля данных
            Binding ChooseFunctionBind = new()
            {
                Source = viewdata,
                Path = new PropertyPath("Function_In_List")
            };
            ChooseFunction.SetBinding(ComboBox.SelectedIndexProperty, ChooseFunctionBind);

            Binding UniformRadioBind = new()
            {
                Source = viewdata,
                Path = new PropertyPath("IsUniform")
            };
            UniformRadio.SetBinding(RadioButton.IsCheckedProperty, UniformRadioBind);

            Binding InputBordersBind = new()
            {
                Source = viewdata,
                Path = new PropertyPath("Input_Boundaries"),
                Converter = new BordersConverter(),
                ValidatesOnDataErrors=true
            };
            InputBoundaries.SetBinding(TextBox.TextProperty, InputBordersBind);


            Binding InputMeshNodesNumberBind = new()
            {
                Source = viewdata,
                Path = new PropertyPath("Mesh_Nodes_Number"),
                ValidatesOnDataErrors = true
            };
            InputMeshNodesNumber.SetBinding(TextBox.TextProperty, InputMeshNodesNumberBind);

            // поля сплайна
            Binding SplineNodesNumberBind = new()
            {
                Source = viewdata,
                Path = new PropertyPath("Spline_Nodes_Number")
            };
            SplineNodesNumber.SetBinding(TextBox.TextProperty, SplineNodesNumberBind);

            Binding FrequentMeshNodesNumberBind = new()
            {
                Source = viewdata,
                Path = new PropertyPath("Frequent_Mesh_Nodes_Number")
            };
            FrequentMeshNodesNumber.SetBinding(TextBox.TextProperty, FrequentMeshNodesNumberBind);

            Binding EpsResidualBind = new()
            {
                Source = viewdata,
                Path = new PropertyPath("Residual_Norm_Eps")
            };
            EpsResidual.SetBinding(TextBox.TextProperty, EpsResidualBind);

            Binding MaxIterationsBind = new()
            {
                Source = viewdata,
                Path = new PropertyPath("Max_Iterations")
            };
            MaxIterations.SetBinding(TextBox.TextProperty, MaxIterationsBind);
            
            
            
        }
        private void DataFromControlsCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                bool result = true;
                string? error = null;
                string[] error_types = { "Mesh_Nodes_Number", "SplineNodesNum", "Input_Boundaries", "FrequentNodesNum" };
                foreach (var field in error_types)
                {
                    error = viewdata[field];
                    result = error == null && result;
                }
                e.CanExecute = result;
            }
            catch (Exception ex)
            {
                e.CanExecute = false;
            }
        }
        private void DataSaveCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                bool result = true;
                string? error = null;
                string[] error_types = { "Mesh_Nodes_Number", "Input_Boundaries" };
                foreach (var field in error_types)
                {
                    error = viewdata[field];
                    result = error == null && result;
                }
                e.CanExecute = result;
            }
            catch (Exception ex)
            {
                e.CanExecute = false;
            }
        }
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog savefiledialog = new();
                string FilePath = "";
                if (savefiledialog.ShowDialog() == true)
                {
                    FilePath = savefiledialog.FileName;
                }
                viewdata.GetDataArray();
                viewdata.Save(FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DataFromControlsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                viewdata.GetDataArray();
                viewdata.GetSplineData();
                viewdata.GetSpline();


                Binding ListBox1Bind = new()
                {
                    Source = viewdata,
                    Path = new PropertyPath("splinedata.Spline")
                };
                SplineListBox.SetBinding(ListBox.ItemsSourceProperty, ListBox1Bind);

                Binding ListBox2Bind = new()
                {
                    Source = viewdata,
                    Path = new PropertyPath("splinedata.Nodes")
                };
                FrequentSplineListBox.SetBinding(ListBox.ItemsSourceProperty, ListBox2Bind);

                SeriesCollection = DrawSpline(viewdata);
                myaxes = new MyAxes();
                DataContext = null;
                DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataFromFileClick(object sender, RoutedEventArgs e)
        {

            try
            {
                OpenFileDialog openfiledialog = new();
                string filename = "";
                if (openfiledialog.ShowDialog() == true)
                {
                    filename = openfiledialog.FileName;
                }
                viewdata.Load(filename);
                viewdata.GetSplineData();
                viewdata.GetSpline();

                Binding ListBox1Bind = new()
                {
                    Source = viewdata,
                    Path = new PropertyPath("splinedata.Spline")
                };
                SplineListBox.SetBinding(ListBox.ItemsSourceProperty, ListBox1Bind);

                Binding ListBox2Bind = new()
                {
                    Source = viewdata,
                    Path = new PropertyPath("splinedata.Nodes")
                };
                FrequentSplineListBox.SetBinding(ListBox.ItemsSourceProperty, ListBox2Bind);

                SeriesCollection = DrawSpline(viewdata);
                myaxes = new MyAxes();
                DataContext = null;
                DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Чтение из файла не удалось");
            }
        }

    }

    
}
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
using static LiveCharts.SeriesCollection;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace WpfTestData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SeriesCollection Series { get; set; }

        public TestData testdata { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            testdata = new();
            Bind(testdata);
        }

        public void Bind(TestData testdata)
        {
            Binding LeftTestDataBind = new()
            {
                Source = testdata,
                Path = new PropertyPath("a")
            };
            LeftTestData.SetBinding(TextBox.TextProperty, LeftTestDataBind);
            Binding RightTestDataBind = new()
            {
                Source = testdata,
                Path = new PropertyPath("b")
            };
            RightTestData.SetBinding(TextBox.TextProperty, RightTestDataBind);
        }
        private void CheckTestData(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                bool result = true;
                string? error = null;
                string[] error_types = { "Borders"};
                foreach (var field in error_types)
                {
                    error = testdata[field];
                    result = error == null && result;
                }
                e.CanExecute = result;
            }
            catch (Exception ex)
            {
                e.CanExecute = false;
            }
        }
        private void DrawTestData(object sender, RoutedEventArgs e)
        {
            int len = 10;
            ChartValues<ObservablePoint> SinSeries = [];
            double[] xs = new double[len];
            double[] ys = new double[len];
            for (int i = 0; i < len; i++)
            {
                xs[i] = (testdata.b - testdata.a) / (len - 1) * i + testdata.a;
                ys[i] = Math.Sin(xs[i]);
                SinSeries.Add(new ObservablePoint(xs[i], ys[i]));
            }
            var converter = new System.Windows.Media.BrushConverter();
            var red = (Brush)converter.ConvertFromString("red");
            var gray = (Brush)converter.ConvertFromString("gray");
            var white = (Brush)converter.ConvertFromString("white");
            var green = (Brush)converter.ConvertFromString("green");

            Series =
            [
                /*
                new LineSeries
                {
                    LineSmoothness = 1,
                    Title = "Sin",
                    Values = SinSeries,
                },
                */
                new ScatterSeries
                {
                    Title = "Sin",
                    Values = SinSeries,
                    Fill = red,
                },

            ];
            DataContext = null;
            DataContext = this;
        }
    }
}
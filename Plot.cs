using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf_splines;
using System.Windows;
using System.Xml;

namespace Wpf_splines
{
    public partial class MainWindow : Window
    {
        public Func<double, string> YFormatter { get; set; }
        public SeriesCollection DrawSpline(ViewData viewdata)
        {
            int len = viewdata.splinedata.FrequentNodesNum;
            ChartValues<ObservablePoint> SplineSeries = [];
            for (int i = 0; i < len; i++)
            {
                double[] temp = viewdata.splinedata.Nodes[i];
                SplineSeries.Add(new ObservablePoint(temp[0], temp[1]));
            }
            var converter = new System.Windows.Media.BrushConverter();
            var red = (Brush)converter.ConvertFromString("red");
            var gray = (Brush)converter.ConvertFromString("gray");
            var white = (Brush)converter.ConvertFromString("white");
            var green = (Brush)converter.ConvertFromString("green");


            int DataLen = viewdata.dataarray.Grid.Length;

            ChartValues<ObservablePoint> DataSeries = [];
            for (int i = 0; i < DataLen; i++)
            {
                DataSeries.Add(new ObservablePoint(viewdata.dataarray.Grid[i], viewdata.dataarray.Fields[0][i]));
            }
            SeriesCollection SeriesCollection =
            [
                new LineSeries
                {
                    LineSmoothness = 1,
                    Title = "Spline",
                    Values = SplineSeries,
                },

                new ScatterSeries
                {
                    Title = "Data",
                    Values = DataSeries,
                    Fill = green
                },

            
            ];
            YFormatter = values => values.ToString("n2");
            return SeriesCollection;
        }
    }
}

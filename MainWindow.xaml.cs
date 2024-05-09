using System.Globalization;
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
using Task1;
using static Task1.Programm;

namespace CollectionList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class StringCont
        {
            public string ThisString { get; set; }
            public int position { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            var mycollection = new MainCollection(0, 0)
            {
                new DataList("key1", DateTime.Today, new double[] { 1, 2, 3 }, FDI_F),
                new DataArray("key2", DateTime.Today, new double[] { -1, 1, 2}, FValues_F),
                new DataList("key3", DateTime.Today, new double[] { 5, 5, 3 }, FDI_F),
                new DataArray("key4", DateTime.Today, new double[] { -1, 1, 2, 3, 4}, FValues_F),
            };


            Binding CollectionBind = new()
            {
                Source = mycollection,
            };
            CollectionListBox.SetBinding(ListBox.ItemsSourceProperty, CollectionBind);

            StringCont mystringcont = new StringCont();

            Binding CollectionClickBind = new()
            {
                Source = mystringcont,
                Path = new PropertyPath("position"),
                Mode = BindingMode.OneWayToSource
            };
            CollectionListBox.SetBinding(ListBox.SelectedIndexProperty, CollectionClickBind);

            Binding ThisStringBind = new()
            {
                Source = mystringcont,
                Path = new PropertyPath("position"),
                ConverterParameter = mycollection,
                Converter = new ItemConverter()
            };
            ThisStringText.SetBinding(TextBlock.TextProperty, ThisStringBind);
            
        }

        public class ItemConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int pos = (int)value;
                if (pos == -1)
                {
                    return "Здесь будет ваш выбор";
                }

                MainCollection mc = (MainCollection)parameter;
                for(int i = 0; i < mc.Count; ++i)
                {
                    if (i == pos) return mc[i].ToLongString("");
                }
                return mc;

            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return true;
            }
        }
    }
}
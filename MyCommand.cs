using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfTestData
{
    public static class MyCommand
    {
        public static RoutedCommand CheckControlsCommand = new RoutedCommand("Check controls command", typeof(MyCommand));
    }
}

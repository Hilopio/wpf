using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestData
{
    public class TestData : IDataErrorInfo
    {
        public double a {  get; set; }
        public double b { get; set; }

        public TestData(double a = 0, double b = 0) 
        {
            this.a = a;
            this.b = b;
        }

        public string this[string arg]
        {
            get
            {
                return arg switch
                {
                    "Borders" => a >= b ? "Левая граница больше или равна правой" : null,
                    _ => null
                };
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}

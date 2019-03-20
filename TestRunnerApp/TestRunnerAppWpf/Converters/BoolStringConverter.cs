using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TestRunnerAppWpf
{
    public class BoolStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case true:
                    return "True";
                case false:
                    return "False";
                default:
                    return "Neither true nor false";

            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            switch (value)
            {
                case "true":
                    return true;
                case "True":
                    return true;
                case "1":
                    return true;
                default:
                    return false;

            }
        }
    }
}

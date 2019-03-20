using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public class WebDriverTypeImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case WebDriverType.Chrome:
                    return "pack://application:,,,/Icons/chrome-512.png";
                case WebDriverType.Firefox:
                    return "pack://application:,,,/Icons/firefox.png";
                case WebDriverType.IE:
                    return "pack://application:,,,/Icons/internet_explorer.png";
                case WebDriverType.None:
                    return "pack://application:,,,/Icons/transparent16.png";
                default:
                    return "Unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}

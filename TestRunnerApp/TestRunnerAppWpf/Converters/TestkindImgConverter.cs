using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public class TestkindImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case TestKind.Other:
                     return "pack://application:,,,/Icons/StatusHelp_grey_disabled_16x.png";
                case TestKind.AD:
                    return "pack://application:,,,/Icons/StatusCriticalError_16x.png";
                case TestKind.ADO:
                    return "pack://application:,,,/Icons/StatusWarning_16x.png";
                case TestKind.Exchange:
                    return "pack://application:,,,/Icons/StatusHelp_grey_disabled_16x.png";
                case TestKind.Web:
                    return "pack://application:,,,/Icons/StatusHelp_grey_disabled_16x.png";
                default:
                    return "pack://application:,,,/Icons/StatusHelp_grey_disabled_16x.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

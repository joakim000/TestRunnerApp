using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public class OutcomeImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case Outcome.Pass:
                    return "pack://application:,,,/Icons/StatusOK_16x.png";
                case Outcome.Fail:
                    return "pack://application:,,,/Icons/StatusCriticalError_16x.png";
                case Outcome.Warning:
                    return "pack://application:,,,/Icons/StatusWarning_16x.png";
                case Outcome.NotRun:
                    return "pack://application:,,,/Icons/StatusHelp_grey_disabled_16x.png";
                default:
                    return "Unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

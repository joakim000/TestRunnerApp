using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public class OutcomeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case Outcome.Pass:
                    return "Pass";
                case Outcome.Fail:
                    return "Fail";
                case Outcome.Warning:
                    return "Warning";
                case Outcome.NotRun:
                    return "N/A";
                default:
                    return "Unknown";
            }
//switch (value)
//            {
//                case 0:
//                    return "Pass";
//                case 1:
//                    return "Fail";
//                case 2:
//                    return "Warning";
//                default:
//                    return "Unknown";
//            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public class VisiJiraCloudTmj : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Managment m = (Managment)value;
            if (m == Enums.Mgmt.Find(x => x.key == "JiraCloudTmj"))
                return "Visible";
            else
                return "Collapsed";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolJiraCloudTmj : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Managment m = (Managment)value;
            if (m == Enums.Mgmt.Find(x => x.key == "JiraCloudTmj"))
                return true;
            else
                return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

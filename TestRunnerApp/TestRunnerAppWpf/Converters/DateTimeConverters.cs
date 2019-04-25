using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TestRunnerAppWpf
{
    public class DateTimeShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;
                string dtString = $"{dt.ToShortDateString()} {dt.ToShortTimeString()}";
                if (String.Equals(dtString, "0001-01-01 00:00"))
                    return string.Empty;
                else
                    return dtString;
            }
            else
                //return "No date";
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class DateTimeForceShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;
                return String.Format("{0:yy-MM-dd HH:mm}", dt);  
            }
            else
                return "No date";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }


    public class CalendarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var dtString = value as string;
            DateTime dt = DateTime.Now;
            DateTime.TryParse(dtString, out dt);
            return dt;
        }
    }

}

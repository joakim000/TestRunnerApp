using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TestRunnerAppWpf
{
    public class ElementSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string elements = null;
            string reserved = null;
            try
            {
                string s = parameter.ToString();
                int separatorIndex = s.IndexOf("#");
                elements = s.Substring(0, separatorIndex);
                reserved = s.Substring(separatorIndex + 1);
                //System.Diagnostics.Debug.WriteLine($"grid actualwidth:{value.ToString()}  elements:{elements}  reserved:{reserved}");

                double availableSpace = double.Parse(value.ToString()) - double.Parse(reserved);
                return (int)Math.Floor(availableSpace / double.Parse(elements));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Exception converting elementsize.{Environment.NewLine}" +
                    $"grid actualwidth:{value.ToString()}  elements:{elements}  reserved:{reserved}{Environment.NewLine}{e.Message}");
                return 100;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}

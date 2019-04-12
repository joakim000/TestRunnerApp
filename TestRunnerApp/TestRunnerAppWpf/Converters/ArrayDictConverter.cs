using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TestRunnerAppWpf
{
    public class ArrayDictConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringArray = value as string[];

            var stringDict = new Dictionary<int, string>();

            for (int i = 0; i < stringArray.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(stringArray[i]))
                {
                    stringDict.Add(i, stringArray[i]);
                }
            }
            return stringDict;

            

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] stringArray = { "troll", "boll" };

            return stringArray;
        }
    }
}

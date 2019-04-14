using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ViewModelSupport;

namespace TestRunnerAppWpf
{
    public class ArrayDictConverter : IValueConverter
    {
        public class KeyValue : ViewModelBase
        {
            //public int Key { get; set; }
            //public string Value { get; set; }

            public int Key
            {
                get => Get(() => Key);
                set => Set(() => Key, value);
            }
            public string Value 
            {
                get => Get(() => Value);
                set => Set(() => Value, value);
            }

            public KeyValue(int key, string value)
            {
                Key = key;
                Value = value;
            }
        }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //var stringArray = value as string[];
            //var stringDict = new Dictionary<int, string>();
            //for (int i = 0; i < stringArray.Length; i++)
            //{
            //    if (!string.IsNullOrWhiteSpace(stringArray[i]))
            //    {
            //        stringDict.Add(i, stringArray[i]);
            //    }
            //}
            //return stringDict;


            var stringArray = value as string[];
            var kvl = new ObservableCollection<KeyValue>();
            //var kvl = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(stringArray[i]))
                {
                    kvl.Add(new KeyValue(i, stringArray[i]));
                }
            }
            return kvl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var kvl = value as ObservableCollection<KeyValue>;
            string[] stringArray = new string[64];
            foreach (var kv in kvl)
            {
                stringArray[kv.Key] = kv.Value;
            }

            return stringArray;
        }
    }
}

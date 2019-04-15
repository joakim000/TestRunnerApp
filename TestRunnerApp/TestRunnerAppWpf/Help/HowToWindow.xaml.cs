using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class HowToWindow : Window
    {
        string howTo = string.Empty;

        public HowToWindow()
        {
            InitializeComponent();
            DataContext = this;

            try
            {
                //Type t = typeof(TestModel);
                var ass = Assembly.GetAssembly(typeof(TestModel));
                string resourceName = ass.GetManifestResourceNames().Single(str =>
                    str.EndsWith("HowTo.cs"));
                using (Stream stream = ass.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        howTo = reader.ReadToEnd();
                    }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception reading HowTo.cs: {e}");
                howTo = fillString();
            }


            HowToTB.Text = howTo;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new SaveFileDialog
            {
                Filter = "Text files (*.cs)|*.cs|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = "TestAppHowTo.cs"
        };
            if (picker.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(picker.FileName, howTo);
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Error saving file:{Environment.NewLine}{exception.Message}");
                    Debug.WriteLine($"Error saving file:{Environment.NewLine}{exception.Message}");
                }
            }
            else
            {
                Debug.WriteLine("Save file cancelled");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string fillString()
        {
            return @"Template file not found.";
        }

       
    } // class
} // namespace

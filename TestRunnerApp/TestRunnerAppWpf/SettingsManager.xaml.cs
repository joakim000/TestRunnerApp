using System;
using System.Collections.Generic;
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
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public partial class SettingsManager : Window
    {
        public SettingsModel settings { get; set; }

        public SettingsManager()
        {
            InitializeComponent();

            settings = new SettingsModel();
            DataContext = settings;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{settings.user}:{settings.pw}");
            var base64token = Convert.ToBase64String(byteArray);
            settings.token = base64token;

            this.DialogResult = true;
        }


    }
}

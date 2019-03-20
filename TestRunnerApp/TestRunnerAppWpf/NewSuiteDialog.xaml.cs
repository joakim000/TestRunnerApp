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
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public partial class NewSuiteDialog : Window
    {
        public SuiteModel newItem { get; set; }

        public NewSuiteDialog()
        {
            InitializeComponent();

            newItem = new SuiteModel();
            DataContext = newItem;

            //newItem.PropertyChanged += NewItem_PropertyChanged;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}

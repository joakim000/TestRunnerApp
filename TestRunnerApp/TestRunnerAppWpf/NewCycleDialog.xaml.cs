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
    public partial class NewCycleDialog : Window
    {
        public CycleModel newItem { get; set; }

        public NewCycleDialog(MainViewModel mainViewModel)
        {

            DataContext = mainViewModel;
            newItem = new CycleModel();
            

            if (mainViewModel.jiraCloudMgmt)
            {

            }
            

            InitializeComponent();

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

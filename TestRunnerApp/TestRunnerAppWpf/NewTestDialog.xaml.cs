using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public partial class NewTestDialog : Window
    {
        public NewTestDialogViewModel viewModel { get; set; }
        MainViewModel mainViewModel { get; set; }

        public NewTestDialog(MainViewModel mainViewModel)
        {
            InitializeComponent();

            viewModel = new NewTestDialogViewModel(mainViewModel);
            DataContext = viewModel;
            //viewModel.labelsPanel = LabelsPanel;

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

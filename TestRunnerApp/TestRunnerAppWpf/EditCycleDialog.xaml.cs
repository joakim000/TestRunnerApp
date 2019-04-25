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
    public partial class EditCycleDialog : Window
    {
        public CycleDialogViewModel viewModel { get; set; }
        MainViewModel mainViewModel { get; set; }
        
        public EditCycleDialog(MainViewModel mainViewModel)
        {
            InitializeComponent();

            viewModel = new CycleDialogViewModel(mainViewModel, this);
            DataContext = viewModel;
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

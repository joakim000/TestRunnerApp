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
        public CycleDialogViewModel viewModel { get; set; }
        MainViewModel mainViewModel { get; set; }
        
        public NewCycleDialog(MainViewModel mainViewModel)
        {
            InitializeComponent();

            viewModel = new CycleDialogViewModel(mainViewModel, this);
            DataContext = viewModel;
            viewModel.SetSelectedDateRange();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cal_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.Cal_SelectedDatesChanged(sender, e);
        }

        private void PlannedStart_LostFocus(object sender, RoutedEventArgs e)
        {
            viewModel.PlannedStart_LostFocus(sender, e);
        }
        private void PlannedEnd_LostFocus(object sender, RoutedEventArgs e)
        {
            viewModel.PlannedEnd_LostFocus(sender, e);
        }
    }
}

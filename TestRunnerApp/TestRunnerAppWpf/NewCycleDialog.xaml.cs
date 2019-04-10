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
        public NewCycleDialogViewModel viewModel { get; set; }

        public CycleModel newItem { get; set; }
        MainViewModel mainViewModel { get; set; }
        //bool jiraCloudMgmt { get; set; }
        //bool jiraServerMgmt { get; set; }
        //bool reqTestdMgmt { get; set; }

    

        //public NewCycleDialog(bool jiraCloudMgmt, bool jiraServerMgmt, bool reqTestMgmt)
        public NewCycleDialog(MainViewModel mainViewModel)
        {
            InitializeComponent();

            viewModel = new NewCycleDialogViewModel(mainViewModel);
            DataContext = viewModel;

            //jiraPanel.DataContext = mainViewModel.jiraCloudMgmt;
            //jiraGrid.DataContext = newItem;
            

            


            


            

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

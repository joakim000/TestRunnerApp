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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            model.gridViewModel = gridView.model;
            model.detailsViewModel = detailsView.model;
            model.appWindow = this; // For Activate()

            model.gridViewModel.selectedItems.PropertyChanged += model.SelectedItems_PropertyChanged;
            model.SelectedItems_PropertyChanged(null, null);

            // Need to set these here first time to avoid NullReferenceException
            model.gridViewModel.mainViewModel = model;
            model.gridViewModel.PropertyChanged += model.GridViewModel_PropertyChanged;
            model.gridViewModel.suite.PropertyChanged += model.Suite_PropertyChanged;
            model.gridViewModel.suite.tests.CollectionChanged += model.Tests_CollectionChanged;
            model.gridViewModel.suite.cycles.CollectionChanged += model.Cycles_CollectionChanged;

            model.detailsViewModel.mainViewModel = model;
            model.detailsViewModel.PropertyChanged += model.DetailsViewModel_PropertyChanged;

            // Continue previous session, if not opened by association
            if (string.IsNullOrEmpty(model.gridViewModel.suite.filename) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousDir) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousFile))
            {
                Tuple<string, SuiteModel> fileopen =
                    FileMgmt.OpenSuite(Properties.Settings.Default.PreviousDir + @"\" +
                    Properties.Settings.Default.PreviousFile);

                if (fileopen.Item2 != null)
                {
                    model.gridViewModel.suite = fileopen.Item2;
                    model.SelectedItems_PropertyChanged(null, null);
                    FileMgmt.unsavedChanges = false;
                    model.unsavedChanges = false;
                }
                if (fileopen.Item1 != null)
                    model.gridViewModel.suite.filename = fileopen.Item1;

                if (model.gridViewModel.suite.currentCycle != null)
                {
                    string cc = model.gridViewModel.suite.currentCycle.key;
                    model.gridViewModel.suite.currentCycle = model.gridViewModel.suite.cycles.Where(x => x.key == cc).First();
                    CyclesCombo.SelectedItem = model.gridViewModel.suite.currentCycle;
                }

            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Debug.WriteLine("Saving user-settings");
            Properties.Settings.Default.Save();

            if (model.unsavedChanges)
            {
                var result = MessageBox.Show("Save suite before exit?", "TestApp",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        model.Execute_FileSaveCmd();
                        e.Cancel = false;
                        break;
                    case MessageBoxResult.No:
                        e.Cancel = false;
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CyclesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

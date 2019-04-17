using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            model.gridViewModel.suite = model.detailsViewModel.suite;
            model.detailsViewModel.PropertyChanged += model.DetailsViewModel_PropertyChanged;

            // Continue previous session, if not opened by association
            if (string.IsNullOrEmpty(model.gridViewModel.suite.filename) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousDir) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousFile))
            {
                string fileToOpen = Properties.Settings.Default.PreviousDir + @"\" +
                    Properties.Settings.Default.PreviousFile;

                FileMgmt.OpenFileSetup(fileToOpen, model);

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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelSupport;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public class GridViewModel : ViewModelBase
    {
        public SuiteModel suite
        {
            get => Get(() => suite);
            set => Set(() => suite, value);
        }
        public SelectedItems selectedItems
        {
            get => Get(() => selectedItems);
            set => Set(() => selectedItems, value);
        }
        public MainViewModel mainViewModel
        {
            get => Get(() => mainViewModel);
            set => Set(() => mainViewModel, value);

        }


        public GridViewModel()
        {
            suite = new SuiteModel();
            selectedItems = new SelectedItems();
            this.PropertyChanged += GridViewModel_PropertyChanged;
            AttachChildEvents();
        }

        private void GridViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "suite")
                AttachChildEvents();
                FileMgmt.unsavedChanges = false;
                mainViewModel.unsavedChanges = false;
        }
        
        private void AttachChildEvents()
        {
            suite.PropertyChanged += Suite_PropertyChanged;
            suite.tests.CollectionChanged += Tests_CollectionChanged;
        }

        private void Tests_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => mainViewModel.unsavedChanges = true;
        private void Suite_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => mainViewModel.unsavedChanges = true;
    }
}

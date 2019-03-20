using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelSupport;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public class SelectedItems : ViewModelBase
    {
        public ObservableCollection<TestModel> selectedItems
        {
            get => Get(() => selectedItems);
            set => Set(() => selectedItems, value);
        }

        public SelectedItems()
        {
            selectedItems = new ObservableCollection<TestModel>();
        }

    }
}

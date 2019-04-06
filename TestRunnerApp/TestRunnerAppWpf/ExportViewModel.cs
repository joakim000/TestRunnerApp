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



    class ExportViewModel : ViewModelBase
    {
        public ExportViewModel() { }

        public ObservableCollection<CycleModel> cycles
        {
            get => Get(() => cycles);
            set => Set(() => cycles, value);
        }

        /* New cycle */
        public string projectKey
        {
            get => Get(() => projectKey);
            set => Set(() => projectKey, value);
        }
        public string name
        {
            get => Get(() => name);
            set => Set(() => name, value);
        }
        public string key
        {
            get => Get(() => key);
            set => Set(() => key, value);
        }
        public string description
        {
            get => Get(() => description);
            set => Set(() => description, value);
        }
        public string folderId
        {
            get => Get(() => folderId);
            set => Set(() => folderId, value);
        }
        public string folderName
        {
            get => Get(() => folderName);
            set => Set(() => folderName, value);
        }
        public string setOwner
        {
            get => Get(() => setOwner);
            set => Set(() => setOwner, value);
        }

               

    }
}

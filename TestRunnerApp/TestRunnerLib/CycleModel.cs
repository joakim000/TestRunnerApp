using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ViewModelSupport;

namespace TestRunnerLib
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CycleModel : ViewModelBase
    {
       
        public ObservableCollection<TestModel> tests
        {
            get => Get(() => tests);
            set => Set(() => tests, value);
        }
        public string name
        {
            get => Get(() => name);
            set => Set(() => name, value);
        }
        public string description
        {
            get => Get(() => description);
            set => Set(() => description, value);
        }
        public string folder
        {
            get => Get(() => folder);
            set => Set(() => folder, value);
        }
        public string status
        {
            get => Get(() => status);
            set => Set(() => status, value);
        }
        public string version
        {
            get => Get(() => version);
            set => Set(() => version, value);
        }
       



        public CycleModel()
        {
            tests = new ObservableCollection<TestModel>();
        }

    }
}

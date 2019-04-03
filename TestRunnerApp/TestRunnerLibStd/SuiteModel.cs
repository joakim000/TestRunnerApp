using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ViewModelSupport;

namespace TestRunnerLib
{
    [JsonObject(MemberSerialization.OptOut)]
    public class SuiteModel : ViewModelBase
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
        public string system
        {
            get => Get(() => system);
            set => Set(() => system, value);
        }
        public string filename
        {
            get => Get(() => filename);
            set => Set(() => filename, value);
        }
        public string version
        {
            get => Get(() => version);
            set => Set(() => version, value);
        }
        public string developer
        {
            get => Get(() => developer);
            set => Set(() => developer, value);
        }
        public string tester
        {
            get => Get(() => tester);
            set => Set(() => tester, value);
        }

        public string notes
        {
            get => Get(() => notes);
            set => Set(() => notes, value);
        }


        public SuiteModel()
        {
            tests = new ObservableCollection<TestModel>();
        }
    }
}

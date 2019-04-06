using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ViewModelSupport;

namespace TestRunnerLib
{
    [JsonObject(MemberSerialization.OptOut)]

    public class CycleRun : ViewModelBase
    {
        public CycleRun(TestModel t, RunModel r)
        {
            test = t;
            run = r;

            testUid = t.uid;
            runUid = r.uid;
            testVersion = t.version;
        }

        public TestModel test
        {
            get => Get(() => test);
            set => Set(() => test, value);
        }
        public Guid testUid
        {
            get => Get(() => testUid);
            set => Set(() => testUid, value);
        }
        public string testVersion
        {
            get => Get(() => testVersion);
            set => Set(() => testVersion, value);
        }
        public RunModel run
        {
            get => Get(() => run);
            set => Set(() => run, value);
        }
        public Guid runUid
        {
            get => Get(() => runUid);
            set => Set(() => runUid, value);
        }
    }

    public class CycleModel : ViewModelBase
    {
        public CycleModel()
        {
            uid = Guid.NewGuid();
            tests = new ObservableCollection<TestModel>();
            runs = new ObservableCollection<RunModel>();
            cycleRuns = new ObservableCollection<CycleRun>();
        }
        public ObservableCollection<TestModel> tests
        {
            get => Get(() => tests);
            set => Set(() => tests, value);
        }
        public ObservableCollection<RunModel> runs
        {
            get => Get(() => runs);
            set => Set(() => runs, value);
        }
        public ObservableCollection<CycleRun> cycleRuns
        {
            get => Get(() => cycleRuns);
            set => Set(() => cycleRuns, value);
        }
        public Guid uid
        {
            get => Get(() => uid);
            set => Set(() => uid, value);
        }

        /* Jira integration */
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
        public string version // Version of what? Test probably
        {
            get => Get(() => version);
            set => Set(() => version, value);
        }
        /* end: Jira integration */



        

    }
}

using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using TestRunnerLib.Jira;
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

            //testUid = t.uid;
            //runUid = r.uid;
            //testVersion = t.version;
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
        public bool exported 
        {
            get => Get(() => exported);
            set => Set(() => exported, value);
        }

    }

    [JsonObject(MemberSerialization.OptOut)]
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
        public string id
        {
            get => Get(() => id);
            set => Set(() => id, value);
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
        public string status
        {
            get => Get(() => status);
            set => Set(() => status, value);
        }
        public string notes
        {
            get => Get(() => notes);
            set => Set(() => notes, value);
        }
        public Mgmt mgmt
        {
            get => Get(() => mgmt, Mgmt.None);
            set => Set(() => mgmt, value);
        }


        /* Jira integration */
        public bool jiraCloud
        {
            get => Get(() => jiraCloud, false);
            set => Set(() => jiraCloud, value);
        }
        public string jiraProjectKey
        {
            get => Get(() => jiraProjectKey);
            set => Set(() => jiraProjectKey, value);
        }
        public JiraCycle jiraCycle
        {
            //get => Get(() => jiraCycle, new JiraCycle());
            get => Get(() => jiraCycle);
            set => Set(() => jiraCycle, value);
        }
        


        
        //public string key
        //{
        //    get => Get(() => key);
        //    set => Set(() => key, value);
        //}
      
        //public string folder
        //{
        //    get => Get(() => folder);
        //    set => Set(() => folder, value);
        //}
        //public string status
        //{
        //    get => Get(() => status);
        //    set => Set(() => status, value);
        //}
        //public string version // Version of what? Test probably
        //{
        //    get => Get(() => version);
        //    set => Set(() => version, value);
        //}
        /* end: Jira integration */





    }
}

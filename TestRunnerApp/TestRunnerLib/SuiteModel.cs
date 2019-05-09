using Newtonsoft.Json;

using System;
using System.Collections.ObjectModel;

using TestRunnerLib.Jira;

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
        public ObservableCollection<CycleModel> cycles
        {
            get => Get(() => cycles);
            set => Set(() => cycles, value);
        }
        public ObservableCollection<JiraStatus> testStatuses
        {
            get => Get(() => testStatuses);
            set => Set(() => testStatuses, value);
        }
        public ObservableCollection<JiraPrio> prios
        {
            get => Get(() => prios);
            set => Set(() => prios, value);
        }
        public CycleModel currentCycle
        {
            get => Get(() => currentCycle);
            set => Set(() => currentCycle, value);
        }
        public Guid uid
        {
            get => Get(() => uid);
            set => Set(() => uid, value);
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




        //public Mgmt mgmt
        //{
        //    get => Get(() => mgmt, Mgmt.None);
        //    set => Set(() => mgmt, value);
        //}
        public Managment mgmt
        {
            get => Get(() => mgmt);
            set => Set(() => mgmt, value);
        }
        //public List<Managment> mgmtOptions
        //{
        //    get => Get(() => mgmtOptions, Enums.Mgmt);
        //    set => Set(() => mgmtOptions, value);
        //}

        /* Jira */
        public string jiraInstance
        {
            get => Get(() => jiraInstance);
            set => Set(() => jiraInstance, value);
        }
        public JiraProject jiraProject
        {
            get => Get(() => jiraProject);
            set => Set(() => jiraProject, value);
        }
        public JiraProject jiraSelectedCyclesFolder
        {
            get => Get(() => jiraSelectedCyclesFolder);
            set => Set(() => jiraSelectedCyclesFolder, value);
        }
        public JiraProject jiraSelectedTestsFolder
        {
            get => Get(() => jiraSelectedTestsFolder);
            set => Set(() => jiraSelectedTestsFolder, value);
        }
        public bool jiraLabelToId
        {
            get => Get(() => jiraLabelToId);
            set => Set(() => jiraLabelToId, value);
        }
        public string jiraLabelToIdToken
        {
            get => Get(() => jiraLabelToIdToken);
            set => Set(() => jiraLabelToIdToken, value);
        }


        // deprecated
        public JiraCycle jiraCycle
        {
            get => Get(() => jiraCycle);
            set => Set(() => jiraCycle, value);
        }


        public SuiteModel()
        {
            uid = Guid.NewGuid();
            tests = new ObservableCollection<TestModel>();
            cycles = new ObservableCollection<CycleModel>();
            //name = "Untitled suite";

            //if (mgmt == null)
            //    mgmt = Enums.Mgmt.Find(x => x.key == "None");

            //currentCycle = new CycleModel();
            //currentCycle.key = "R1";
            //currentCycle.name = "New cycle";
            //cycles.Add(currentCycle);

            // Default prios
            if (prios == null)
            {
                prios = new ObservableCollection<JiraPrio>();
                prios.Add(new JiraPrio("Low", "Green"));
                prios.Add(new JiraPrio("Normal", "DarkGoldenRod"));
                prios.Add(new JiraPrio("High", "Red"));
            }
            

            // Default statuses
            if (testStatuses == null)
            {
                testStatuses = new ObservableCollection<JiraStatus>();
                testStatuses.Add(new JiraStatus("Ready", "Blue"));
                testStatuses.Add(new JiraStatus("Draft", "DarkGoldenRod"));
                testStatuses.Add(new JiraStatus("Approved", "Green"));
                testStatuses.Add(new JiraStatus("On hold", "Gray"));
            }
            
                 


            this.PropertyChanged += SuiteModel_PropertyChanged;

        }

        private void SuiteModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "jiraLabelToId")
            {
                foreach (TestModel t in tests)
                    t.jiraLabelToId = jiraLabelToId;
            }
            if (e.PropertyName == "jiraLabelToIdToken")
            {
                foreach (TestModel t in tests)
                    t.jiraLabelToIdToken = jiraLabelToIdToken;
            }
        }
    }
}

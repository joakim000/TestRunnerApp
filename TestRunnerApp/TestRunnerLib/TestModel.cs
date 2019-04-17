using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using TestRunnerLib.Jira;
using ViewModelSupport;
using Lib;
using System.Linq;
using System.ComponentModel;

namespace TestRunnerLib
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TestDataItem : ViewModelBase
    {
        public int index
        {
            get => Get(() => index);
            set => Set(() => index, value);
        }
        public string data
        {
            get => Get(() => data);
            set => Set(() => data, value);
        }

        public TestDataItem(int index, string data)
        {
            this.index = index;
            this.data = data;
        }
    }


    [JsonObject(MemberSerialization.OptOut)]
    public class TestModel : ViewModelBase
    {
        public ObservableCollection<RunModel> runs
        {
            get => Get(() => runs);
            set => Set(() => runs, value);
        }
        public ObservableCollection<CycleModel> cycles
        {
            get => Get(() => cycles);
            set => Set(() => cycles, value);
        }
        public Guid uid
        {
            get => Get(() => uid, Guid.NewGuid());
            set => Set(() => uid, value);
        }
        // Duplicates child data
            // Number of runs: runs.Count();
        public int numberOfRuns
        {
            get => Get(() => numberOfRuns, 0);
            set => Set(() => numberOfRuns, value);
        }
            // Last result: runs.Last().result
        public Outcome previousOutcome
        {
            get => Get(() => previousOutcome, Outcome.NotRun);
            set => Set(() => previousOutcome, value);
        }
        public DateTime? previousDateTime
        {
            get => Get(() => previousDateTime);
            set => Set(() => previousDateTime, value);
        }
        public Int64 previousRunTime
        {
            get => Get(() => previousRunTime);
            set => Set(() => previousRunTime, value);
        }


        /* Description fields */
        public TestKind kind
        {
            get => Get(() => kind);
            set => Set(() => kind, value);
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
        public string objective
        {
            get => Get(() => objective);
            set => Set(() => objective, value);
        }
        public string descExecution
        {
            get => Get(() => descExecution);
            set => Set(() => descExecution, value);
        }
        public string descPrecond
        {
            get => Get(() => descPrecond);
            set => Set(() => descPrecond, value);
        }
        public string descExpected
        {
            get => Get(() => descExpected);
            set => Set(() => descExpected, value);
        }
        public string notes
        {
            get => Get(() => notes);
            set => Set(() => notes, value);
        }
        public string prio
        {
            get => Get(() => prio);
            set => Set(() => prio, value);
        }
        public string version
        {
            get => Get(() => version, "1.0");
            set => Set(() => version, value);
        }
        public int? estimatedTime
        {
            get => Get(() => estimatedTime);
            set => Set(() => estimatedTime, value);
        }
        /* end: Description fields */



        /* Test call */
        public string callAss
        {
            get => Get(() => callAss);
            set => Set(() => callAss, value);
        }
        public string callSpace
        {
            get => Get(() => callSpace);
            set => Set(() => callSpace, value);
        }
        public string callType
        {
            get => Get(() => callType);
            set => Set(() => callType, value);
        }
        public ObservableCollection<TestDataItem> testDataColl
        {
            get => Get(() => testDataColl);
            set => Set(() => testDataColl, value);
        }
        /* end: Test call */

        /* Jira integration */
        public JiraCase jiraCase
        {
            get => Get(() => jiraCase);
            set => Set(() => jiraCase, value);
        }
        public bool jiraCloudTmj
        {
            get => Get(() => jiraCloudTmj, false);
            set => Set(() => jiraCloudTmj, value);
        }
        public string jiraProjectKey
        {
            get => Get(() => jiraProjectKey);
            set => Set(() => jiraProjectKey, value);
        }
        /* end: Jira integration */


        /* Deprecated */
        //public string callMeth
        //{
        //    get => Get(() => callMeth);
        //    set => Set(() => callMeth, value);
        //}
        //public string webDriver
        //{
        //    get => Get(() => webDriver);
        //    set => Set(() => webDriver, value);
        //}
        //public bool useWebDriver
        //{
        //    get => Get(() => useWebDriver);
        //    set => Set(() => useWebDriver, value);
        //}
        


        public TestModel()
        {
            runs = new ObservableCollection<RunModel>();
            cycles = new ObservableCollection<CycleModel>();
            testDataColl = new ObservableCollection<TestDataItem>();
            previousOutcome = Outcome.NotRun;
            this.PropertyChanged += TestModel_PropertyChanged;
        }

        private void TestModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "jiraCase")
            {
                if (jiraCase != null)
                {
                    jiraCase.PropertyChanged += JiraCase_PropertyChanged;
                    CopyFromJiraCase();
                }
            }
        }

        private void JiraCase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CopyFromJiraCase();
        }


        private void CopyFromJiraCase()
        {
            id = jiraCase.key;
            name = jiraCase.name;
            objective = jiraCase.objective;
            descPrecond = jiraCase.precondition;

            if (jiraCase.priority != null)
                prio = jiraCase.priority.name;

            estimatedTime = jiraCase.estimatedTime;

            //if (jiraCase.status != null)
            //    status = jiraCase.status.name;

            //if (jiraCase.folder != null)
            //    prio = jiraCase.folder.name;

            //if (jiraCase.owner != null)
            //    owner = jiraCase.owner.displayName

            //createdOn = jiraCase.createdOn;       


        }



        public void Execute_AddTestDataCmd()
        {
            AddTestdata();
        }
        public bool CanExecute_AddTestDataCmd()
        {
            return true;
        }

        public void AddTestdata()
        {
            int highestIndex = 0;
            if (testDataColl.Count > 0)
                highestIndex = testDataColl.Max<TestDataItem, int>(x => x.index);
            testDataColl.Add(new TestDataItem(highestIndex + 1, string.Empty));
        }
        

        private string StringCopy(string s)
        {
            return s == null ? null : string.Copy(s);
        }

        public TestModel DeepCopy() 
        {
            var c = new TestModel();

            c.callAss = StringCopy(callAss);
            c.callSpace = StringCopy(callSpace);
            c.callType = StringCopy(callType);

            //c.testData = new string[testData.Length];
            //Array.Copy(testData, c.testData, testData.Length);
            c.testDataColl = new ObservableCollection<TestDataItem>();
            foreach (TestDataItem tdi in testDataColl)
            {
                TestDataItem toAdd = new TestDataItem(tdi.index, tdi.data);
                c.testDataColl.Add(toAdd);
            }


            c.kind = kind; // ref
            c.id = StringCopy(id);
            c.name = StringCopy(name);
            c.objective = StringCopy(objective);

            c.descExecution = StringCopy(descExecution);
            c.descPrecond = StringCopy(descPrecond);
            c.descExpected = StringCopy(descExpected);

            c.notes = StringCopy(notes);
            c.prio = StringCopy(prio);
            c.version = StringCopy(version);

            c.jiraProjectKey = StringCopy(jiraProjectKey);
            c.jiraCloudTmj = jiraCloudTmj;
            c.jiraCase = jiraCase;


            //c.jiraPrioId = StringCopy(jiraPrioId);
            //c.jiraPrioName = StringCopy(jiraPrioName);
            //c.jiraTestId = StringCopy(jiraTestId);
            //c.jiraTestName = StringCopy(jiraTestName);
            //c.jiraTestVersion = StringCopy(jiraTestVersion);

            return c;
        }


    }
}

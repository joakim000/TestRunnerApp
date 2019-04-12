using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using TestRunnerLib.Jira;
using ViewModelSupport;

namespace TestRunnerLib
{
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
        public DateTime previousDateTime
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
        public string[] testData
        {
            get => Get(() => testData, new string[64]);
            set => Set(() => testData, value);
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

        //public string jiraTestId
        //{
        //    get => Get(() => jiraTestId);
        //    set => Set(() => jiraTestId, value);
        //}
        //public string jiraTestName
        //{
        //    get => Get(() => jiraTestName);
        //    set => Set(() => jiraTestName, value);
        //}
        //public string jiraPrioId
        //{
        //    get => Get(() => jiraPrioId);
        //    set => Set(() => jiraPrioId, value);
        //}
        //public string jiraPrioName
        //{
        //    get => Get(() => jiraPrioName);
        //    set => Set(() => jiraPrioName, value);
        //}
        //public string jiraTestVersion
        //{
        //    get => Get(() => jiraTestVersion);
        //    set => Set(() => jiraTestVersion, value);
        //}
        /* end: Jira integration */


        /* Deprecated */
        public string callMeth
        {
            get => Get(() => callMeth);
            set => Set(() => callMeth, value);
        }
        public string webDriver
        {
            get => Get(() => webDriver);
            set => Set(() => webDriver, value);
        }
        public bool useWebDriver
        {
            get => Get(() => useWebDriver);
            set => Set(() => useWebDriver, value);
        }
            /* For compatibility with v1 .testapp-files */
        public string callParam
        {
            get => Get(() => callParam);
            set => Set(() => callParam, value);
        }
        public string callParam2
        {
            get => Get(() => callParam2);
            set => Set(() => callParam2, value);
        }
        public string callParam3
        {
            get => Get(() => callParam3);
            set => Set(() => callParam3, value);
        }
        public string callParam4
        {
            get => Get(() => callParam4);
            set => Set(() => callParam4, value);
        }
            /* end: For compatibility with v1 .testapp-files */
        /* end: Deprecated */

       


        public TestModel()
        {
            //uid = Guid.NewGuid();
            testData = new string[64];
            runs = new ObservableCollection<RunModel>();
            cycles = new ObservableCollection<CycleModel>();
            previousOutcome = Outcome.NotRun;
            useWebDriver = true;

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
            c.testData = new string[testData.Length];
            Array.Copy(testData, c.testData, testData.Length);

            c.kind = kind; // ref
            c.id = StringCopy(id);
            c.name = StringCopy(name);

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

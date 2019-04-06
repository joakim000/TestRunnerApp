using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
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
            get => Get(() => uid);
            set => Set(() => uid, value);
        }
        // Duplicates child data
            // Number of runs: runs.Count();
        public int numberOfRuns
        {
            get => Get(() => numberOfRuns);
            set => Set(() => numberOfRuns, value);
        }
            // Last result: runs.Last().result
        public Outcome previousOutcome
        {
            get => Get(() => previousOutcome);
            set => Set(() => previousOutcome, value);
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
            get => Get(() => version, "1");
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
            get => Get(() => testData);
            set => Set(() => testData, value);
        }
        /* end: Test call */

        /* Jira integration */
        public string jiraTestId
        {
            get => Get(() => jiraTestId);
            set => Set(() => jiraTestId, value);
        }
        public string jiraTestName
        {
            get => Get(() => jiraTestName);
            set => Set(() => jiraTestName, value);
        }
        public string jiraPrioId
        {
            get => Get(() => jiraPrioId);
            set => Set(() => jiraPrioId, value);
        }
        public string jiraPrioName
        {
            get => Get(() => jiraPrioName);
            set => Set(() => jiraPrioName, value);
        }
        public string jiraTestVersion
        {
            get => Get(() => jiraPrioName);
            set => Set(() => jiraPrioName, value);
        }
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
            uid = Guid.NewGuid();
            runs = new ObservableCollection<RunModel>();
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
            c.callAss = string.Copy(callAss);
            c.callMeth = StringCopy(callMeth);
            c.callParam = string.Copy(callParam);
            c.callParam2 = StringCopy(callParam2);
            c.callParam3 = StringCopy(callParam3);
            c.callParam4 = StringCopy(callParam4);
            c.callSpace = StringCopy(callSpace);
            c.callType = StringCopy(callType);
            c.descExecution = StringCopy(descExecution);
            c.descExpected = StringCopy(descExpected);
            c.id = StringCopy(id);
            c.name = StringCopy(name);
            c.notes = StringCopy(notes);
            c.prio = StringCopy(prio);

            c.numberOfRuns = 0;
            c.previousOutcome = Outcome.NotRun;



            return c;


        }


    }
}

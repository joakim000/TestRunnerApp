using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ViewModelSupport;

namespace TestRunnerLib
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TestModel : ViewModelBase
    {
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
        public ObservableCollection<RunModel> runs
        {
            get => Get(() => runs);
            set => Set(() => runs, value);
        }
        public Outcome previousOutcome
        {
            get => Get(() => previousOutcome);
            set => Set(() => previousOutcome, value);
        }
        public int numberOfRuns
        {
            get => Get(() => numberOfRuns);
            set => Set(() => numberOfRuns, value);
        }
        // Number of runs: runs.Count();
        // Last result: runs.Last().result
        // Has run: runs.Count() > 0

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



        public TestModel()
        {
            runs = new ObservableCollection<RunModel>();
            previousOutcome = Outcome.NotRun;
            useWebDriver = true;
        }

    }
}

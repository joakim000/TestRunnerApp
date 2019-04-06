using System;
using System.Diagnostics;
using Newtonsoft.Json;
using ViewModelSupport;

namespace TestRunnerLib
{
    [JsonObject(MemberSerialization.OptOut)]
    public class RunModel : ViewModelBase
    {
        public Guid uid
        {
            get => Get(() => uid, Guid.NewGuid());
            set => Set(() => uid, value);
        }
        public TestModel test
        {
            get => Get(() => test);
            set => Set(() => test, value);
        }

        /* In test */
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
        public string[] testData // In test
        {
            get => Get(() => testData);
            set => Set(() => testData, value);
        }
        public TestKind testKind  // In test, result
        {
            get => Get(() => testKind);
            set => Set(() => testKind, value);
        }
        /* In test */

        public TestResult resultObj
        {
            get => Get(() => resultObj);
            set => Set(() => resultObj, value);
        }
        public DateTime datetime
        {
            get => Get(() => datetime);
            set => Set(() => datetime, value);
        }
        public DateTime datetimeEnd
        {
            get => Get(() => datetime);
            set => Set(() => datetime, value);
        }
        public double runTime
        {
            get => Get(() => runTime);
            set => Set(() => runTime, value);
        }
        public WebDriverType webDriverType // In result, better here?
        {
            get => Get(() => webDriverType, WebDriverType.None);
            set => Set(() => webDriverType, value);
        }
        public string environment
        {
            get => Get(() => environment);
            set => Set(() => environment, value);
        }
        public bool rerun
        {
            get => Get(() => rerun);
            set => Set(() => rerun, value);
        }
        public string note
        {
            get => Get(() => note);
            set => Set(() => note, value);
        }
        public Outcome result // In result. Good for when run fails and resultObj is null.
        {
            get => Get(() => result, Outcome.NotRun);
            set => Set(() => result, value);
        }
       


        /* Deprecated */
        public Guid id
        {
            get => Get(() => uid);
            set => Set(() => uid, value);
        }
        /* end: Deprecated */




        public RunModel() { }

        public RunModel(TestModel t)
        {
            RunTest(t);
        }

        public RunModel(TestModel t, WebDriverType webDriverType)
        {
            this.webDriverType = webDriverType;
            RunTest(t);
        }

        private void RunTest(TestModel t)
        {
            //test = t; // Should be deep copy, method in TestModel
            test = t.DeepCopy();
            testUid = t.uid;
            testVersion = t.version;
            //testdata
            //testkind

            rerun = t.numberOfRuns > 0 ? true : false;
            datetime = DateTime.Now;
                Debug.WriteLine($"Invoke: driverType=[{webDriverType}]  param=[{t.callParam}, {t.callParam2}, {t.callParam3}, {t.callParam4}, ]");
            resultObj = Runner8.InvokeTest(t.callAss, t.callSpace, t.callType, webDriverType, t.callParam, t.callParam2, t.callParam3, t.callParam4);
            datetimeEnd = DateTime.Now;
            runTime = datetimeEnd.Subtract(datetime).TotalMilliseconds;

            if (resultObj != null)
                result = resultObj.outcome;
        }
    }
}

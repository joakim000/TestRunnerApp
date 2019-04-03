using System;
using System.Diagnostics;
using Newtonsoft.Json;
using ViewModelSupport;

namespace TestRunnerLib
{
    [JsonObject(MemberSerialization.OptOut)]
    public class RunModel : ViewModelBase
    {
        public Guid id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public Outcome result
        {
            get => Get(() => result);
            set => Set(() => result, value);
        }
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

        public RunModel() { }

        public RunModel(TestModel t)
        {
            WebDriverType webDriverType = WebDriverType.None;

            id = Guid.NewGuid();
            rerun = t.numberOfRuns > 0 ? true : false;
            datetime = DateTime.Now;

            Debug.WriteLine($"Invoke: driverType=[{webDriverType}]  param=[{t.callParam}, {t.callParam2}, {t.callParam3}, {t.callParam4}, ]");

            resultObj = Runner8.InvokeTest(t.callAss, t.callSpace, t.callType, webDriverType, t.callParam, t.callParam2, t.callParam3, t.callParam4);

            if (resultObj != null)
                result = resultObj.outcome;
        }

        public RunModel(TestModel t, WebDriverType webDriverType)
        {
            id = Guid.NewGuid();
            rerun = t.numberOfRuns > 0 ? true : false;
            datetime = DateTime.Now;

            Debug.WriteLine($"Invoke: driverType=[{webDriverType}]  param=[{t.callParam}, {t.callParam2}, {t.callParam3}, {t.callParam4}, ]");

            resultObj = Runner8.InvokeTest(t.callAss, t.callSpace, t.callType, webDriverType, t.callParam, t.callParam2, t.callParam3, t.callParam4);

            if (resultObj != null)
                result = resultObj.outcome;
        }
    }
}

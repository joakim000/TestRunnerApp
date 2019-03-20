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
            id = Guid.NewGuid();
            rerun = t.numberOfRuns > 0 ? true : false;
            datetime = DateTime.Now;

            string param1 = null;
            if (!string.IsNullOrEmpty(t.callParam))
                param1 = t.callParam;

            WebDriverType driver = WebDriverType.None;
            switch (Properties.Settings.Default.WebDriver)
            {
                case "chrome":
                    driver = WebDriverType.Chrome;
                    break;
                case "firefox":
                    driver = WebDriverType.Firefox;
                    break;
                case "ie":
                    driver = WebDriverType.IE;
                    break;
                default:
                    driver = WebDriverType.None;
                    break;
            }
                
            Debug.WriteLine($"Invoke: driverType=[{driver}]  param=[{param1}]");

            resultObj = Runner8.InvokeTest(t.callAss, t.callSpace, t.callType, driver, t.callParam, t.callParam2, t.callParam3, t.callParam4);

            if (resultObj != null)
                result = resultObj.outcome;
        }
    }
}

using System;
using Newtonsoft.Json;
using ViewModelSupport;


namespace TestRunnerLib
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class TestResult : ViewModelBase
    {
        public Outcome outcome
        {
            get => Get(() => outcome, Outcome.NotRun);
            set => Set(() => outcome, value);
        }

         public TestKind kind
        {
            get => Get(() => kind);
            set => Set(() => kind, value);
        }

        public int failStep
        {
            get => Get(() => failStep, 0);
            set => Set(() => failStep, value);
        }
        public string message
        {
            get => Get(() => message, string.Empty);
            set => Set(() => message, value);
        }

        public WebDriverType webDriver
        {
            get => Get(() => webDriver, WebDriverType.None);
            set => Set(() => webDriver, value);
        }

        public Exception e
        {
            get => Get(() => e);
            set => Set(() => e, value);
        }
        public string eType
        {
            get => Get(() => eType);
            set => Set(() => eType, value);
        }
        public string eMessage
        {
            get => Get(() => eMessage);
            set => Set(() => eMessage, value);
        }
         public System.Collections.IDictionary eData
        {
            get => Get(() => eData);
            set => Set(() => eData, value);
        }

        private void setWebDriver()
        {

        }

        public TestResult() { }

        // 1 return
        public TestResult(Outcome o) => outcome = o;
        // 2 returns
        public TestResult(Outcome o, int step)
        {
            outcome = o;
            failStep = step;
        }
        public TestResult(Outcome o, string s)
        {
            outcome = o;
            message = s;
        }

        public TestResult(Outcome o, Exception e)
        {
            outcome = o;
            this.e = e;
            message = "Exception";
            eType = e.GetType().ToString();
            eMessage = e.Message;
            eData = e.Data;
        }
        // 3 returns
        public TestResult(Outcome o, int step, Exception e)
        {
            outcome = o;
            failStep = step;
            message = "Exception";
            this.e = e;
            eType = e.GetType().ToString();
            eMessage = e.Message;
            eData = e.Data;
        }
        public TestResult(Outcome o, string s, Exception e)
        {
            outcome = o;
            message = s;
            this.e = e;
            eType = e.GetType().ToString();
            eMessage = e.Message;
            eData = e.Data;
        }
        public TestResult(Outcome o, int step, string s)
        {
            outcome = o;
            failStep = step;
            message = s;
        }
        // 4 returns
        public TestResult(Outcome o, int step, string s, Exception e)
        {
            outcome = o;
            failStep = step;
            message = s;
            this.e = e;
            eType = e.GetType().ToString();
            eMessage = e.Message;
            eData = e.Data;
        }

    }
}

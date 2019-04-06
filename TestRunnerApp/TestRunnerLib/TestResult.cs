using System;
using Newtonsoft.Json;
using ViewModelSupport;


namespace TestRunnerLib
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class TestResult : ViewModelBase
    {
        public Guid uid
        {
            get => Get(() => uid, Guid.NewGuid());
            set => Set(() => uid, value);
        }
        public Outcome outcome
        {
            get => Get(() => outcome, Outcome.NotRun);
            set => Set(() => outcome, value);
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
       
        /* Exceptions */
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
        /* end: Exceptions */

        /* Deprecated */
        public WebDriverType webDriver // In RunModel
        {
            get => Get(() => webDriver, WebDriverType.None);
            set => Set(() => webDriver, value);
        }
        public TestKind kind // In TestModel
        {
            get => Get(() => kind);
            set => Set(() => kind, value);
        }
        /* end: Deprecated */



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
        }
        public TestResult(Outcome o, string s, Exception e)
        {
            outcome = o;
            message = string.IsNullOrWhiteSpace(s) ? "Exception" : s;
            this.e = e;
            eType = e.GetType().ToString();
            eMessage = e.Message;
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
            message = string.IsNullOrWhiteSpace(s) ? "Exception" : s;
            this.e = e;
            eType = e.GetType().ToString();
            eMessage = e.Message;
        }

    }
}

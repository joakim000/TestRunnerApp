using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using TestRunnerLib;

using ViewModelSupport;

namespace TestRunnerAppCon
{
    public class RunTests
    {
        
        private WebDriverType webDriverType;
        private Model model;
        private CycleModel currentCycle;
        private CustomSynchronizationContext syncContext;

        private string[] outcomes;
        private string idPattern;

        private BackgroundWorker runTestWorker = null;

        public List<TestModel> Run(Model model,
                        string[] outcomes,
                        string idPattern,
                        Dictionary<string, Tuple<int, string>> testData,
                        WebDriverType webDriverType,
                        CustomSynchronizationContext context,
                        BackgroundWorker worker)
        {
            this.model = model;
            this.outcomes = outcomes;
            this.idPattern = idPattern;
            this.webDriverType = webDriverType;

            this.runTestWorker = worker;
            this.syncContext = context;

            this.currentCycle = model.suite.currentCycle;

            var tests = new ObservableCollection<TestModel>(Report.SelectTests(model.suite, outcomes, idPattern));

            // Modify testdata from cmd-line
            foreach (var td in testData)
            {
                TestModel t = tests.Where(test => string.Equals(test.id, td.Key)).SingleOrDefault();
                if (t != null)
                {
                    if (t.testDataColl.Where(tdi => tdi.index == td.Value.Item1).Count() > 0)
                    {
                        t.testDataColl.Where(tdi => tdi.index == td.Value.Item1).Single().data = td.Value.Item2;
                    }
                }
            }

            StartAsyncRunner(tests);
            return new List<TestModel>(tests);
        }


        private void CancelRunJob() => runTestWorker.CancelAsync();

        private void StartAsyncRunner(ObservableCollection<TestModel> tests)
        {
            //enableRun = false;
            //if (Properties.Settings.Default.OnTop)
            //    topMost = true;
            //progressBarValue = 0;
            //runStatus = "Running";
            //runTotal = tests.Count().ToString();
            //runSlash = "/";
            //runCurrent = "1";

            //runTestWorker = new BackgroundWorker
            //{
            //    WorkerReportsProgress = true,
            //    WorkerSupportsCancellation = true
            //};
            runTestWorker.WorkerReportsProgress = true;
            runTestWorker.WorkerSupportsCancellation = true;

            runTestWorker.DoWork += runTestWorker_DoWork;
            runTestWorker.ProgressChanged += runTestWorker_ProgressChanged;
            runTestWorker.RunWorkerCompleted += runTestworker_RunWorkerCompleted;

            //syncContext = SynchronizationContext.Current;
            runTestWorker.RunWorkerAsync(tests);
        }

        void runTestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //WebDriverType webDriverType = Settings.GetWebDriverType();

            var tests = (ObservableCollection<TestModel>)e.Argument;
            int testsRun = 0;
            int testsToRun = tests.Count();
            CycleModel cycle = currentCycle;

            int pass = 0, fail = 0, warning = 0, skipped = 0;


            foreach (TestModel test in tests)
            {
                if (string.IsNullOrEmpty(test.callAss) || 
                    string.IsNullOrEmpty(test.callSpace) ||
                    string.IsNullOrEmpty(test.callType))
                {
                    skipped++;
                    continue;
                }


                Console.WriteLine($"Running {test.id} {test.name}");
                Debug.WriteLine($"Running {test.id} {test.name}");

                RunModel r = new RunModel(test, webDriverType);
                syncContext.Send(x => test.runs.Add(r), null);
                syncContext.Send(x => test.numberOfRuns = test.runs.Count(), null);
                syncContext.Send(x => test.previousOutcome = r.result, null);
                syncContext.Send(x => test.previousDateTime = r.datetime, null);
                syncContext.Send(x => test.previousRunTime = r.runTime, null);

                if (cycle != null)
                {
                    CycleRun cycleRun = new CycleRun(test, r);
                    syncContext.Send(x => cycle.cycleRuns.Add(cycleRun), null);
                }

                testsRun++;
                if (test.previousOutcome == Outcome.Pass) pass++;
                if (test.previousOutcome == Outcome.Fail) fail++;
                if (test.previousOutcome == Outcome.Warning) warning++;


                if (runTestWorker.CancellationPending == true)
                {
                    //syncContext.Send(x => runStatus = "Cancelled", null);
                    e.Cancel = true;
                    return;
                }
                int progressPercentage = Convert.ToInt32(((double)testsRun / testsToRun) * 100);
                Tuple<int, int, TestModel> report = new Tuple<int, int, TestModel>(testsRun, testsToRun, test);
                (sender as BackgroundWorker).ReportProgress(progressPercentage, report);

            }
            e.Result = new Tuple<int, int, int, int>(pass, fail, warning, skipped);

        }

        void runTestWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBarValue = e.ProgressPercentage;
            if (e.UserState != null)
            {
                // Update UI
                var t = (Tuple<int, int, TestModel>)e.UserState;


                string line = 

                string.Format("|{0,2}{1,3}{2,2}{3,5}|{4,10}{5,30}{6,10}",
                    t.Item1.ToString(),
                    "of",
                    t.Item2.ToString(),
                    e.ProgressPercentage + "%",
                    t.Item3.id,
                    t.Item3.name,
                    t.Item3.previousOutcome
                    );

                Console.WriteLine(line);


                //runCurrent = (t.Item1 + 1).ToString();
            }
        }

        void runTestworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //topMost = false;
            //appWindow.Activate();
            //enableRun = true;
            //if (runStatus != "Cancelled")
            //{
            //    runStatus = "Done";
            //    runCurrent = runTotal;
            //    ResetProgress();
            //}

            //detailsViewModel.UpdateLog();

            model.unsavedChanges = true;

            Tuple<int, int, int, int> result = null;
            string resultLine = null;

            try
            {
                result = e.Result as Tuple<int, int, int, int>;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Exception reading results: {ex.Message}");
                Debug.WriteLine($"Exception reading results: {ex}");
                resultLine = $"Exception reading results: {ex.Message}";
            }


            if (result != null)
            {
                resultLine =
                string.Format("{0,10}{1,3}{2,10}{3,3}{4,10}{5,3}{6,10}{7,3}",
                     "Passed:",
                     result.Item1.ToString(),
                     "Failed:",
                     result.Item2.ToString(),
                     "Warning:",
                     result.Item3.ToString(),
                     "Skipped:",
                     result.Item4.ToString()
                     );
            }
            else
                if (string.IsNullOrEmpty(resultLine))
                    resultLine = "Error reading results.";


            string error = e.Error == null ? "None" : e.Error.Message;

            if (e.Cancelled)
            {
                Console.WriteLine($"Run cancelled");
                Console.WriteLine($"Error code: {e.Error}");
                Console.WriteLine(resultLine);
            }
            else
            {
                Console.WriteLine($"Run complete");
                Console.WriteLine($"Error code: {e.Error}");
                Console.WriteLine(resultLine);

                //Report.ListTests(model.suite, outcomes, idPattern);

                //Console.WriteLine("Saving suite to file.");
                //FileMgmt.SaveSuite(model.suite);
            }


            

            //Debug.WriteLine($"Async runTestWorker result: {e.Result}");
        }

    }
}

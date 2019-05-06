using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using TestRunnerLib;

using ViewModelSupport;

namespace TestRunnerAppWpf
{
    public partial class MainViewModel : ViewModelBase
    {
        /* Run tests */ // Should be moved to lib? Has view updates...
        private void CancelRunJob() => runTestWorker.CancelAsync();

        private void StartAsyncRunner(ObservableCollection<TestModel> tests)
        {
            enableRun = false;
            if (Properties.Settings.Default.OnTop)
                topMost = true;
            progressBarValue = 0;
            runStatus = "Running";
            runTotal = tests.Count().ToString();
            runSlash = "/";
            runCurrent = "1";

            runTestWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            runTestWorker.DoWork += runTestWorker_DoWork;
            runTestWorker.ProgressChanged += runTestWorker_ProgressChanged;
            runTestWorker.RunWorkerCompleted += runTestworker_RunWorkerCompleted;

            syncContext = SynchronizationContext.Current;
            runTestWorker.RunWorkerAsync(tests);
        }

        void runTestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            WebDriverType webDriverType = Settings.GetWebDriverType();

            var tests = (ObservableCollection<TestModel>)e.Argument;
            int testsRun = 0;
            int testsToRun = tests.Count();
            CycleModel cycle = gridViewModel.suite.currentCycle;

            foreach (TestModel test in tests)
            {
                Debug.WriteLine($"Running {test.name}");
                TestRunnerLib.Log.AddNoWrite($"Running {test.name}");

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
                if (runTestWorker.CancellationPending == true)
                {
                    syncContext.Send(x => runStatus = "Cancelled", null);
                    e.Cancel = true;
                    return;
                }
                int progressPercentage = Convert.ToInt32(((double)testsRun / testsToRun) * 100);
                Tuple<int, string> report = new Tuple<int, string>(testsRun, $"Test: {test.name}   Outcome: {test.previousOutcome}");
                (sender as BackgroundWorker).ReportProgress(progressPercentage, report);

            }
            e.Result = testsRun;

        }

        void runTestWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarValue = e.ProgressPercentage;
            if (e.UserState != null)
            {
                // Update UI
                var t = (Tuple<int, string>)e.UserState;
                Debug.WriteLine(t.Item2);
                runCurrent = (t.Item1 + 1).ToString();
            }
        }

        void runTestworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            topMost = false;
            appWindow.Activate();
            enableRun = true;
            if (runStatus != "Cancelled")
            {
                runStatus = "Done";
                runCurrent = runTotal;
                ResetProgress();
            }
            detailsViewModel.UpdateLog();
            this.unsavedChanges = true;
            //Debug.WriteLine($"Async runTestWorker result: {e.Result}");
        }

    }
}

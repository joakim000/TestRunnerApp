using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViewModelSupport;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public partial class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Debug.WriteLine("Creating mainViewModel");


            runStatus = "Idle";
            runTotal = string.Empty;
            runCurrent = string.Empty;
            runSlash = string.Empty;
            topMost = false;
            enableRun = true;

            this.unsavedChanges = false;

            Settings.GetSettings(this);
            CheckWebDriverAvailibility();

        }

        /* Events */
        public void GridViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            unsavedChanges = true;
            if (e.PropertyName == "suite")
            {
                unsavedChanges = true;
                gridViewModel.suite.PropertyChanged += Suite_PropertyChanged;
                gridViewModel.suite.tests.CollectionChanged += Tests_CollectionChanged;
                gridViewModel.suite.cycles.CollectionChanged += Cycles_CollectionChanged;
                setWindowTitle();
            }
        }

        internal void Tests_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            this.unsavedChanges = true;

        internal void Cycles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            this.unsavedChanges = true;

        internal void Suite_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.unsavedChanges = true;
            if (e.PropertyName == "name")
                setWindowTitle();
        }

        public void SelectedItems_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (gridViewModel.selectedItems.selectedItems.Count() == 1)
            {
                detailsViewModel.test = gridViewModel.selectedItems.selectedItems[0];
            }
            else
            {
                detailsViewModel.suite = gridViewModel.suite;
            }

            if (gridViewModel.selectedItems.selectedItems == null)
                Debug.WriteLine("selectedItems is null");
            else if (gridViewModel.selectedItems.selectedItems.Count() == 0)
                Debug.WriteLine("selectedItems count is 0");
            else
            {
                foreach (TestModel test in gridViewModel.selectedItems.selectedItems)
                {
                    Debug.WriteLine($"{test.name} Runs:{test.runs.Count()} Last outcome:{test.previousOutcome}");
                }
            }
        }

        public void DetailsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //unsavedChanges = true;
        }

            /* end: Events */



            private void setWindowTitle() => 
            windowTitle = string.IsNullOrWhiteSpace(gridViewModel.suite.name) ? Properties.Settings.Default.AppTitle : $"{gridViewModel.suite.name}  - {Properties.Settings.Default.AppTitle}";

        public void CheckWebDriverAvailibility()
        {
            /* Current availibility-testing too time-consuming to do at startup */
            //WebDriver.checkAvailibility();
            //chromeAvailable = WebDriver.chromeAvailable;
            //firefoxAvailable = WebDriver.firefoxAvailable;
            //ieAvailable = WebDriver.ieAvailable;

            chromeAvailable = Properties.Settings.Default.chromeAvailable;
            firefoxAvailable = Properties.Settings.Default.firefoxAvailable;
            ieAvailable = Properties.Settings.Default.ieAvailable; 
        }
       

        public void RunAllAsync()
        {
            StartAsyncRunner(gridViewModel.suite.tests);
        }

        public void RunSelectedAsync(ObservableCollection<TestModel> tests)
        {
            #if DEBUG
            Debug.WriteLine("Running selected tests");
            if (gridViewModel.selectedItems.selectedItems == null)
                Debug.WriteLine("selectedItems is null");
            else if (gridViewModel.selectedItems.selectedItems.Count() == 0)
                Debug.WriteLine("selectedItems count is 0");
            else
            {
                foreach (TestModel test in gridViewModel.selectedItems.selectedItems)
                {
                    Debug.WriteLine($"{test.name} Runs:{test.runs.Count()} Last outcome:{test.previousOutcome}");
                }
            }
            #endif

            StartAsyncRunner(tests);
        }

       

        /* Run tests */ // Should be moved to lib? Has view updates...
        private void CancelJob() => worker.CancelAsync();

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

            

            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            syncContext = SynchronizationContext.Current;
            worker.RunWorkerAsync(tests);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            WebDriverType webDriverType = Settings.GetWebDriverType();

            var tests = (ObservableCollection<TestModel>)e.Argument;
            int testsRun = 0;
            int testsToRun = tests.Count();

            //CycleModel cycle = new CycleModel();
            //cycle.name = "Cycle name";
            //cycle.description = "Cycle description";
            //syncContext.Send(x => gridViewModel.suite.cycles.Add(cycle), null);
            CycleModel cycle = gridViewModel.suite.currentCycle;
            

            foreach (TestModel test in tests)
            {
                Debug.WriteLine($"Running {test.name}");

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
                if (worker.CancellationPending == true)
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

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarValue = e.ProgressPercentage;
            if (e.UserState != null)
            {
                // Update UI
                var t = (Tuple<int, string>)e.UserState;
                Debug.WriteLine( t.Item2);
                runCurrent = (t.Item1 + 1).ToString();
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            topMost = false;
            appWindow.Activate();
            enableRun = true;
            if (runStatus != "Cancelled")
            {
                runStatus = "Done";
                runCurrent = runTotal;
            }
            this.unsavedChanges = true;
            Debug.WriteLine($"Async worker result: {e.Result}");

            //Debug.WriteLine($"Cycles in suite: {gridViewModel.suite.cycles.Count()}");
            //if (gridViewModel.suite.cycles.Count() > 0)
            //{


            //    Debug.WriteLine($"Testruns in first cycle: {gridViewModel.suite.cycles.First().cycleRuns.Count()}");
            //    foreach (CycleRun c in gridViewModel.suite.cycles.First().cycleRuns)
            //    {
            //        Debug.WriteLine($"Current testname: {c.test.name} Name at run time: {c.run.test.name} Outcome: {c.run.result.ToString()}");
            //    }
            //    Debug.WriteLine($"Testruns in previous cycle: {gridViewModel.suite.cycles.Last().cycleRuns.Count()}");
            //    foreach (CycleRun c in gridViewModel.suite.cycles.Last().cycleRuns)
            //    {
            //        Debug.WriteLine($"Current testname: {c.test.name} Name at run time: {c.run.test.name} Outcome: {c.run.result.ToString()}");
            //    }
            //}
        }


    } // MainViewModel
}

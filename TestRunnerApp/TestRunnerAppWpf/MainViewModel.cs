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
using TestRunnerLib.Jira;

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

            Settings.GetSettings(this);
            CheckWebDriverAvailibility();

            this.PropertyChanged += MainViewModel_PropertyChanged;

        }

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "unsavedChanges")
                Debug.WriteLine("unsavedChanges changed to: " + unsavedChanges.ToString());
        }
        /* Events */
        public void GridViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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
                Debug.WriteLine( t.Item2);
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
            }
            this.unsavedChanges = true;
            Debug.WriteLine($"Async runTestWorker result: {e.Result}");
        }

        public void LoadJiraProjectAsync(JiraProject p)
        {
            StartAsyncLoader(p);
        }


        /* Load project data */ // Should be moved to lib? Has view updates...
        private void CancelLoadJob() => projectLoadWorker.CancelAsync();

        private void StartAsyncLoader(JiraProject p)
        {
            enableProjectLoad = false;
            progressBarValue = 0;
            runStatus = "Loading";
            runTotal = "11"; //Steps in load
            runSlash = "/";
            runCurrent = "1";

            projectLoadWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            projectLoadWorker.DoWork += projectLoadWorker_DoWork;
            projectLoadWorker.ProgressChanged += projectLoadWorker_ProgressChanged;
            projectLoadWorker.RunWorkerCompleted += projectLoadWorker_RunWorkerCompleted;

            syncContext = SynchronizationContext.Current;
            projectLoadWorker.RunWorkerAsync(p);
        }

        void projectLoadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            JiraProject p = (JiraProject)e.Argument;
            string maxResults = "100";
            JiraLoad load = new JiraLoad();
            int done = 0;
            int total = 11;

            //syncContext.Send(x => test.previousOutcome = r.result, null);

            load.LoadProjectData(p);
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.folders = load.LoadFolders(p.key, null, maxResults).Result;
            p.separateFolders();
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);


            p.versions = load.LoadVersions(p.key).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.components = load.LoadComponents(p.key).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            


            p.prios = load.LoadPrios(p.key, maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.environments = load.LoadEnvirons(p.key, maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            // Project statuses
            p.caseStatuses = load.LoadStatus(p.key, "TEST_CASE", maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.cycleStatuses = load.LoadStatus(p.key, "TEST_CYCLE", maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.executionStatuses = load.LoadStatus(p.key, "TEST_EXECUTION", maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);
            //p.planStatuses = await LoadStatus(p.key, "TEST_PLAN", maxResults);

            p.cycles = load.LoadCycles(p, p.key, null, maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.cases = load.LoadCases(p, p.key, null, maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);



        }

        void projectLoadUpdate(object sender, DoWorkEventArgs e, int done, int total)
        {
            if (projectLoadWorker.CancellationPending == true)
            {
                syncContext.Send(x => runStatus = "Cancelled", null);
                e.Cancel = true;
                return;
            }
            int progressPercentage = Convert.ToInt32(((double)done / total) * 100);
            Tuple<int, string> report = new Tuple<int, string>(done, $"Parts loaded: {done}");
            (sender as BackgroundWorker).ReportProgress(progressPercentage, report);
        }

        void projectLoadWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
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

        void projectLoadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            enableProjectLoad = true;
            if (runStatus != "Cancelled")
            {
                runStatus = "Done";
                runCurrent = runTotal;
            }
            //this.unsavedChanges = true;
            Debug.WriteLine($"Async projectLoadWorker result: {e.Result}");
        }




    } // MainViewModel
}

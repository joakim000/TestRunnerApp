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
            runStatus = "Idle";
            runTotal = string.Empty;
            runCurrent = string.Empty;
            runSlash = string.Empty;
            topMost = false;
            enableRun = true;

            
            this.unsavedChanges = false;

            //GetSettings();
            Settings.GetSettings(this);
            CheckWebDriverAvailibility();
        }

        public  void GridViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "suite")
            {
                gridViewModel.suite.PropertyChanged += Suite_PropertyChanged;
                gridViewModel.suite.tests.CollectionChanged += Tests_CollectionChanged;
                setWindowTitle();
            }
        }
        internal void Tests_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            this.unsavedChanges = true;

        internal void Suite_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.unsavedChanges = true;
            if (e.PropertyName == "name")
                setWindowTitle();
        }

        private void setWindowTitle() => 
            windowTitle = string.IsNullOrWhiteSpace(gridViewModel.suite.name) ? Properties.Settings.Default.AppTitle : $"{gridViewModel.suite.name}  - {Properties.Settings.Default.AppTitle}";

        //private void GetSettings()
        //{
        //    try
        //    {
        //        // Get WebDriver-setting
        //        switch (Settings.GetWebDriverType())
        //        {
        //            case WebDriverType.Chrome:
        //                Execute_SetChromeCmd();
        //                break;
        //            case WebDriverType.Firefox:
        //                Execute_SetFirefoxCmd();
        //                break;
        //            case WebDriverType.IE:
        //                Execute_SetIECmd();
        //                break;
        //            default:
        //                Execute_SetChromeCmd();
        //                break;
        //        }

        //        // Get on-top-when-running setting
        //        checkedOnTop = Properties.Settings.Default.OnTop;

        //        switch (Properties.Settings.Default.Theme)
        //        {
        //            case "Light":
        //                Themes.SetLight();
        //                checkedDarkTheme = false;
        //                break;
        //            case "Dark":
        //                Themes.SetDark();
        //                checkedDarkTheme = true;
        //                break;
        //         default:
        //                Themes.SetLight();
        //                checkedDarkTheme = false;
        //                break;
        //        }
        //    }
        //    catch (NullReferenceException e)
        //    {
        //        // In case of problems with settings-file
        //        Debug.WriteLine($"Null-ref getting settings: {e.Message}");
        //    }
        //}


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

        public void SelectedItems_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (gridViewModel.selectedItems.selectedItems.Count() == 1)
            {
                detailsViewModel.suiteDetailsVisi = false;
                detailsViewModel.testDetailsVisi = true;
                detailsViewModel.test = gridViewModel.selectedItems.selectedItems[0];
            }
            else
            {
                detailsViewModel.testDetailsVisi = false;
                detailsViewModel.suiteDetailsVisi = true;
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

        public void CopySelectedToNew()  // Should be method in TestModel
        {
            if (gridViewModel.selectedItems.selectedItems.Count() == 1)
            {
                var t = gridViewModel.selectedItems.selectedItems[0];
                var c = t.DeepCopy();
                gridViewModel.suite.tests.Add(c);
                this.unsavedChanges = true;
            }
        }


        /* Legacy sync running 
        public void RunAll()
        {
            foreach (TestModel test in gridViewModel.suite.tests)
            {
                Debug.WriteLine($"Running {test.name}");

                RunModel r = new RunModel(test);
                test.previousOutcome = r.result;
                test.runs.Add(r);
                test.numberOfRuns = test.runs.Count();
            }
        }

        public void RunSelected(ObservableCollection<TestModel> tests)
        {
            //Debug.WriteLine("Running selected tests");
            //if (gridViewModel.selectedItems.selectedItems == null)
            //    Debug.WriteLine("selectedItems is null");
            //else if (gridViewModel.selectedItems.selectedItems.Count() == 0)
            //    Debug.WriteLine("selectedItems count is 0");
            //else
            //{
            //    foreach (TestModel test in gridViewModel.selectedItems.selectedItems)
            //    {
            //        Debug.WriteLine($"{test.name} Runs:{test.runs.Count()} Last outcome:{test.previousOutcome}");
            //    }
            //}

            foreach (TestModel test in tests)
            {
                Debug.WriteLine($"Running {test.name}");

                RunModel r = new RunModel(test);
                test.previousOutcome = r.result;
                test.runs.Add(r);
                test.numberOfRuns = test.runs.Count();
            }
        }
        */

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

        //private WebDriverType GetWebDriverType()
        //{
        //    WebDriverType driver = WebDriverType.None;
        //    try
        //    {
        //        switch (Properties.Settings.Default.WebDriver)
        //        {
        //            case "chrome":
        //                driver = WebDriverType.Chrome;
        //                break;
        //            case "firefox":
        //                driver = WebDriverType.Firefox;
        //                break;
        //            case "ie":
        //                driver = WebDriverType.IE;
        //                break;
        //            default:
        //                driver = WebDriverType.None;
        //                break;
        //        }
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        // In case of problems with settings-file
        //        Debug.WriteLine($"Null-ref getting webdriversetting: {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"General exception getting webdriversetting:: {ex.Message}");
        //    }
        //    Debug.WriteLine($"GetWebDriverType returned: {driver.ToString()}");

        //    return driver;
        //}


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

            CycleModel cycle = new CycleModel();
            syncContext.Send(x => gridViewModel.suite.cycles.Add(cycle), null);

            foreach (TestModel test in tests)
            {
                Debug.WriteLine($"Running {test.name}");

                RunModel r = new RunModel(test, webDriverType);
                syncContext.Send(x => test.runs.Add(r), null);
                syncContext.Send(x => test.previousOutcome = r.result, null);
                syncContext.Send(x => test.numberOfRuns = test.runs.Count(), null);
                //test.previousOutcome = r.result;
                //test.numberOfRuns = test.runs.Count();

                CycleRun cycleRun = new CycleRun(test, r);
                syncContext.Send(x => cycle.cycleRuns.Add(cycleRun), null);

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

            Debug.WriteLine($"Cycles in suite: {gridViewModel.suite.cycles.Count()}");
            if (gridViewModel.suite.cycles.Count() > 0)
            {


                Debug.WriteLine($"Testruns in first cycle: {gridViewModel.suite.cycles.First().cycleRuns.Count()}");
                foreach (CycleRun c in gridViewModel.suite.cycles.First().cycleRuns)
                {
                    Debug.WriteLine($"Current testname: {c.test.name} Name at run time: {c.run.test.name} Outcome: {c.run.result.ToString()}");
                }
                Debug.WriteLine($"Testruns in previous cycle: {gridViewModel.suite.cycles.Last().cycleRuns.Count()}");
                foreach (CycleRun c in gridViewModel.suite.cycles.Last().cycleRuns)
                {
                    Debug.WriteLine($"Current testname: {c.test.name} Name at run time: {c.run.test.name} Outcome: {c.run.result.ToString()}");
                }
            }
        }


    } // MainViewModel
}

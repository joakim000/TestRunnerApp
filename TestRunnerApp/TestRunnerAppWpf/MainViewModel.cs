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


            runStatus = "Ready";
            runTotal = string.Empty;
            runCurrent = string.Empty;
            runSlash = string.Empty;
            topMost = false;
            enableRun = true;

            Settings.GetSettings(this);
            CheckWebDriverAvailibility();

            //JiraSetup();

            this.PropertyChanged += MainViewModel_PropertyChanged;


            //ContinueSession();
        }

        public async void JiraSetup()
        {
            JiraConnect jc = new JiraConnect(this);
            JiraConnectInfo jiraCloud = await jc.Preflight();
            JiraConnectInfo tmjCloud = await jc.TmjPrep();

            //if (!jiraCloud.ready || !tmjCloud.ready)
            //    if (CanExecute_MgmtSettingsCmd())
            //        Execute_MgmtSettingsCmd();
            //else
            //    //jira = new Jira(await jc.Preflight(), await jc.TmjPrep());
            //    jira = new Jira(jiraCloud, tmjCloud);

            if (jiraCloud.ready && tmjCloud.ready)
                jira = new Jira(jiraCloud, tmjCloud);

            else
            {
                if (CanExecute_MgmtSettingsCmd())
                    Execute_MgmtSettingsCmd();
            }


        }

        public void ToggleJiraCaseUpdates(bool toggleOn)
        {
            if (toggleOn)
            {
                foreach (TestModel t in gridViewModel.suite.tests)
                {
                    if (t.jiraCase != null)
                    {
                        t.jiraCase.PropertyChanged -= t.jiraCase.JiraCase_PropertyChanged;
                        t.jiraCase.PropertyChanged += t.jiraCase.JiraCase_PropertyChanged;
                    }
                }
            }
            else
            {
                foreach (TestModel t in gridViewModel.suite.tests)
                {
                    if (t.jiraCase != null)
                        t.jiraCase.PropertyChanged -= t.jiraCase.JiraCase_PropertyChanged;
                }
            }
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
            {
                //Debug.WriteLine("selectedItems count is 0" + " - sender: " + sender.ToString());
            }
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


        public void ContinueSession()
        {
            // Continue previous session, if not opened by association
            if (string.IsNullOrEmpty(gridViewModel.suite.filename) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousDir) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousFile))
            {
                if (!Properties.Settings.Default.LoadFailure)
                {
                    Properties.Settings.Default.LoadFailure = true;

                    string fileToOpen = Properties.Settings.Default.PreviousDir + @"\" +
                        Properties.Settings.Default.PreviousFile;

                    TestRunnerLib.Log.AddNoWrite($"Continue session from {fileToOpen}");
                    LoadData.OpenFileSetup(fileToOpen, this);

                    Properties.Settings.Default.LoadFailure = false;
                }
            }
        }

        private void setWindowTitle()
        {
            string s = string.Empty;
            if (!string.IsNullOrWhiteSpace(gridViewModel.suite.name))
                s += gridViewModel.suite.name;
            else
                s += "Untitled suite";
            s += " ";
            if (!string.IsNullOrWhiteSpace(gridViewModel.suite.filename))
                s += $"[{gridViewModel.suite.filename}] ";
            s += $"- {Properties.Settings.Default.AppTitle}";

            windowTitle = s;

        }

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

       

        
        


    } // MainViewModel
}

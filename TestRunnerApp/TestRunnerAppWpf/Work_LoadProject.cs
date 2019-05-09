using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TestRunnerLib;
using TestRunnerLib.Jira;

using ViewModelSupport;

namespace TestRunnerAppWpf
{
    public partial class MainViewModel : ViewModelBase
    {
        public void LoadJiraProjectAsync(JiraProject p)
        {
            if (jira != null)
                StartAsyncLoader(p);
            else
            {
                Debug.WriteLine("StartAsyncLoader: jira was null");
                JiraSetup();
            }
        }


        /* Load project data */ // Should be moved to lib? Has view updates...
        private void CancelLoadJob() => projectLoadWorker.CancelAsync();

        private void StartAsyncLoader(JiraProject p)
        {
            // Disable stuff while working
            enableMenus = false;
            enableProjectLoad = false;
            ToggleJiraCaseUpdates(false);

            // Setup status bar
            progressBarValue = 0;
            runStatus = "Loading project";
            runTotal = string.Empty; runSlash = string.Empty; runCurrent = string.Empty;
#if DEBUG
            //runTotal = "11"; //Steps in load
            //runSlash = "/";
            //runCurrent = "1";
#endif


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
            //JiraLoad load = new JiraLoad();
            int done = 0;
            int total = 11;

            //syncContext.Send(x => test.previousOutcome = r.result, null);

            jira.load.LoadProjectData(p);
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.versions = jira.load.LoadVersions(p.key).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.components = jira.load.LoadComponents(p.key).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);


            p.folders = jira.load.LoadFolders(p.key, null, maxResults).Result;
            p.separateFolders();
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.prios = jira.load.LoadPrios(p.key, maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            //p.instancePrios = jira.load.LoadPriosJira().Result;
            //done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.environments = jira.load.LoadEnvirons(p.key, maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            // Project statuses
            p.caseStatuses = jira.load.LoadStatus(p.key, "TEST_CASE", maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.cycleStatuses = jira.load.LoadStatus(p.key, "TEST_CYCLE", maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.executionStatuses = jira.load.LoadStatus(p.key, "TEST_EXECUTION", maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            //p.planStatuses = = jira.load.LoadStatus(p.key, "TEST_PLAN", maxResults).Result;

            p.cycles = jira.load.LoadCycles(p, p.key, null, maxResults).Result;
            done++; e.Result = done; projectLoadUpdate(sender, e, done, total);

            p.cases = jira.load.LoadCases(p, p.key, null, maxResults).Result;
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
#if DEBUG
                var t = (Tuple<int, string>)e.UserState;
                //Debug.WriteLine(t.Item2);
                //runCurrent = (t.Item1 + 1).ToString();
#endif
            }
        }

        void projectLoadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (runStatus != "Cancelled")
            {
                runStatus = "Done";
                runCurrent = runTotal;
                ResetProgress();


                // Set SelectedItems on tests
                foreach (TestModel t in gridViewModel.suite.tests)
                {
                    t.SetSelectedItems(gridViewModel.suite.jiraProject);
                }

            }
            Debug.WriteLine($"Async projectLoadWorker result: {e.Result}");

            UpdateJiraCasesInTests();

            // Reenable stuff after work done
            enableProjectLoad = true;
            ToggleJiraCaseUpdates(true);
            enableMenus = true;

        }

        async void ResetProgress()
        {
            await Task.Delay(10000); // Wait before resetting
            runStatus = "Ready";
            runCurrent = string.Empty;
            runSlash = string.Empty;
            runTotal = string.Empty;
            progressBarValue = 0;
        }

        void UpdateJiraCasesInTests()
        {
            var s = gridViewModel.suite;
            foreach (TestModel t in s.tests)
            {
                if (t.jiraCase != null && t.jiraCase.project != null)
                {
                    if (s.jiraProject.cases.Where(x => x.id == t.jiraCase.id).Count() > 0)
                        t.jiraCase = s.jiraProject.cases.Where(x => x.id == t.jiraCase.id).First();
                }
            }
        }



    }
}

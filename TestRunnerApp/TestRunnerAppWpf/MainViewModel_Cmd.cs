using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelSupport;
using TestRunnerLib;
using TestRunnerLib.Jira;
using System.Reflection;

using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using System.Windows.Input;

namespace TestRunnerAppWpf
{
    public partial class MainViewModel : ViewModelBase
    {
        // Settings commands
        public void Execute_SetChromeCmd()
        {
            checkedChrome = true; checkedFirefox = false; checkedIE = false; checkedPhantomJS = false; checkedEdge = false;
            Properties.Settings.Default.WebDriver = "chrome";
            Properties.Settings.Default.Save();
        }
        public bool CanExecute_SetChromeCmd()
        {
            return true;
        }

        public void Execute_SetFirefoxCmd()
        {
            checkedChrome = false; checkedFirefox = true; checkedIE = false; checkedPhantomJS = false; checkedEdge = false;
            Properties.Settings.Default.WebDriver = "firefox";
            Properties.Settings.Default.Save();
        }
        public bool CanExecute_SetFirefoxCmd() 
        {
            return true;
        }

        public void Execute_SetIECmd()
        {
            checkedChrome = false; checkedFirefox = false; checkedIE = true; checkedPhantomJS = false; checkedEdge = false;
            Properties.Settings.Default.WebDriver = "ie";
            Properties.Settings.Default.Save();
        }
        public bool CanExecute_SetIECmd()
        {
            return true;
        }

        public void Execute_SetPhantomJSCmd()
        {
            checkedChrome = false; checkedFirefox = false; checkedIE = false; checkedPhantomJS = true; checkedEdge = false;
            Properties.Settings.Default.WebDriver = "phantomjs";
            Properties.Settings.Default.Save();
        }
        public bool CanExecute_SetPhantomJSCmd()
        {
            return true;
        }

        public void Execute_SetEdgeCmd()
        {
            checkedChrome = false; checkedFirefox = false; checkedIE = false; checkedPhantomJS = false; checkedEdge = true;
            Properties.Settings.Default.WebDriver = "edge";
            Properties.Settings.Default.Save();
        }
        public bool CanExecute_SetEdgeCmd()
        {
            return true;
        }

        public void Execute_SetThemeCmd()
        {
            if (!checkedDarkTheme)
            {
                Themes.SetLight();
            }
            else
            {
                Themes.SetDark();
            }
            Debug.WriteLine("Saving user-settings");
            Properties.Settings.Default.Save();
        }
        public bool CanExecute_SetThemeCmd()
        {
            return true;
        }


        public void Execute_SetCycleViewCmd()
        {
            singleCycleView = !multiCycleView;
        }
        public bool CanExecute_SetCycleViewCmd()
        {
            return true;
        }

       
        public void Execute_SetOnTopCmd()
        {
            Properties.Settings.Default.OnTop = !Properties.Settings.Default.OnTop;
            checkedOnTop = Properties.Settings.Default.OnTop;
            Properties.Settings.Default.Save();
        }
        public bool CanExecute_SetOnTopCmd()
        {
            return true;
        }

        // Help commands
        public void Execute_HelpCmd()
        {
            Debug.WriteLine("HelpCmd not implemented");
        }
        public bool CanExecute_HelpCmd()
        {
            return true;
        }

        public void Execute_AboutCmd()
        {
            string versionString = Properties.Settings.Default.versionString;
            string aboutString = $"TestApp v{versionString}{Environment.NewLine}{Environment.NewLine}Joakim Odermalm{Environment.NewLine}Unicus Sverige{Environment.NewLine}2019";
            MessageBox.Show(aboutString);
        }
        public bool CanExecute_AboutCmd()
        {
            return true;
        }

        public void Execute_HowToCmd()
        {
            Window HowToWindow = new HowToWindow();
            HowToWindow.Show();
        }
        public bool CanExecute_HowToCmd()
        {
            return true;
        }

        // Suite management commands
        private bool unsavedChangesHelper()
        {
            if (this.unsavedChanges)
            {
                var result = MessageBox.Show("Save current suite?", "TestApp",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Execute_FileSaveCmd();
                        return true;
                    case MessageBoxResult.No:
                        return true;
                    case MessageBoxResult.Cancel:
                        return false;
                    default:
                        return false;
                }
            }
            else
                return true;
        }

        public void Execute_NewSuiteCmd()
        {
            if (unsavedChangesHelper())
            {
                var d = new NewSuiteDialog();
                if (d.ShowDialog() == true)
                {
                    undoSuite = FileMgmt.Serialize(gridViewModel.suite);

                    if (string.IsNullOrEmpty(d.newItem.name))
                        d.newItem.name = "Untitled suite";
                    gridViewModel.suite = d.newItem;

                    SelectedItems_PropertyChanged(null, null);
                    FileMgmt.filename = null;

                }
            }
        }
        public bool CanExecute_NewSuiteCmd()
        {
            return true;
        }

        public void Execute_FileOpenCmd()
        {
            if (unsavedChangesHelper())
            {
                string fileToOpen = FileMgmt.OpenSuiteFrom();
                if (fileToOpen != null)
                    FileMgmt.OpenFileSetup(fileToOpen, this);

            }
        }
        public bool CanExecute_FileOpenCmd()
        {
            return true;
        }

        public void Execute_FileSaveAsCmd()
        {
            Tuple<bool, string> saveResult = FileMgmt.SaveAsSuite(gridViewModel.suite);
            if (saveResult.Item1)
            {
                gridViewModel.suite.filename = saveResult.Item2;
                this.unsavedChanges = false;
            }
        }
        public bool CanExecute_FileSaveAsCmd()
        {
            return true;
        }

        public void Execute_FileSaveCmd()
        {
            Tuple<bool, string> saveResult = FileMgmt.SaveSuite(gridViewModel.suite);
            if (saveResult.Item1)
            {
                gridViewModel.suite.filename = saveResult.Item2;
                this.unsavedChanges = false;
            }
        }
        public bool CanExecute_FileSaveCmd()
        {
            return true;
        }

        public void Execute_ImportLibCmd()
        {
            FileMgmt.CopyTestLibraryFrom();
        }
        public bool CanExecute_ImportLibCmd()
        {
            return true;
        }

        public void Execute_OpenLibDirCmd()
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Tests");
        }
        public bool CanExecute_OpenLibDirCmd()
        {
            return true;
        }

        /* Deprecated */
        public void Execute_SuiteDetailsCmd()
        {
            detailsViewModel.testDetailsVisi = false;
            detailsViewModel.suiteDetailsVisi = true;
            detailsViewModel.suite = gridViewModel.suite;
        }
        public bool CanExecute_SuiteDetailsCmd()
        {
            return true;
        }
        /* Deprecated */

        public void Execute_UndoCmd()
        {
            if (undoSuite != null)
            {
                redoSuite = string.Copy(undoSuite);
                SuiteModel restoredSuite = (SuiteModel)FileMgmt.DeserialSuite(undoSuite);
                if (restoredSuite != null)
                    gridViewModel.suite = restoredSuite;
                    //TODO BUG: Restore selectedItems
            }
        }
        public bool CanExecute_UndoCmd()
        {
            return true;
        }

        public void Execute_RedoCmd()
        {
            if (redoSuite != null)
            {
                SuiteModel restoredSuite = (SuiteModel)FileMgmt.DeserialSuite(redoSuite);
                if (restoredSuite != null)
                    gridViewModel.suite = restoredSuite;
            }
        }
        public bool CanExecute_RedoCmd()
        {
            return true;
        }

        // Cycle commands
        public void Execute_NewCycleCmd()
        {
            var d = new NewCycleDialog(this);
            if (d.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(d.viewModel.newItem.id) && !string.IsNullOrEmpty(d.viewModel.newItem.name))
                {

                    undoSuite = FileMgmt.Serialize(gridViewModel.suite);

                    // Don't readd if newItem actuallty is existing (edited) item
                    if (!gridViewModel.suite.cycles.Contains(d.viewModel.newItem))
                        gridViewModel.suite.cycles.Add(d.viewModel.newItem);
                    gridViewModel.suite.currentCycle = gridViewModel.suite.cycles.Where(x => x.uid == d.viewModel.newItem.uid).First();

                    unsavedChanges = true;
                }
            }
            
        }
        public bool CanExecute_NewCycleCmd()
        {
            return true;
        }

        public void Execute_DiscardCyclesCmd()
        {
            if (multiCycleView) { 
                 Debug.WriteLine("Selected cycles:" + Environment.NewLine);
                 foreach (CycleModel c in detailsViewModel.selectedCycleItems)
                     Debug.WriteLine(c.id + " " + c.name + Environment.NewLine);

                 if (detailsViewModel.selectedCycleItems != null)
                 {
                     undoSuite = FileMgmt.Serialize(gridViewModel.suite);
                     foreach (CycleModel c in detailsViewModel.selectedCycleItems)
                         gridViewModel.suite.cycles.Remove(c);
                     unsavedChanges = true;
                 }
            }

            else
            {
                if (gridViewModel.suite.currentCycle != null)
                {
                    Debug.WriteLine("Discarding cycle - current cycle:" + gridViewModel.suite.currentCycle.id);
                    undoSuite = FileMgmt.Serialize(gridViewModel.suite);
                    //gridViewModel.suite.cycles.Remove(currentCycle);
                    gridViewModel.suite.cycles.Remove(gridViewModel.suite.cycles.Where(x => x.uid == gridViewModel.suite.currentCycle.uid).First());
                    unsavedChanges = true;
                    if (gridViewModel.suite.cycles.Count > 0)
                        gridViewModel.suite.currentCycle = gridViewModel.suite.cycles.First();

                    if (gridViewModel.suite.currentCycle != null)
                        Debug.WriteLine("Cycle was discarded - new current cycle:" + gridViewModel.suite.currentCycle.id);
                    else
                        Debug.WriteLine("Cycle was discarded - no new current cycle.");
                }
                else 
                    Debug.WriteLine("Discarding cycle - no current cycle.");
            }


        }
        public bool CanExecute_DiscardCyclesCmd()
        {
            return true;
        }

        public void Execute_EditCycleCmd()
        {

            MessageBox.Show("Upcoming feature for v2.1", "TestRunnerApp with Jira",
                MessageBoxButton.OK, MessageBoxImage.Information);

            //var d = new EditCycleDialog(this);
            //if (d.ShowDialog() == true)
            //{
            //    if (!string.IsNullOrEmpty(d.viewModel.newItem.id) && !string.IsNullOrEmpty(d.viewModel.newItem.name))
            //    {

            //        undoSuite = FileMgmt.Serialize(gridViewModel.suite);

            //        // Don't readd if newItem actuallty is existing (edited) item
            //        if (!gridViewModel.suite.cycles.Contains(d.viewModel.newItem))
            //            gridViewModel.suite.cycles.Add(d.viewModel.newItem);
            //        gridViewModel.suite.currentCycle = gridViewModel.suite.cycles.Where(x => x.uid == d.viewModel.newItem.uid).First();

            //        unsavedChanges = true;
            //    }
            //}
        }
        public bool CanExecute_EditCycleCmd()
        {
            return true;
        }

        public void Execute_ImportCyclesCmd()
        {
            
        }
        public bool CanExecute_ImportCyclesCmd()
        {
            return true;
        }

        public void Execute_SelectCycleCmd(object parameter)
        {
            var cmdParam = parameter as CycleModel;
            if (cmdParam != null)
                gridViewModel.suite.currentCycle = cmdParam;
        }
        public bool CanExecute_SelectCycleCmd()
        {
            return true;
        }


        public void Execute_ResetExportedFlagsCmd()
        {
            var c = gridViewModel.suite.currentCycle;
            foreach (CycleRun cr in c.cycleRuns)
            {
                cr.exported = false;
            }
        }
        public bool CanExecute_ResetExportedFlagsCmd()
        {
            return true;
        }





        // Test execution commands
        public void Execute_RunSelectedCmd()
        {
            if (gridViewModel.selectedItems.selectedItems != null || gridViewModel.selectedItems.selectedItems.Count() > 0)
            {
                RunSelectedAsync(gridViewModel.selectedItems.selectedItems);
            }
            Debug.WriteLine($"Cycles in suite: {gridViewModel.suite.cycles.Count()}");
            if (gridViewModel.suite.cycles.Count() > 0)
                Debug.WriteLine($"Testruns in previous cycle: {gridViewModel.suite.cycles.Last().cycleRuns.Count()}");
          
        }
        public bool CanExecute_RunSelectedCmd()
        {
            return true;
        }

        public void Execute_RunAllCmd()
        {
            RunAllAsync();
        }
        public bool CanExecute_RunAllCmd()
        {
            return true;
        }

        public void Execute_PauseCmd()
        {
            Debug.WriteLine("PauseCmd not implemented");
        }
        public bool CanExecute_PauseCmd()
        {
            return true;
        }

        public void Execute_StopCmd()
        {
            CancelRunJob();
        }
        public bool CanExecute_StopCmd()
        {
            return true;
        }

        // Test management commands
        public void Execute_NewTestCmd()
        {
            var d = new NewTestDialog(this);
            if (d.ShowDialog() == true)
            {
                undoSuite = FileMgmt.Serialize(gridViewModel.suite);
                gridViewModel.suite.tests.Add(d.viewModel.newItem);
                unsavedChanges = true;
            }
        }
        public bool CanExecute_NewTestCmd()
        {
            return true;
        }

        public void Execute_CopyTestCmd()
        {
            if (gridViewModel.selectedItems.selectedItems.Count() == 1)
            {
                undoSuite = FileMgmt.Serialize(gridViewModel.suite);
                var t = gridViewModel.selectedItems.selectedItems[0];
                var c = t.DeepCopy();
                gridViewModel.suite.tests.Add(c);
                this.unsavedChanges = true;
            }
        }
        public bool CanExecute_CopyTestCmd()
        {
            return true;
        }

        public void Execute_RemoveTestCmd()
        {
            if (gridViewModel.selectedItems.selectedItems != null)
            {
                undoSuite = FileMgmt.Serialize(gridViewModel.suite);
                foreach (TestModel test in gridViewModel.selectedItems.selectedItems )
                    gridViewModel.suite.tests.Remove(test);
                unsavedChanges = true;
            }
        }
        public bool CanExecute_RemoveTestCmd()
        {
            return true;
        }

        public void Execute_DiscardRunsCmd()
        {
            Debug.WriteLine("Selected runs:" + Environment.NewLine);
            foreach (RunModel run in detailsViewModel.selectedItems)
                Debug.WriteLine(run.datetime + run.result.ToString() + Environment.NewLine);

            if (detailsViewModel.selectedItems != null)
            undoSuite = FileMgmt.Serialize(gridViewModel.suite);
            {
                foreach (RunModel run in detailsViewModel.selectedItems )
                    detailsViewModel.test.runs.Remove(run);
                unsavedChanges = true;
            }
        }
        public bool CanExecute_DiscardRunsCmd()
        {
            return true;
        }


       


        // Development & debugging commands
        public void Execute_Debug1Cmd()
        {
            //gridViewModel.suite = FileMgmt.LoadTestingSuite();
            //SelectedItems_PropertyChanged(null, null);

            //var ass = Assembly.GetAssembly(typeof(TestModel));
            //Type t = Type.GetType($"TestRunnerLib.TestModel,NetOp.TestRunnerLib");
            //Debug.WriteLine($"Ass: {ass}  Type: {t}");

            Debug.WriteLine(AppDomain.CurrentDomain.BaseDirectory);


            //FileMgmt.CopyTestLibraryFrom();


        }
        public bool CanExecute_Debug1Cmd()
        {
            return true;
        }

        public void Execute_Debug2Cmd()
        {
            Debug.WriteLine($"Results from {gridViewModel.suite.tests.Count()} tests.");
            foreach (TestModel test in gridViewModel.suite.tests)
            {

                Debug.WriteLine($"{test.name} Runs:{test.runs.Count()} Last outcome:{test.previousOutcome}");
            }
        }
        public bool CanExecute_Debug2Cmd()
        {
            return true;
        }

        public void Execute_Debug3Cmd()
        {
            var asses = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly a in asses)
            {
                Debug.WriteLine($"============");
                Debug.WriteLine($"Assembly: {a}");
                //Debug.WriteLine($"{a.CodeBase}");
                //Debug.WriteLine($"{a.CustomAttributes}");
                //Debug.WriteLine($"{a.DefinedTypes}");
                //Debug.WriteLine($"{a.EntryPoint}");
                //Debug.WriteLine($"{a.EscapedCodeBase}");
                //Debug.WriteLine($"{a.Evidence}");
                //Debug.WriteLine($"{a.ExportedTypes}");
                Debug.WriteLine($"{a.FullName}");
                //Debug.WriteLine($"{a.GetName()}");
            }
        }
        public bool CanExecute_Debug3Cmd()
        {
            return true;
        }


        /* Reporting */
        
        //public async void Execute_Report1Cmd()
        //{
        //    var j = new Jira();

        //    // Serverinfo
        //    //Task<Tuple<HttpStatusCode, JObject>> t = j.JiraCall(HttpMethod.Get, "serverInfo", null);

        //    // Get test case
        //    Task<Tuple<HttpStatusCode, JObject>> t = j.TmjCall(HttpMethod.Get, "testcases", null);


        //    await t;
        //}

        public  void Execute_Report1Cmd()
        {
            // Serverinfo
            //Tuple<HttpStatusCode, JObject> r = await Jira.GetServerInfo();

            // Current user
            //Tuple<HttpStatusCode, JObject> r = await Jira.CurrentUser(await JiraConnect.Preflight());

            ////Tuple<HttpStatusCode, JObject> r = await Jira.GetFolders("JOAK", "TEST_CASE", null);
            ////Tuple<HttpStatusCode, JObject> r = await Jira.GetPrios("JOAK", null);      
            ////Tuple<HttpStatusCode, JObject> r = await Jira.GetStatuses(null, "TEST_EXECUTION", "100");
            ////Tuple<HttpStatusCode, JObject> r = await Jira.GetEnvirons(null, null);
            ////Tuple<HttpStatusCode, JObject> r = await Jira.GetProj("5301");
            ////Tuple<HttpStatusCode, JObject> r = await Jira.GetCycles("UT", null, null);



            //Debug.WriteLine(r.Item2);

            //if (r.Item2.TryGetValue("accountId", out JToken accountId)) {
            //    Debug.WriteLine(accountId.ToString());
            //}
            //else
            //    Debug.WriteLine("accountId not found");

        }
        public bool CanExecute_Report1Cmd()
        {
            return true;
        }


        public  void Execute_Report2Cmd()
        {
            //Tuple<HttpStatusCode, JObject> r = await Jira.CreateCycle("TEM", "First cycle", "My very first cycle", null, false);

            //Tuple<HttpStatusCode, JObject> user = await Jira.CurrentUser(await JiraConnect.Preflight());
            //string accountId = null;
            //if (user.Item2.TryGetValue("accountId", out JToken accountIdToken))
            //    accountId = accountIdToken.ToString();


            //CycleModel c = gridViewModel.suite.cycles.Last();
            //foreach (CycleRun cr in c.cycleRuns)
            //{
            //    RunModel r = cr.run;
            //    Tuple<HttpStatusCode, JObject> response = await Jira.CreateExec(await JiraConnect.Preflight(),
            //                                                                    "TEM",
            //                                                                    "TEM-R2",
            //                                                                    r.test.id,
            //                                                                    r.result,
            //                                                                    r.webDriverType,
            //                                                                    r.datetimeEnd, 
            //                                                                    r.runTime,
            //                                                                    Properties.Settings.Default.JiraAccountId, 
            //                                                                    r.resultObj.message);
            //    Debug.WriteLine(response.Item2);
            //}

            //Tuple<HttpStatusCode, JObject> r = await Jira.CreateExec("TEM", "First cycle", "My very first cycle", null, false);
            //Debug.WriteLine(r.Item2);
        }
        public bool CanExecute_Report2Cmd()
        {
            return true;
        }


        


        /* Integration */
        public async void Execute_MgmtSettingsCmd()
        {
            var d = new SettingsManager();
            if (d.ShowDialog() == true)
            {
                jiraInstance = Properties.Settings.Default.JiraInstance;
                jiraUser = Properties.Settings.Default.JiraUser;
                jiraToken = Properties.Settings.Default.JiraToken;
                tmjIdToken = Properties.Settings.Default.TmjIdToken;
                tmjKeyToken = Properties.Settings.Default.TmjKeyToken;
                Debug.WriteLine("Saving user-settings");
                Properties.Settings.Default.Save();

                

                // Check if JiraCloudTmj should be enabled
                bool JiraCloudTmj_enabled = true;
                if (string.IsNullOrEmpty(Properties.Settings.Default.JiraInstance))
                    JiraCloudTmj_enabled = false;
                if (string.IsNullOrEmpty(Properties.Settings.Default.JiraUser))
                    JiraCloudTmj_enabled = false;
                //if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraAccountId))
                //{
                //    caller.jiraAccountId = Properties.Settings.Default.JiraAccountId;
                //    Settings.accountIdSet = true;
                //}
                // else JiraCloudTmj_enabled = false; // Enable anyway and try to get accountid in preflight
                if (string.IsNullOrEmpty(Properties.Settings.Default.JiraToken))
                    JiraCloudTmj_enabled = false;
                //if (string.IsNullOrEmpty(Properties.Settings.Default.TmjIdToken))
                //    JiraCloudTmj_enabled = false;
                if (string.IsNullOrEmpty(Properties.Settings.Default.TmjKeyToken))
                    JiraCloudTmj_enabled = false;
                Enums.Mgmt.Find(x => x.key == "JiraCloudTmj").enabled = JiraCloudTmj_enabled;


                /* Deprecated */
                if (Properties.Settings.Default.MgmtSystem == "None")
                {
                    jiraCloudMgmt = false;
                    reqTestMgmt = false;
                }
                else if (Properties.Settings.Default.MgmtSystem == "Jira Cloud with TM4J")
                {
                    jiraCloudMgmt = true;
                    reqTestMgmt = false;
                    JiraConnect jc = new JiraConnect(this);
                    if (!await jc.SetAccountId())
                        MessageBox.Show("Error retrieving Account ID.", "TestAppRunner with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (Properties.Settings.Default.MgmtSystem == "ReqTest")
                {
                    jiraCloudMgmt = false;
                    reqTestMgmt = true;
                }
                /* Deprecated */


            }
        }
        public bool CanExecute_MgmtSettingsCmd()
        {
            return true;
        }


        public async void Execute_ExportCycleCmd()
        {
            if (gridViewModel.suite.jiraProject == null)
            {
                MessageBox.Show("Jira project not found.", "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string errorMsg = string.Empty;
            int successCalls = 0;
            int previouslyExported = 0;

            foreach (var c in gridViewModel.suite.cycles.Where(x => x.jiraCloud && x.jiraProjectKey == gridViewModel.suite.jiraProject.key))
            {
                if (c.jiraCycle == null || string.IsNullOrWhiteSpace(c.jiraCycle.key) || string.IsNullOrWhiteSpace(c.jiraProjectKey))
                {
                    errorMsg += $"Cycle {c.id} is missing Jira data. {Environment.NewLine}";
                    continue;
                }
                foreach (CycleRun cr in c.cycleRuns)
                {
                    if (cr.exported)
                    {
                        previouslyExported++;
                        continue;
                    }


                    RunModel r = cr.run;
                    Tuple<HttpStatusCode, JObject> response = await jira.CreateExec(
                                                                                    c.jiraProjectKey,
                                                                                    c.jiraCycle.key,
                                                                                    r.test.id,
                                                                                    r.result,
                                                                                    r.webDriverType,
                                                                                    r.datetimeEnd,
                                                                                    r.runTime,
                                                                                    Properties.Settings.Default.JiraAccountId,
                                                                                    r.resultObj.message);
                    Debug.WriteLine(response.Item2);
                    if (response.Item1 == HttpStatusCode.Created)
                    {
                        cr.exported = true;
                        successCalls++;
                    }
                    else
                    {
                        int errorCode = response.Item2.Value<int>("errorCode");
                        string status = response.Item2.Value<string>("status");
                        string message = response.Item2.Value<string>("message");
                        errorMsg += $"{errorCode.ToString()} {status}: {message}{Environment.NewLine}";
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                if (previouslyExported > 0)
                    errorMsg += $"{Environment.NewLine}Previously exported: {previouslyExported.ToString()}";
                errorMsg += $"{Environment.NewLine}Number of successful calls: {successCalls.ToString()}";
                MessageBox.Show(errorMsg, "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (previouslyExported > 0)
            {
                errorMsg += $"{Environment.NewLine}Previously exported: {previouslyExported.ToString()}";
                errorMsg += $"{Environment.NewLine}Number of successful calls: {successCalls.ToString()}";
                MessageBox.Show(errorMsg, "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        public bool CanExecute_ExportCycleCmd()
        {
            return true;
        }


    }
}

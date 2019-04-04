using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelSupport;
using TestRunnerLib;
using System.Reflection;

using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;


namespace TestRunnerAppWpf
{
    public partial class MainViewModel : ViewModelBase
    {
        // Settings commands
        public void Execute_SetChromeCmd()
        {
            checkedChrome = true; checkedFirefox = false; checkedIE = false; checkedPhantomJS = false; checkedEdge = false;
            Properties.Settings.Default.WebDriver = "chrome";
        }
        public bool CanExecute_SetChromeCmd()
        {
            return true;
        }

        public void Execute_SetFirefoxCmd()
        {
            checkedChrome = false; checkedFirefox = true; checkedIE = false; checkedPhantomJS = false; checkedEdge = false;
            Properties.Settings.Default.WebDriver = "firefox";
        }
        public bool CanExecute_SetFirefoxCmd() 
        {
            return true;
        }

        public void Execute_SetIECmd()
        {
            checkedChrome = false; checkedFirefox = false; checkedIE = true; checkedPhantomJS = false; checkedEdge = false;
            Properties.Settings.Default.WebDriver = "ie";
        }
        public bool CanExecute_SetIECmd()
        {
            return true;
        }

        public void Execute_SetPhantomJSCmd()
        {
            checkedChrome = false; checkedFirefox = false; checkedIE = false; checkedPhantomJS = true; checkedEdge = false;
            Properties.Settings.Default.WebDriver = "phantomjs";
        }
        public bool CanExecute_SetPhantomJSCmd()
        {
            return true;
        }

        public void Execute_SetEdgeCmd()
        {
            checkedChrome = false; checkedFirefox = false; checkedIE = false; checkedPhantomJS = false; checkedEdge = true;
            Properties.Settings.Default.WebDriver = "edge";
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

        }
        public bool CanExecute_SetThemeCmd()
        {
            return true;
        }

       
        public void Execute_SetOnTopCmd()
        {
            Properties.Settings.Default.OnTop = !Properties.Settings.Default.OnTop;
            checkedOnTop = Properties.Settings.Default.OnTop;
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
            string versionString = "1.1";
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

                Tuple<string, SuiteModel> fileopen = FileMgmt.OpenSuiteFrom();
                if (fileopen.Item2 != null)
                {
                    gridViewModel.suite = fileopen.Item2;
                    SelectedItems_PropertyChanged(null, null);
                    this.unsavedChanges = false;
                }

                if (fileopen.Item1 != null)
                    gridViewModel.suite.filename = fileopen.Item1;
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
                this.unsavedChanges = false;
        }
        public bool CanExecute_FileSaveAsCmd()
        {
            return true;
        }

        public void Execute_FileSaveCmd()
        {
            Tuple<bool, string> saveResult = FileMgmt.SaveSuite(gridViewModel.suite);
            if (saveResult.Item1)
                this.unsavedChanges = false;
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

        // Test execution commands
        public void Execute_RunSelectedCmd()
        {
            if (gridViewModel.selectedItems.selectedItems != null || gridViewModel.selectedItems.selectedItems.Count() > 0)
            {
                //RunSelected(gridViewModel.selectedItems.selectedItems);
                RunSelectedAsync(gridViewModel.selectedItems.selectedItems);
            }
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
            CancelJob();
        }
        public bool CanExecute_StopCmd()
        {
            return true;
        }

        // Test management commands
        public void Execute_NewTestCmd()
        {
            var d = new NewTestDialog();
            if (d.ShowDialog() == true)
            {
                gridViewModel.suite.tests.Add(d.newItem);
            }
        }
        public bool CanExecute_NewTestCmd()
        {
            return true;
        }

        public void Execute_CopyTestCmd()
        {
            CopySelectedToNew();
        }
        public bool CanExecute_CopyTestCmd()
        {
            return true;
        }

        public void Execute_RemoveTestCmd()
        {
            if (gridViewModel.selectedItems.selectedItems != null)
            {
                foreach (TestModel test in gridViewModel.selectedItems.selectedItems )
                    gridViewModel.suite.tests.Remove(test);
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
            {
                foreach (RunModel run in detailsViewModel.selectedItems )
                    detailsViewModel.test.runs.Remove(run);
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
        
        public async void Execute_Report1Cmd()
        {
            var j = new Jira();

            // Serverinfo
            Task<Tuple<HttpStatusCode, JObject>> t = j.JiraCall(HttpMethod.Get, "serverInfo", null);

            // Get test case
            //Task<Tuple<HttpStatusCode, JObject>> t = j.JiraCall(HttpMethod.Get, "testcase/JOAK-T12", null);


            await t;
        }
        public bool CanExecute_Report1Cmd()
        {
            return true;
        }




    }
}

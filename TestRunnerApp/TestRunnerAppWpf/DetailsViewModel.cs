using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelSupport;
using TestRunnerLib;
using System.Collections.ObjectModel;
using System.Net;
using Newtonsoft.Json.Linq;
using TestRunnerLib.Jira;
using System.Windows;

using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Windows.Threading;
using System.IO;

namespace TestRunnerAppWpf
{
    public class DetailsViewModel : ViewModelBase
    {
        public MainViewModel mainViewModel
        {
            get => Get(() => mainViewModel);
            set => Set(() => mainViewModel, value);
        }


        public ObservableCollection<RunModel> selectedItems
        {
            get => Get(() => selectedItems, new ObservableCollection<RunModel>());
            set => Set(() => selectedItems, value);
        }
        public ObservableCollection<CycleModel> selectedCycleItems
        {
            get => Get(() => selectedCycleItems, new ObservableCollection<CycleModel>());
            set => Set(() => selectedCycleItems, value);
        }

        public ObservableCollection<JiraProject> jiraAvailableProjects
        {
            get => Get(() => jiraAvailableProjects, new ObservableCollection<JiraProject>());
            set => Set(() => jiraAvailableProjects, value);
        }
        public JiraProject jiraSelectedProject
        {
            get => Get(() => jiraSelectedProject);
            set => Set(() => jiraSelectedProject, value);
        }

        public TestModel test
        {
            get => Get(() => test);
            set => Set(() => test, value);
        }
        public SuiteModel suite
        {
            get => Get(() => suite);
            set => Set(() => suite, value);
        }
        public CycleModel cycle
        {
            get => Get(() => cycle);
            set => Set(() => cycle, value);
        }
        public List<Managment> mgmtOptions
        {
            get => Get(() => mgmtOptions, Enums.Mgmt);
            set => Set(() => mgmtOptions, value);
        }

        // Logging
        string logsDir = AppDomain.CurrentDomain.BaseDirectory + @"Logs\";
        int updateLogTry = 0;
        public string logText
        {
            get => Get(() => logText, string.Empty);
            set => Set(() => logText, value);
        }
        public DispatcherTimer logTimer
        {
            get => Get(() => logTimer, new DispatcherTimer());
            set => Set(() => logTimer, value);
        }

        public DetailsViewModel()
        {
            Debug.WriteLine("Creating detailsViewModel");

            test = new TestModel();
            cycle = new CycleModel();
            suite = new SuiteModel();

            jiraAvailableProjects = Settings.jiraAvailableProjects;

            //Logging
            logTimer = new DispatcherTimer();
            logTimer.Interval = TimeSpan.FromSeconds(3);
            logTimer.Tick += LogTimer_Tick;
            //logTimer.Start();

            this.PropertyChanged += DetailsViewModel_PropertyChanged;
        }

        private async void LogTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateLog();
            }
            catch (IOException)
            {
                logTimer.Stop();
                await Task.Delay(5000);
                logTimer.Start();
            }

        }

        public async void UpdateLog()
        {
            try
            {
                logText = File.ReadAllText(logsDir + "RunnerDebugOutput.log");
                updateLogTry = 0;
            }
            catch (IOException)
            {
                updateLogTry++;
                await Task.Delay(5000);
                if (updateLogTry < 10)
                    UpdateLog();
            }
        }

            /* Commands */
            public async void Execute_JiraGetAvailableProjectsCmd()
            {
            // Disable UI-controls during load operation
            mainViewModel.enableProjectLoad = false;

            var jiraProjects = new ObservableCollection<JiraProject>();

            // Load keys and TMJ-enabled from TMJ 
            int maxResults = 100;
            Tuple<HttpStatusCode, JObject> projectsTmj = await mainViewModel.jira.GetProjectsTmj(maxResults.ToString());
            if (projectsTmj.Item1 == HttpStatusCode.OK)
            {
                JEnumerable<JToken> projTokens = projectsTmj.Item2.GetValue("values").Children();

                foreach (JToken t in projTokens)
                {
                    //if (t.Value<bool>("enabled"))
                    //{
                        var p = new JiraProject(t.Value<int>("id"),
                                                t.Value<int>("jiraProjectId"),
                                                t.Value<string>("key"),
                                                t.Value<bool>("enabled"));
                        jiraProjects.Add(p);
                    //}
                }
            }

            // Load names and avatars from Jira
            Tuple<HttpStatusCode, object> projects = await mainViewModel.jira.GetProjectsJira(null, null);
            if (projects.Item1 == HttpStatusCode.OK)
            {
                JArray j = projects.Item2 as JArray;
                JEnumerable<JToken> projTokens = j.Children();
                foreach (JToken t in projTokens)
                {
                    JiraProject p;
                    int jiraProjectId = t.Value<int>("id");
                    if (jiraProjects.Where(x => x.jiraProjectId == jiraProjectId).Count() > 0)
                    {
                        p = jiraProjects.Where(x => x.jiraProjectId == jiraProjectId).First();
                        p.name = t.Value<string>("name");

                        // Avatars
                        JObject avatarObj = t.Value<JObject>("avatarUrls");
                        if (avatarObj != null)
                             p.avatar = new JiraAvatar(avatarObj.Value<string>("16x16"),
                                                       avatarObj.Value<string>("24x24"),
                                                       avatarObj.Value<string>("32x32"),
                                                       avatarObj.Value<string>("48x48"));

                        //// 1. Create conversion options
                        //WpfDrawingSettings settings = new WpfDrawingSettings();
                        //settings.IncludeRuntime = false;
                        //settings.TextAsGeometry = true;

                        //// 2. Select a file to be converted
                        //string svgTestFile = "Test.svg";

                        //// 3. Create a file converter
                        //FileSvgConverter converter = new FileSvgConverter(settings);
                        //// 4. Perform the conversion to XAML
                        //converter.Convert(svgTestFile);

                    }
                    else
                    {
                        // Load projects that TMJ didn't find? They won't be enabled.....
                    }
                }
            }
            // Sort out TMJ-enabled projects
            jiraAvailableProjects = new ObservableCollection<JiraProject>(jiraProjects.Where(x => x.enabled));    

            Properties.Settings.Default.JiraAvailableProjects = FileMgmt.Serialize(jiraAvailableProjects);

            // Select a project
            suite = mainViewModel.gridViewModel.suite;
            if (suite.jiraProject != null && suite.jiraProject.key != null)
                if (jiraAvailableProjects.Where(x => x.key == suite.jiraProject.key).Count() > 0)
                {
                    jiraSelectedProject = jiraAvailableProjects.Where(x => x.key == suite.jiraProject.key).First();
                }

            // Enable UI-controls after load operation
            mainViewModel.enableProjectLoad = true;
        }
        public bool CanExecute_JiraGetAvailableProjectsCmd()
        {
            return true;
        }

        public void Execute_JiraReloadProjectCmd()
        {
            LoadProjectData();
        }
        public bool CanExecute_JiraReloadProjectCmd()
        {
            return true;
        }

        public void Execute_ResetCycleFolderCmd()
        {
            suite.jiraProject.selectedCycleFolder = null;
        }
        public bool CanExecute_ResetCycleFolderCmd()
        {
            return true;
        }

        public void Execute_ResetCaseFolderCmd()
        {
            suite.jiraProject.selectedCaseFolder = null;
        }
        public bool CanExecute_ResetCaseFolderCmd()
        {
            return true;
        }

        public void Execute_BatchImportCyclesCmd()
        {
            SuiteModel suite = mainViewModel.detailsViewModel.suite;
            foreach (JiraCycle jc in suite.jiraProject.cycles)
            {
                CycleModel c;
                if (suite.cycles.Where(x => x.jiraCycle != null && x.jiraCycle.key == jc.key).Count() > 0)
                {
                    c = suite.cycles.Where(x => x.jiraCycle != null && x.jiraCycle.key == jc.key).First();
                }
                else
                {
                    c = new CycleModel();
                    suite.cycles.Add(c);
                }
                c.jiraCycle = jc; // CycleModel copies data on jiraCycle propertychange
            }
        }
        public bool CanExecute_BatchImportCyclesCmd()
        {
            return true;
        }

        public void Execute_BatchImportCasesCmd()
        {
            SuiteModel suite = mainViewModel.gridViewModel.suite;
            //SuiteModel suite = mainViewModel.detailsViewModel.suite;
            foreach (JiraCase jc in suite.jiraProject.cases)
            {
                TestModel t;
                
                if (suite.tests.Where(x => x.jiraCase != null && x.jiraCase.key == jc.key).Count() > 0)
                {
                    t = suite.tests.Where(x => x.jiraCase != null && x.jiraCase.key == jc.key).First();
                }
                else
                {
                    t = new TestModel();
                    t.jiraLabelToId = suite.jiraLabelToId;
                    t.jiraLabelToIdToken = suite.jiraLabelToIdToken;
                    suite.tests.Add(t);
                }
                t.jiraCase = jc; // TestModel copies data on jiraCase propertychange
            }
        }
        public bool CanExecute_BatchImportCasesCmd()
        {
            return true;
        }

        /* end: Commands */

        /* Deprecated */
        //public bool testDetailsVisi
        //{
        //    get => Get(() => testDetailsVisi);
        //    set => Set(() => testDetailsVisi, value);
        //}
        //public bool suiteDetailsVisi
        //{
        //    get => Get(() => suiteDetailsVisi);
        //    set => Set(() => suiteDetailsVisi, value);
        //}
        /* end: Deprecated */


       

        public void DetailsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"DetailsViewModel propchanged: {e.PropertyName}");

            if (e.PropertyName == "jiraSelectedProject")
            {
                LoadProjectData();
            }
           


        }

        public void LoadProjectData()
        {
            if (jiraSelectedProject == null)
            {
                MessageBox.Show("Loading project data: No project selected.", "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            mainViewModel.gridViewModel.suite.jiraProject = jiraSelectedProject;

            if (mainViewModel.gridViewModel.suite.jiraProject == null)
            {
                MessageBox.Show("Loading project data: No Jira project in suite.", "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Debug.WriteLine($"jiraproject.key: {mainViewModel.gridViewModel.suite.jiraProject.key}");

            var p = mainViewModel.gridViewModel.suite.jiraProject;

            if (Enums.Mgmt.Find(x => x.key == "JiraCloudTmj").enabled)
            {
                mainViewModel.LoadJiraProjectAsync(p);
            }
            else
                mainViewModel.Execute_MgmtSettingsCmd();

            

        }

    }
}

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



        /* Commands */
        public async void Execute_JiraGetAvailableProjectsCmd()
        {
            Tuple<HttpStatusCode, JObject> projects = await Jira.GetProjects(await JiraConnect.TmjPrep(), "100");
            if (projects.Item1 == HttpStatusCode.OK)
            {
                //JToken values = projects.Item2.GetValue("values");
                //JEnumerable<JToken> projTokens = values.Children();
                JEnumerable<JToken> projTokens = projects.Item2.GetValue("values").Children();

                var jiraProjects = new ObservableCollection<JiraProject>();
                foreach (JToken t in projTokens)
                {
                    if (t.Value<bool>("enabled"))
                    {
                        var p = new JiraProject(t.Value<int>("id"),
                                                t.Value<int>("jiraProjectId"),
                                                t.Value<string>("key"),
                                                t.Value<bool>("enabled"));
                        jiraProjects.Add(p);
                    }
                }

                jiraAvailableProjects = jiraProjects;
                jiraSelectedProject = jiraProjects.First();
                Properties.Settings.Default.JiraAvailableProjects = FileMgmt.Serialize(jiraAvailableProjects);
                
            }


        }
        public bool CanExecute_EditCycleCmd()
        {
            return true;
        }
        /* end: Commands */

        /* Deprecated */
        public bool testDetailsVisi
        {
            get => Get(() => testDetailsVisi);
            set => Set(() => testDetailsVisi, value);
        }
        public bool suiteDetailsVisi
        {
            get => Get(() => suiteDetailsVisi);
            set => Set(() => suiteDetailsVisi, value);
        }
        /* end: Deprecated */


        public DetailsViewModel()
        {
            Debug.WriteLine("Creating detailsViewModel");

            test = new TestModel();
            suite = new SuiteModel();
            cycle = new CycleModel();

            jiraAvailableProjects = Settings.jiraAvailableProjects;



            //if (mainViewModel.gridViewModel.suite.jiraProject != null)
            //{
            //    string key = mainViewModel.gridViewModel.suite.jiraProject.key;
            //    jiraSelectedProject = jiraAvailableProjects.Where(x => x.key == key).First();
            //}




            this.PropertyChanged += DetailsViewModel_PropertyChanged;

        }

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

            JiraConnect.LoadProjectData(p);

        }

    }
}

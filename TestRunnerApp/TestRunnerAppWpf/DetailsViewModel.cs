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

        public async void LoadProjectData()
        {
                Debug.WriteLine($"jiraproject.key: {mainViewModel.gridViewModel.suite.jiraProject.key}");

                mainViewModel.gridViewModel.suite.jiraProject = jiraSelectedProject;
                var p = mainViewModel.gridViewModel.suite.jiraProject;
                Tuple<HttpStatusCode, JObject> response;

                response = await Jira.GetProjJira(await JiraConnect.Preflight(), p.key);
                if (response.Item1 == HttpStatusCode.OK)
                {
                    p.name = response.Item2.Value<string>("name");
                    p.description = response.Item2.Value<string>("description");
                    p.self = response.Item2.Value<string>("self");
                }

                response = await Jira.GetStatuses(await JiraConnect.TmjPrep(), p.key, null, "100");
                if (response.Item1 == HttpStatusCode.OK)
                {
                    JEnumerable<JToken> statusTokens = response.Item2.GetValue("values").Children();
                    var statuses = new ObservableCollection<JiraStatus>();
                    foreach (JToken t in statusTokens)
                    {
                        var s = new JiraStatus();
                        statuses.Add(s);

                        s.id = t.Value<int>("id");
                        try { s.project = t.Value<JObject>("project").ToObject<IdSelf>(); } catch (NullReferenceException) { }
                        s.name = t.Value<string>("name");
                        s.description = t.Value<string>("description");
                        s.index = t.Value<int>("index");
                        s.color = t.Value<string>("color");
                        s.archived = t.Value<bool>("archived");
                        s.isDefault = t.Value<bool>("default");
                    }
                    p.statuses = statuses;
                }

                response = await Jira.GetCycles(await JiraConnect.TmjPrep(), p.key, null, "100");
                if (response.Item1 == HttpStatusCode.OK)
                {
                    JEnumerable<JToken> tokens = response.Item2.GetValue("values").Children();
                    var cycles = new ObservableCollection<JiraCycle>();
                    foreach (JToken t in tokens)
                    {
                        var c = new JiraCycle();
                        cycles.Add(c);

                        c.id = t.Value<int>("id");
                        c.key = t.Value<string>("key");
                        c.name = t.Value<string>("name");
                        c.description = t.Value<string>("description");
                        c.plannedStartDate = t.Value<string>("plannedStartDate");
                        c.plannedEndDate = t.Value<string>("plannedEndDate");
                        try { c.project = t.Value<JObject>("project").ToObject<IdSelf>(); } catch (NullReferenceException) { }
                        try { c.jiraProjectVersion = t.Value<JObject>("jiraProjectVersion").ToObject<IdSelf>(); } catch (NullReferenceException) { }
                        try { c.status = t.Value<JObject>("status").ToObject<JiraStatus>(); } catch (NullReferenceException) { }
                        try { c.folder = t.Value<JObject>("folder").ToObject<IdSelf>(); } catch (NullReferenceException) { }
                    }
                    p.cycles = cycles;
                    foreach (JiraCycle c in cycles)
                    {
                        c.status = p.statuses.Where(x => x.id == c.status.id).First();
                    }
                }

        }
    }
}

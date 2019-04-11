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
    public class NewCycleDialogViewModel : ViewModelBase
    {
        public MainViewModel mainViewModel
        {
            get => Get(() => mainViewModel);
            set => Set(() => mainViewModel, value);
        }

        public JiraCycle jiraSelectedCycle
        {
            get => Get(() => jiraSelectedCycle);
            set => Set(() => jiraSelectedCycle, value);
        }

        public JiraProject jiraProject
        {
            get => Get(() => jiraProject);
            set => Set(() => jiraProject, value);
        }

        public CycleModel newItem
        {
            get => Get(() => newItem);
            set => Set(() => newItem, value);
        }

        public bool noMgmt
        {
            get => Get(() => noMgmt, true);
            set => Set(() => noMgmt, value);
        }

        public bool managed
        {
            get => Get(() => managed, false);
            set => Set(() => managed, value);
        }



        /* Commands */

        /* end: Commands */

        


        public NewCycleDialogViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            newItem = new CycleModel();


            if (mainViewModel.jiraCloudMgmt)
            {
                noMgmt = false;
                managed = true;
                jiraProject = mainViewModel.gridViewModel.suite.jiraProject;

                newItem.jiraCloud = true;
                newItem.jiraProjectKey = jiraProject.key;

                if (jiraProject.cycles.Count() > 0)
                    jiraSelectedCycle = jiraProject.cycles.First();

            }
            if (mainViewModel.reqTestMgmt)
            {
                noMgmt = false;
            }


            this.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"NewCycleDialogViewModel propchanged: {e.PropertyName}");

            if (e.PropertyName == "jiraSelectedCycle")
            {
                Debug.WriteLine($"jiracycle.key: {mainViewModel.gridViewModel.suite.jiraCycle.key}");
                
                newItem.jiraCycle = jiraSelectedCycle;

                newItem.id = newItem.jiraCycle.key;
                newItem.name = newItem.jiraCycle.name;
                newItem.description = newItem.jiraCycle.description;



                //newItem.key = jiraSelectedCycle.key;
                //newItem.jiraCycle.key = jiraSelectedCycle.key;


                //mainViewModel.gridViewModel.suite.jiraCycle = jiraSelectedCycle;
            }

        }
    }
}






/*
 
         public async void Execute_JiraGetAvailableCyclesCmd()
        {
            Tuple<HttpStatusCode, JObject> cycles = await Jira.GetCycles(await JiraConnect.TmjPrep(), jiraProject.key, null, "100");
            if (cycles.Item1 == HttpStatusCode.OK)
            {
                JEnumerable<JToken> tokens = cycles.Item2.GetValue("values").Children();
                var available = new ObservableCollection<JiraCycle>();
                foreach (JToken t in tokens)
                {
                    var c = new JiraCycle();
                    available.Add(c);

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
                jiraProject.cycles = available;
                jiraSelectedCycle = jiraProject.cycles.First();

                Tuple<HttpStatusCode, JObject> response = await Jira.GetStatuses(await JiraConnect.TmjPrep(), jiraProject.key, null, "100");
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
                    jiraProject.statuses = statuses;


                }


                foreach (JiraCycle c in available)
                {
                    c.status = jiraProject.statuses.Where(x => x.id == c.status.id).First();
                }


            }


        }
        public bool CanExecute_JiraGetAvailableCyclesCmd()
        {
            return true;
        } 




    */    
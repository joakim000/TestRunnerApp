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


        public ObservableCollection<JiraCycle> jiraAvailableCycles
        {
            get => Get(() => jiraAvailableCycles, new ObservableCollection<JiraCycle>());
            set => Set(() => jiraAvailableCycles, value);
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



        /* Commands */
        public async void Execute_JiraGetAvailableCyclesCmd()
        {
            Tuple<HttpStatusCode, JObject> cycles = await Jira.GetCycles(await JiraConnect.TmjPrep(), newItem.project, null, "100");
            if (cycles.Item1 == HttpStatusCode.OK)
            {
                //JToken values = projects.Item2.GetValue("values");
                //JEnumerable<JToken> projTokens = values.Children();
                JEnumerable<JToken> tokens = cycles.Item2.GetValue("values").Children();

                var jiraCycles = new ObservableCollection<JiraCycle>();
                foreach (JToken t in tokens)
                {
                        var c = new JiraCycle(t.Value<int>("id"),
                                                t.Value<int>("jiraProjectId"),
                                                t.Value<string>("key"),
                                                t.Value<bool>("enabled"));
                        jiraCycles.Add(c);
                    
                }

                jiraAvailableCycles = jiraCycles;
                Properties.Settings.Default.JiraAvailableProjects = FileMgmt.Serialize(jiraAvailableCycles);
                jiraSelectedCycle = jiraCycles.First();
            }


        }
        public bool CanExecute_JiraGetAvailableCyclesCmd()
        {
            return true;
        }
        /* end: Commands */

        


        public NewCycleDialogViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            

            newItem = new CycleModel();

            this.PropertyChanged += ViewModel_PropertyChanged;


            if (mainViewModel.jiraCloudMgmt)
            {
                newItem.project = mainViewModel.gridViewModel.suite.jiraProject.key;
                //idTB.IsEnabled = false;
            }

            //if (jiraServerMgmt)
            //{

            //}

            if (mainViewModel.reqTestMgmt)
            {

            }



        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"NewCycleDialogViewModel propchanged: {e.PropertyName}");
            if (e.PropertyName == "jiraSelectedCycle")
            {
                mainViewModel.gridViewModel.suite.jiraCycle = jiraSelectedCycle;
                Debug.WriteLine($"jiracycle.key: {mainViewModel.gridViewModel.suite.jiraCycle.key}");
            }
            
        }
    }
}

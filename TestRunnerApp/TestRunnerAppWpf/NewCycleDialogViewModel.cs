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

            this.PropertyChanged += ViewModel_PropertyChanged;

            if (mainViewModel.jiraCloudMgmt)
            {
                noMgmt = false;
                managed = true;

                if (mainViewModel.gridViewModel.suite.jiraProject == null)
                    mainViewModel.gridViewModel.suite.jiraProject = new JiraProject();

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


            
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"NewCycleDialogViewModel propchanged: {e.PropertyName}");

            if (e.PropertyName == "jiraSelectedCycle")
            {
                if (jiraSelectedCycle == null)
                    return;

                mainViewModel.gridViewModel.suite.jiraCycle = jiraSelectedCycle;

                Debug.WriteLine($"jiracycle.key: {mainViewModel.gridViewModel.suite.jiraCycle.key}");

                // Find existing cycles with jiraCycle key
                var cyclesFromJira = mainViewModel.gridViewModel.suite.cycles.Where(x => x.jiraCycle != null);
                int existCount = cyclesFromJira.Where(x => x.jiraCycle.key == jiraSelectedCycle.key).Count();
                if (existCount > 0)
                    newItem = cyclesFromJira.Where(x => x.jiraCycle.key == jiraSelectedCycle.key).First();

                Debug.WriteLine("Found number of existing cycles with jiraCycleID: " + existCount);

                newItem.jiraCycle = jiraSelectedCycle;

            }

        }

       

    }
}






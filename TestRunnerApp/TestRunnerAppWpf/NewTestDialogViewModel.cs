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
    public class NewTestDialogViewModel : ViewModelBase
    {
        public MainViewModel mainViewModel
        {
            get => Get(() => mainViewModel);
            set => Set(() => mainViewModel, value);
        }

        public JiraCase jiraSelectedCase
        {
            get => Get(() => jiraSelectedCase);
            set => Set(() => jiraSelectedCase, value);
        }

        public JiraProject jiraProject
        {
            get => Get(() => jiraProject);
            set => Set(() => jiraProject, value);
        }

        public TestModel newItem
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

        


        public NewTestDialogViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            newItem = new TestModel();

            this.PropertyChanged += ViewModel_PropertyChanged;

            if (mainViewModel.jiraCloudMgmt)
            {
                noMgmt = false;
                managed = true;
                jiraProject = mainViewModel.gridViewModel.suite.jiraProject;

                newItem.jiraCloudTmj = true;
                newItem.jiraProjectKey = jiraProject.key;

                //if (jiraProject.cycles.Count() > 0)
                //    jiraSelectedCase = jiraProject.cases.First();

            }
            if (mainViewModel.reqTestMgmt)
            {
                noMgmt = false;
            }


            
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"NewTestDialogViewModel propchanged: {e.PropertyName}");

            if (e.PropertyName == "jiraSelectedCase")
            {
                Debug.WriteLine($"jiracase.key: {mainViewModel.gridViewModel.suite.jiraCycle.key}");

                // Find existing tests with jiraTest key
                var casesFromJira = mainViewModel.gridViewModel.suite.tests.Where(x => x.jiraCase != null);
                int existCount = casesFromJira.Where(x => x.jiraCase.key == jiraSelectedCase.key).Count();
                if (existCount > 0)
                    newItem = casesFromJira.Where(x => x.jiraCase.key == jiraSelectedCase.key).First();

                Debug.WriteLine("Found number of existing cycles with jiraCaseID: " + existCount);

                JiraData2Item();

            }

        }

        private void JiraData2Item()
        {
            newItem.jiraCase = jiraSelectedCase;
            newItem.id = newItem.jiraCase.key;
            newItem.name = newItem.jiraCase.name;
            newItem.objective = newItem.jiraCase.objective;
            newItem.descPrecond = newItem.jiraCase.precondition;
            if (newItem.jiraCase.priority != null)
                newItem.prio = newItem.jiraCase.priority.name;

            //if (newItem.jiraCase.status != null)
            //    newItem.status = newItem.jiraCase.status.name;

            //if (newItem.jiraCase.folder != null)
            //    newItem.prio = newItem.jiraCase.folder.name;

            //if (newItem.jiraCase.owner != null)
            //    newItem.prio = newItem.jiraCase.folder.name;




            //newItem.createdOn = newItem.jiraCase.createdOn;           
            //newItem.estimatedTime = newItem.jiraCase.estimatedTime;
        }

    }
}






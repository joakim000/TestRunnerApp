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
using System.Windows.Controls;

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
        public bool editing
        {
            get => Get(() => editing, false);
            set => Set(() => editing, value);
        }
        public string windowTitle
        {
            get => Get(() => windowTitle);
            set => Set(() => windowTitle, value);
        }



        /* Commands */

        /* end: Commands */




        public NewTestDialogViewModel(MainViewModel mainViewModel)
        {
            windowTitle = "Add new test";
            this.mainViewModel = mainViewModel;
            newItem = new TestModel();
            this.PropertyChanged += ViewModel_PropertyChanged;

            if (mainViewModel.gridViewModel.suite.mgmt == Enums.Mgmt.Find(x => x.key == "None"))
            {
            }

            if (mainViewModel.gridViewModel.suite.mgmt == Enums.Mgmt.Find(x => x.key == "JiraCloudTmj"))
            {
                noMgmt = false;

                newItem.jiraLabelToId = mainViewModel.gridViewModel.suite.jiraLabelToId;
                newItem.jiraLabelToIdToken = mainViewModel.gridViewModel.suite.jiraLabelToIdToken;

                if (mainViewModel.gridViewModel.suite.jiraProject == null)
                    mainViewModel.gridViewModel.suite.jiraProject = new JiraProject();

                jiraProject = mainViewModel.gridViewModel.suite.jiraProject;

                // Deprecated                
                //newItem.jiraCloudTmj = true;
                //newItem.jiraProjectKey = jiraProject.key;

            }
            if (mainViewModel.gridViewModel.suite.mgmt == Enums.Mgmt.Find(x => x.key == "ReqTest"))
            {
                noMgmt = false;
            }
        }

        public NewTestDialogViewModel(MainViewModel mainViewModel, TestModel t)
        {
            windowTitle = "Edit test";
            editing = true;
            this.mainViewModel = mainViewModel;
            newItem = t;
            this.PropertyChanged += ViewModel_PropertyChanged;

            if (mainViewModel.gridViewModel.suite.mgmt == Enums.Mgmt.Find(x => x.key == "None"))
            {
            }

            if (mainViewModel.gridViewModel.suite.mgmt == Enums.Mgmt.Find(x => x.key == "JiraCloudTmj"))
            {
                noMgmt = false;

                newItem.jiraLabelToId = mainViewModel.gridViewModel.suite.jiraLabelToId;
                newItem.jiraLabelToIdToken = mainViewModel.gridViewModel.suite.jiraLabelToIdToken;

                if (mainViewModel.gridViewModel.suite.jiraProject == null)
                    mainViewModel.gridViewModel.suite.jiraProject = new JiraProject();

                jiraProject = mainViewModel.gridViewModel.suite.jiraProject;


            }
            if (mainViewModel.gridViewModel.suite.mgmt == Enums.Mgmt.Find(x => x.key == "ReqTest"))
            {
                noMgmt = false;
            }
        }



        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"NewTestDialogViewModel propchanged: {e.PropertyName}");

            if (e.PropertyName == "jiraSelectedCase")
            {
                if (jiraSelectedCase != null)
                {
                    // Find existing tests with jiraTest key
                    //var casesFromJira = mainViewModel.gridViewModel.suite.tests.Where(x => x.jiraCase != null);
                    //int existCount = casesFromJira.Where(x => x.jiraCase.key == jiraSelectedCase.key).Count();
                    //if (existCount > 0)
                    //    newItem = casesFromJira.Where(x => x.jiraCase.key == jiraSelectedCase.key).First();
                    //Debug.WriteLine("Found number of existing cases with jiraCaseID: " + existCount);

                    if (!editing)
                        managed = true;
                    newItem.jiraCase = jiraSelectedCase;


                }
            }

        }


    }
}






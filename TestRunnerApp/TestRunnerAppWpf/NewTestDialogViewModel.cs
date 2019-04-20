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

        public WrapPanel labelsPanel
        {
            get => Get(() => labelsPanel);
            set => Set(() => labelsPanel, value);
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
                //managed = true;

                if (mainViewModel.gridViewModel.suite.jiraProject == null)
                    mainViewModel.gridViewModel.suite.jiraProject = new JiraProject();

                jiraProject = mainViewModel.gridViewModel.suite.jiraProject;

                newItem.jiraCloudTmj = true;
                newItem.jiraProjectKey = jiraProject.key;

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
                if (jiraSelectedCase != null)
                {
                    // Find existing tests with jiraTest key
                    var casesFromJira = mainViewModel.gridViewModel.suite.tests.Where(x => x.jiraCase != null);
                    int existCount = casesFromJira.Where(x => x.jiraCase.key == jiraSelectedCase.key).Count();
                    if (existCount > 0)
                        newItem = casesFromJira.Where(x => x.jiraCase.key == jiraSelectedCase.key).First();
                    Debug.WriteLine("Found number of existing cases with jiraCaseID: " + existCount);

                    managed = true;
                    newItem.jiraCase = jiraSelectedCase;

                    labelsPanel.Children.Clear();
                    foreach (string s in newItem.jiraCase.labels)
                    {
                        Label l = new Label();
                        //l.BorderThickness = new System.Windows.Thickness(2);
                        //l.FontWeight = new System.Windows.FontWeight();
                        //l.Content = s;

                        l.Margin = new System.Windows.Thickness(0, 0, 5, 0);
                        l.Content = $"[ {s} ]";
                        labelsPanel.Children.Add(l);
                    }

                }
            }

        }


    }
}






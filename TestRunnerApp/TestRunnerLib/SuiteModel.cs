using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using TestRunnerLib.Jira;
using ViewModelSupport;

namespace TestRunnerLib
{
    [JsonObject(MemberSerialization.OptOut)]
    public class SuiteModel : ViewModelBase
    {
        public ObservableCollection<TestModel> tests
        {
            get => Get(() => tests);
            set => Set(() => tests, value);
        }
        public ObservableCollection<CycleModel> cycles
        {
            get => Get(() => cycles);
            set => Set(() => cycles, value);
        }
        public CycleModel currentCycle
        {
            get => Get(() => currentCycle);
            set => Set(() => currentCycle, value);
        }
        public Guid uid
        {
            get => Get(() => uid);
            set => Set(() => uid, value);
        }
        public string name
        {
            get => Get(() => name);
            set => Set(() => name, value);
        }
        public string system
        {
            get => Get(() => system);
            set => Set(() => system, value);
        }
        public string filename
        {
            get => Get(() => filename);
            set => Set(() => filename, value);
        }
        public string version
        {
            get => Get(() => version);
            set => Set(() => version, value);
        }
        public string developer
        {
            get => Get(() => developer);
            set => Set(() => developer, value);
        }
        public string tester
        {
            get => Get(() => tester);
            set => Set(() => tester, value);
        }

        public string notes
        {
            get => Get(() => notes);
            set => Set(() => notes, value);
        }

        /* Jira */
        public string jiraInstance
        {
            get => Get(() => jiraInstance);
            set => Set(() => jiraInstance, value);
        }
        public JiraProject jiraProject
        {
            get => Get(() => jiraProject);
            set => Set(() => jiraProject, value);
        }
        public JiraProject jiraSelectedCyclesFolder
        {
            get => Get(() => jiraSelectedCyclesFolder);
            set => Set(() => jiraSelectedCyclesFolder, value);
        }
        public JiraProject jiraSelectedTestsFolder
        {
            get => Get(() => jiraSelectedTestsFolder);
            set => Set(() => jiraSelectedTestsFolder, value);
        }


        // deprecated
        public JiraCycle jiraCycle
        {
            get => Get(() => jiraCycle);
            set => Set(() => jiraCycle, value);
        }


        public SuiteModel()
        {
            uid = Guid.NewGuid();
            tests = new ObservableCollection<TestModel>();
            cycles = new ObservableCollection<CycleModel>();

            //currentCycle = new CycleModel();
            //currentCycle.key = "R1";
            //currentCycle.name = "New cycle";
            //cycles.Add(currentCycle);
        }
    }
}

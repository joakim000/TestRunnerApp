using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelSupport;

namespace TestRunnerLib.Jira
{

    [JsonObject(MemberSerialization.OptOut)]
    public class JiraProject : ViewModelBase
    {
        public ObservableCollection<JiraCycle> cycles
        {
            get => Get(() => cycles, new ObservableCollection<JiraCycle>());
            set => Set(() => cycles, value);
        }
        public ObservableCollection<JiraStatus> statuses
        {
            get => Get(() => statuses, new ObservableCollection<JiraStatus>());
            set => Set(() => statuses, value);
        }
        /* Properties returned by TMJ */
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public int jiraProjectId
        {
            get => Get(() => jiraProjectId);
            set => Set(() => jiraProjectId, value);
        }
        public string key
        {
            get => Get(() => key);
            set => Set(() => key, value);
        }
        // Test managment enabled
        public bool enabled
        {
            get => Get(() => enabled);
            set => Set(() => enabled, value);
        }
        /* end: Properties returned by TMJ */
        public string name
        {
            get => Get(() => name);
            set => Set(() => name, value);
        }
        public string self
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
        public string description
        {
            get => Get(() => description);
            set => Set(() => description, value);
        }



        public JiraProject() { }

        public JiraProject(int id, int jiraProjectId, string key, bool enabled)
        {
            this.id = id;
            this.jiraProjectId = jiraProjectId;
            this.key = key;
            this.enabled = enabled;
        }
    }

   


}
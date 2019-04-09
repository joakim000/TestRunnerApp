using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelSupport;

namespace TestRunnerLib.Jira
{

    [JsonObject(MemberSerialization.OptOut)]
    public class JiraProject : ViewModelBase
    {
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
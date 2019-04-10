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

    [JsonObject(MemberSerialization.OptOut)]
    public class IdSelf : ViewModelBase
    {
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public string self
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
    }


    [JsonObject(MemberSerialization.OptOut)]
    public class JiraCycle : ViewModelBase
    {
        /* Properties returned by TMJ */
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public string key
        {
            get => Get(() => key);
            set => Set(() => key, value);
        }
        public string name
        {
            get => Get(() => name);
            set => Set(() => name, value);
        }
        public string description
        {
            get => Get(() => description);
            set => Set(() => description, value);
        }
        public string plannedStartDate
        {
            get => Get(() => plannedStartDate);
            set => Set(() => plannedStartDate, value);
        }
        public string plannedStartEnd
        {
            get => Get(() => plannedStartEnd);
            set => Set(() => plannedStartEnd, value);
        }

        public int jiraProjectId
        {
            get => Get(() => jiraProjectId);
            set => Set(() => jiraProjectId, value);
        }
        public string jiraProjectSelf
        {
            get => Get(() => jiraProjectSelf);
            set => Set(() => jiraProjectSelf, value);
        }
        public IdSelf project
        {
            get => Get(() => project);
            set => Set(() => project, value);
        }


        public int jiraProjecVersiontId
        {
            get => Get(() => jiraProjectId);
            set => Set(() => jiraProjectId, value);
        }
        public IdSelf jiraProjecVersion
        {
            get => Get(() => jiraProjecVersion);
            set => Set(() => jiraProjecVersion, value);
        }

        public int jiraStatusId
        {
            get => Get(() => jiraStatusId);
            set => Set(() => jiraStatusId, value);
        }
        public string jiraStatusSelf
        {
            get => Get(() => jiraStatusSelf);
            set => Set(() => jiraStatusSelf, value);
        }
        public IdSelf status
        {
            get => Get(() => status);
            set => Set(() => status, value);
        }



        public int jiraFolderId
        {
            get => Get(() => jiraFolderId);
            set => Set(() => jiraFolderId, value);
        }
        public string jiraFolderSelf
        {
            get => Get(() => jiraFolderSelf);
            set => Set(() => jiraFolderSelf, value);
        }
        public IdSelf folder
        {
            get => Get(() => folder);
            set => Set(() => folder, value);
        }





        public JiraCycle() { }

        public JiraCycle(int id, int jiraProjectId, string key, bool enabled)
        {
            this.id = id;
            this.jiraProjectId = jiraProjectId;
            this.key = key;
        }
    }
}
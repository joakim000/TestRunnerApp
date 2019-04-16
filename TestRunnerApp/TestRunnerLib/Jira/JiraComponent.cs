using Newtonsoft.Json;
using ViewModelSupport;

namespace TestRunnerLib.Jira
{
    [JsonObject(MemberSerialization.OptOut)]
    public class JiraComponent : ViewModelBase
    {
        /* Properties returned by Jira API */
        public string self
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
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
        public JiraUser lead
        {
            get => Get(() => lead);
            set => Set(() => lead, value);
        }
        public string assigneeType
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
        public JiraUser assignee
        {
            get => Get(() => lead);
            set => Set(() => lead, value);
        }
        public string realAssigneeType
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
        public JiraUser realAssignee
        {
            get => Get(() => lead);
            set => Set(() => lead, value);
        }
        public bool isAssigneeTypeValid
        {
            get => Get(() => isAssigneeTypeValid);
            set => Set(() => isAssigneeTypeValid, value);
        }
        public string projectKey // "project" in response
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
        public int projectId
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }


        // Not in Jira API response, possibly useful for TM4J
        public IdSelf project
        {
            get => Get(() => project);
            set => Set(() => project, value);
        }



        public JiraComponent() { }


    }

}
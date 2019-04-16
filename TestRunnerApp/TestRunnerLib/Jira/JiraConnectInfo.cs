using ViewModelSupport;

namespace TestRunnerLib.Jira
{
    public class JiraConnectInfo : ViewModelBase
    {
        // Jira
        public string jiraUser
        {
            get => Get(() => jiraUser);
            set => Set(() => jiraUser, value);
        }
        public string jiraAccountId
        {
            get => Get(() => jiraAccountId);
            set => Set(() => jiraAccountId, value);
        }
        public string jiraToken
        {
            get => Get(() => jiraToken);
            set => Set(() => jiraToken, value);
        }
        public string jiraInstance
        {
            get => Get(() => jiraInstance);
            set => Set(() => jiraInstance, value);
        }
        public string baseURL
        {
            get => Get(() => baseURL);
            set => Set(() => baseURL, value);
        }
        // TMJ
        public string tmjIdToken
        {
            get => Get(() => tmjIdToken);
            set => Set(() => tmjIdToken, value);
        }
        public string tmjKeyToken
        {
            get => Get(() => tmjKeyToken);
            set => Set(() => tmjKeyToken, value);
        }
        // Ready or not?
        public bool ready
        {
            get => Get(() => ready, false);
            set => Set(() => ready, value);
        }

        public JiraConnectInfo() { }

        public JiraConnectInfo(string jiraUser, string jiraAccountId, string jiraToken, string jiraInstance)
        {
            this.jiraUser = jiraUser;
            this.jiraAccountId = jiraAccountId;
            this.jiraToken = jiraToken;
            this.jiraInstance = jiraInstance;
        }


    }

    
}


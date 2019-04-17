using System.Diagnostics;

using System;
using System.Threading.Tasks;

using System.Net;

using Newtonsoft.Json.Linq;
using System.Windows;
using TestRunnerLib;
using TestRunnerLib.Jira;
using ViewModelSupport;

namespace TestRunnerAppWpf
{

    public class JiraConnect
    {

        private MainViewModel mainViewModel { get; set; }

        public JiraConnect(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public async Task<bool> SetAccountId()
        {
            //Tuple<HttpStatusCode, object> user = await jira.CurrentUser(await Preflight());

            Tuple<HttpStatusCode, object> user = await mainViewModel.jira.CurrentUser();
            if (user.Item1 == HttpStatusCode.OK)
            {
                JObject j = user.Item2 as JObject;
                if (j.TryGetValue("accountId", out JToken accountIdToken))
                {
                    Properties.Settings.Default.JiraAccountId = accountIdToken.ToString();
                    Settings.accountIdSet = true;
                    return true;
                }
            }

            //MessageBox.Show("Error retrieving Account ID.", "TestAppRunner with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        public  async Task<JiraConnectInfo> Preflight()
        {
            var info = new JiraConnectInfo();
            string message = string.Empty;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraUser))
            {
                info.jiraUser = Properties.Settings.Default.JiraUser;
            }
            else
                message += "Jira user not set." + Environment.NewLine;

            if (Settings.accountIdSet)
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraAccountId))
                {
                    info.jiraAccountId = Properties.Settings.Default.JiraAccountId;
                }
                else
                {
                    message += "Jira Account ID not set." + Environment.NewLine;

                    Settings.accountIdSet = false;
                    if (!await SetAccountId())
                        message += "Error retrieving Account ID." + Environment.NewLine;
                    else
                        info.jiraAccountId = Properties.Settings.Default.JiraAccountId;
                }
            }

            if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraToken))
            {
                info.jiraToken = Properties.Settings.Default.JiraToken;
            }
            else
                message += "Jira API Token not set." + Environment.NewLine;


            if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraInstance))
            {
                info.jiraInstance = Properties.Settings.Default.JiraInstance;
                info.baseURL = @"https://" + Properties.Settings.Default.JiraInstance + @"/rest/api/3/";
            }
            else
                message += "Jira instance not set." + Environment.NewLine;

            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "TestAppRunner with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                info.ready = true;

            return info;
        }


        public async Task<JiraConnectInfo> TmjPrep()
        {
            var info = new JiraConnectInfo();
            string message = string.Empty;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.TmjIdToken))
            {
                info.tmjIdToken = Properties.Settings.Default.TmjIdToken;
            }
            else
            // Not used as of now
            //message += "TM4J Id Token not set." + Environment.NewLine;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.TmjKeyToken))
            {
                info.tmjKeyToken = Properties.Settings.Default.TmjKeyToken;
            }
            else
                message += "TM4J Key Token not set." + Environment.NewLine;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraAccountId))
            {
                info.jiraAccountId = Properties.Settings.Default.JiraAccountId;
            }
            else
            {
                if (!await SetAccountId())
                    message += "Error retrieving Account ID." + Environment.NewLine;
                else
                    info.jiraAccountId = Properties.Settings.Default.JiraAccountId;
            }

            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "TestAppRunner with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                info.ready = true;

            return info;
        }
    }

   
}


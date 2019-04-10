using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
using TestRunnerLib;
using TestRunnerLib.Jira;

namespace TestRunnerAppWpf
{
    
    public static class JiraConnect
    {

        public static async Task<bool> SetAccountId()
        {
            Tuple<HttpStatusCode, JObject> user = await Jira.CurrentUser(await Preflight());
            if (user.Item1 == HttpStatusCode.OK)
            {
                if (user.Item2.TryGetValue("accountId", out JToken accountIdToken))
                {
                    Properties.Settings.Default.JiraAccountId = accountIdToken.ToString();
                    Settings.accountIdSet = true;
                    return true;
                }
            }
            //MessageBox.Show("Error retrieving Account ID.", "TestAppRunner with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        public static async Task<JiraConnectInfo> Preflight()
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
                    //message += "Jira Account ID not set." + Environment.NewLine;
                    Settings.accountIdSet = false;
                    if (!await JiraConnect.SetAccountId())
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


        public async static Task<JiraConnectInfo> TmjPrep()
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


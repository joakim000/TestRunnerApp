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
using System.Collections.ObjectModel;

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


        public static async void LoadProjectData(JiraProject p)
        {
            string maxResults = "100";
            Tuple<HttpStatusCode, JObject> response;

            // Get basic project data
            response = await Jira.GetProjJira(await JiraConnect.Preflight(), p.key);
            if (response.Item1 == HttpStatusCode.OK)
            {
                p.name = response.Item2.Value<string>("name");
                p.description = response.Item2.Value<string>("description");
                p.self = response.Item2.Value<string>("self");
            }

            // Project statuses
            p.caseStatuses = await LoadStatus(p.key, "TEST_CASE", maxResults);
            p.cycleStatuses = await LoadStatus(p.key, "TEST_CYCLE", maxResults);
            p.executionStatuses = await LoadStatus(p.key, "TEST_EXECUTION", maxResults);
            //p.planStatuses = await LoadStatus(p.key, "TEST_PLAN", maxResults);

            
            //response = await Jira.GetStatuses(await JiraConnect.TmjPrep(), p.key, null, maxResults);
            //if (response.Item1 == HttpStatusCode.OK)
            //{
            //    JEnumerable<JToken> statusTokens = response.Item2.GetValue("values").Children();
            //    var statuses = new ObservableCollection<JiraStatus>();
            //    foreach (JToken t in statusTokens)
            //    {
            //        var s = new JiraStatus();
            //        statuses.Add(s);

            //        s.id = t.Value<int>("id");
            //        try { s.project = t.Value<JObject>("project").ToObject<IdSelf>(); } catch (NullReferenceException) { }
            //        s.name = t.Value<string>("name");
            //        s.description = t.Value<string>("description");
            //        s.index = t.Value<int>("index");
            //        s.color = t.Value<string>("color");
            //        s.archived = t.Value<bool>("archived");
            //        s.isDefault = t.Value<bool>("default");
            //    }
            //    p.statuses = statuses;
            //}

            // Project folders
            response = await Jira.GetFolders(await JiraConnect.TmjPrep(), p.key, null, maxResults);
            if (response.Item1 == HttpStatusCode.OK)
            {
                JEnumerable<JToken> statusTokens = response.Item2.GetValue("values").Children();
                var folders = new ObservableCollection<JiraFolder>();
                foreach (JToken t in statusTokens)
                {
                    var s = new JiraFolder();
                    folders.Add(s);

                    s.id = t.Value<int>("id");
                    s.parentId = t.Value<int?>("parentId");
                    s.name = t.Value<string>("name");
                    s.index = t.Value<int>("index");
                    s.folderType = t.Value<string>("folderType");
                    try { s.project = t.Value<JObject>("project").ToObject<IdSelf>(); } catch (NullReferenceException) { }
                }
                p.folders = folders;
                p.separateFolders();
            }

            // Project priorities
            response = await Jira.GetPrios(await JiraConnect.TmjPrep(), p.key, maxResults);
            if (response.Item1 == HttpStatusCode.OK)
            {
                JEnumerable<JToken> statusTokens = response.Item2.GetValue("values").Children();
                var prios = new ObservableCollection<JiraPrio>();
                foreach (JToken t in statusTokens)
                {
                    var s = new JiraPrio();
                    prios.Add(s);

                    s.id = t.Value<int>("id");
                    try { s.project = t.Value<JObject>("project").ToObject<IdSelf>(); } catch (NullReferenceException) { }
                    s.name = t.Value<string>("name");
                    s.description = t.Value<string>("description");
                    s.index = t.Value<int>("index");
                    s.isDefault = t.Value<bool>("default");
                }
                p.prios = prios;
            }

            // Project environments
            response = await Jira.GetEnvirons(await JiraConnect.TmjPrep(), p.key, maxResults);
            if (response.Item1 == HttpStatusCode.OK)
            {
                JEnumerable<JToken> statusTokens = response.Item2.GetValue("values").Children();
                var environs = new ObservableCollection<JiraEnvironment>();
                foreach (JToken t in statusTokens)
                {
                    var s = new JiraEnvironment();
                    environs.Add(s);

                    s.id = t.Value<int>("id");
                    try { s.project = t.Value<JObject>("project").ToObject<IdSelf>(); } catch (NullReferenceException) { }
                    s.name = t.Value<string>("name");
                    s.description = t.Value<string>("description");
                    s.index = t.Value<int>("index");
                    s.archived = t.Value<bool>("archived");
                }
                p.environments = environs;
            }

            // Project test cycles
            response = await Jira.GetCycles(await JiraConnect.TmjPrep(), p.key, null, maxResults);
            if (response.Item1 == HttpStatusCode.OK)
            {
                JEnumerable<JToken> tokens = response.Item2.GetValue("values").Children();
                var cycles = new ObservableCollection<JiraCycle>();
                foreach (JToken t in tokens)
                {
                    var c = new JiraCycle();
                    cycles.Add(c);

                    c.id = t.Value<int>("id");
                    c.key = t.Value<string>("key");
                    c.name = t.Value<string>("name");
                    c.description = t.Value<string>("description");
                    c.plannedStartDate = t.Value<string>("plannedStartDate");
                    c.plannedEndDate = t.Value<string>("plannedEndDate");

                    if (t.Value<JObject>("project") != null)
                        c.project = t.Value<JObject>("project").ToObject<IdSelf>();
                    if (t.Value<JObject>("jiraProjectVersion") != null)
                        c.jiraProjectVersion = t.Value<JObject>("jiraProjectVersion").ToObject<JiraVersion>();
                    if (t.Value<JObject>("status") != null)
                        c.status = t.Value<JObject>("status").ToObject<JiraStatus>();
                    if (t.Value<JObject>("folder") != null)
                        c.folder = t.Value<JObject>("folder").ToObject<JiraFolder>();
                }
                p.cycles = cycles;
                foreach (JiraCycle c in cycles)
                {
                    
                    if (c.status != null)
                        try { c.status = p.cycleStatuses.Where(x => x.id == c.status.id).First(); } catch (InvalidOperationException) { }
                    if (c.folder != null)
                        try { c.folder = p.folders.Where(x => x.id == c.folder.id).First(); } catch (InvalidOperationException) { }
                    if (c.jiraProjectVersion != null)
                    {
                        response = await Jira.GetVersion(await JiraConnect.Preflight(), c.jiraProjectVersion.id.ToString());
                        if (response.Item1 == HttpStatusCode.OK)
                        {
                            c.jiraProjectVersion.name = response.Item2.Value<string>("name");
                            c.jiraProjectVersion.description = response.Item2.Value<string>("description");
                            c.jiraProjectVersion.releaseDate = response.Item2.Value<string>("releaseDate");
                            c.jiraProjectVersion.released = response.Item2.Value<bool>("released");
                            c.jiraProjectVersion.overdue = response.Item2.Value<bool>("overdue");
                            c.jiraProjectVersion.archived = response.Item2.Value<bool>("archived");
                        }
                    }
                }

                // Project test cases
                response = await Jira.GetCases(await JiraConnect.TmjPrep(), p.key, null, maxResults);
                if (response.Item1 == HttpStatusCode.OK)
                {
                    JEnumerable<JToken> caseTokens = response.Item2.GetValue("values").Children();
                    var cases = new ObservableCollection<JiraCase>();
                    foreach (JToken t in caseTokens)
                    {
                        var c = new JiraCase();
                        cases.Add(c);

                        c.id = t.Value<int>("id");
                        c.key = t.Value<string>("key");
                        c.name = t.Value<string>("name");

                        c.createdOn = t.Value<string>("createdOn");
                        c.objective = t.Value<string>("objective");
                        c.precondition = t.Value<string>("precondition");
                        c.estimatedTime = t.Value<int?>("estimatedTime");

                        // labels
                        try { c.project = t.Value<JObject>("project").ToObject<IdSelf>(); } catch (NullReferenceException) { }
                        if (t.Value<JObject>("priority") != null)
                            c.priority = t.Value<JObject>("priority").ToObject<JiraPrio>();
                        if (t.Value<JObject>("status") != null)
                        c.status = t.Value<JObject>("status").ToObject<JiraStatus>();
                        if (t.Value<JObject>("folder") != null)
                            c.folder = t.Value<JObject>("folder").ToObject<JiraFolder>();
                        if (t.Value<JObject>("owner") != null)
                            c.owner = t.Value<JObject>("owner").ToObject<Owner>();
                    }
                    p.cases = cases;
                    foreach (JiraCase c in cases)
                    {
                        if (c.status != null)
                            try { c.status = p.caseStatuses.Where(x => x.id == c.status.id).First(); } catch (InvalidOperationException) { }
                        if (c.folder != null)
                            try { c.folder = p.folders.Where(x => x.id == c.folder.id).First(); } catch (InvalidOperationException) { }
                        if (c.priority != null)
                            try { c.priority = p.prios.Where(x => x.id == c.priority.id).First(); } catch (InvalidOperationException) { }

                    }
                }

            }

        }

        private async static Task<ObservableCollection<JiraStatus>> LoadStatus(string projectKey,
                                                                               string statusType,
                                                                               string maxResults)
        {
            var response = await Jira.GetStatuses(await JiraConnect.TmjPrep(), projectKey, statusType, maxResults);
            if (response.Item1 == HttpStatusCode.OK)
            {
                JEnumerable<JToken> statusTokens = response.Item2.GetValue("values").Children();
                var statuses = new ObservableCollection<JiraStatus>();
                foreach (JToken t in statusTokens)
                {
                    var s = new JiraStatus();
                    statuses.Add(s);

                    s.id = t.Value<int>("id");
                    try { s.project = t.Value<JObject>("project").ToObject<IdSelf>(); } catch (NullReferenceException) { }
                    s.name = t.Value<string>("name");
                    s.description = t.Value<string>("description");
                    s.index = t.Value<int>("index");
                    s.color = t.Value<string>("color");
                    s.archived = t.Value<bool>("archived");
                    s.isDefault = t.Value<bool>("default");
                }
                return statuses;
            }
            else
                return new ObservableCollection<JiraStatus>();
        }

    }

}


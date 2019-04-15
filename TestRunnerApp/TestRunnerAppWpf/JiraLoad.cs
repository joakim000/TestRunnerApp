﻿using System.Diagnostics;

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
using ViewModelSupport;

namespace TestRunnerAppWpf
{

    
    public class JiraLoad
    {

        public async void LoadProjectData(JiraProject p)
        {
            Tuple<HttpStatusCode, JObject> response;

            // Get basic project data
            response = await Jira.GetProjJira(await JiraConnect.Preflight(), p.key);
            if (response.Item1 == HttpStatusCode.OK)
            {
                p.name = response.Item2.Value<string>("name");
                p.description = response.Item2.Value<string>("description");
                p.self = response.Item2.Value<string>("self");
            }

        }

        // Project folders
        public async Task<ObservableCollection<JiraFolder>> LoadFolders(string projectKey,
                                                                         string folderType,
                                                                             string maxResults)
        {
            var response = await Jira.GetFolders(await JiraConnect.TmjPrep(), projectKey, folderType, maxResults);
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
                return folders;

            }
            else
                return new ObservableCollection<JiraFolder>();
        }

        // Project priorities
        public async Task<ObservableCollection<JiraPrio>> LoadPrios(string projectKey,
                                                                              string maxResults)
        {
            var response = await Jira.GetPrios(await JiraConnect.TmjPrep(), projectKey, maxResults);
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
                 return prios;
            }
            else
                return new ObservableCollection<JiraPrio>();
        }

        // Project environments
        public async Task<ObservableCollection<JiraEnvironment>> LoadEnvirons(string projectKey,
                                                                              string maxResults)
        {
            var response = await Jira.GetEnvirons(await JiraConnect.TmjPrep(), projectKey, maxResults);
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
                return environs;
            }
            else
                return new ObservableCollection<JiraEnvironment>();
        }       

        // Project test cycles
        public async Task<ObservableCollection<JiraCycle>> LoadCycles(JiraProject p,
                                                                      string projectKey,
                                                                      string folderId,
                                                                      string maxResults)
        {
            var response = await Jira.GetCycles(await JiraConnect.TmjPrep(), projectKey, folderId, maxResults);
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
                return cycles;
            }
            else
                return new ObservableCollection<JiraCycle>();
        }


        // Project test cases
        public async Task<ObservableCollection<JiraCase>> LoadCases(JiraProject p,
                                                                    string projectKey, 
                                                                    string folderId,                              
                                                                    string maxResults)
        { 
            var response = await Jira.GetCases(await JiraConnect.TmjPrep(), projectKey, folderId, maxResults);
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
                    if (t.Value<JObject>("project") != null)
                        c.project = t.Value<JObject>("project").ToObject<IdSelf>();
                    c.createdOn = t.Value<string>("createdOn");
                    c.objective = t.Value<string>("objective");
                    c.precondition = t.Value<string>("precondition");
                    c.estimatedTime = t.Value<int?>("estimatedTime");
                    if (t.Value<JArray>("labels") != null)
                    {
                        JArray ja = t.Value<JArray>("labels");
                        var labelList = new List<string>();
                        foreach (string s in ja)
                            labelList.Add(s);
                        c.labels = labelList.ToArray();
                    }
                    if (t.Value<JObject>("component") != null)
                        c.component = t.Value<JObject>("component").ToObject<IdSelf>();
                    if (t.Value<JObject>("priority") != null)
                        c.priority = t.Value<JObject>("priority").ToObject<JiraPrio>();
                    if (t.Value<JObject>("status") != null)
                        c.status = t.Value<JObject>("status").ToObject<JiraStatus>();
                    if (t.Value<JObject>("folder") != null)
                        c.folder = t.Value<JObject>("folder").ToObject<JiraFolder>();
                    if (t.Value<JObject>("owner") != null)
                        c.owner = t.Value<JObject>("owner").ToObject<Owner>();
                }

                foreach (JiraCase c in cases)
                {
                    if (c.status != null)
                        try { c.status = p.caseStatuses.Where(x => x.id == c.status.id).First(); } catch (InvalidOperationException) { }
                    if (c.folder != null)
                        try { c.folder = p.folders.Where(x => x.id == c.folder.id).First(); } catch (InvalidOperationException) { }
                    if (c.priority != null)
                        try { c.priority = p.prios.Where(x => x.id == c.priority.id).First(); } catch (InvalidOperationException) { }

                    Debug.WriteLine(c.labels);
                }
                return cases;
            }
            else
                return new ObservableCollection<JiraCase>();
        }

        public async Task<ObservableCollection<JiraStatus>> LoadStatus(string projectKey,
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

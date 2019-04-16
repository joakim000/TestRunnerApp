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
using ViewModelSupport;

namespace TestRunnerLib.Jira
{
    public static partial class Jira
    {
        /* Special purpose */

        public static async Task<Tuple<HttpStatusCode, object>> GetServerInfo(JiraConnectInfo info)
        {
            var t = JiraCall(info, HttpMethod.Get, "serverInfo", null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, object>> CurrentUser(JiraConnectInfo info)
        {
            var t = JiraCall(info, HttpMethod.Get, "myself", null);
            await t;
            return t.Result;
        }

        /* Get single item */

        public static async Task<Tuple<HttpStatusCode, JObject>> GetCase(JiraConnectInfo info, string testCaseKey)
        {
            string path = "/" + testCaseKey;

            var t = TmjCall(info, HttpMethod.Get, "testcases" + path, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetCycle(JiraConnectInfo info, string testCycleIdOrKey)
        {
            string path = "/" + testCycleIdOrKey;

            var t = TmjCall(info, HttpMethod.Get, "testcycles" + path, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetProjTmj(JiraConnectInfo info, string projectIdOrKey)
        {
            string path = "/" + projectIdOrKey;

            var t = TmjCall(info, HttpMethod.Get, "projects" + path, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, object>> GetProjJira(JiraConnectInfo info, string projectIdOrKey)
        {
            string path = "/" + projectIdOrKey;

            var t = JiraCall(info, HttpMethod.Get, "project" + path, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, object>> GetVersion(JiraConnectInfo info, string versionId)
        {
            string path = "/" + versionId;

            var t = JiraCall(info, HttpMethod.Get, "version" + path, null);
            await t;
            return t.Result;
        }


        /* Get collections */

        public static async Task<Tuple<HttpStatusCode, JObject>> GetCases(JiraConnectInfo info,
                                                                         string projectKey, string folderId, string maxResults)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(projectKey))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "projectKey=" + projectKey;
            }
            if (!string.IsNullOrEmpty(folderId))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "folderId=" + folderId;

            }
            if (!string.IsNullOrEmpty(maxResults))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "maxResults=" + maxResults;
            }

            var t = TmjCall(info, HttpMethod.Get, "testcases" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, object>> GetComponents(JiraConnectInfo info,
                                                                               string projectIdOrKey)
        {
            var t = JiraCall(info, HttpMethod.Get, "project/" + projectIdOrKey + "/components", null);
            await t;
            return t.Result;
        }


        public static async Task<Tuple<HttpStatusCode, JObject>> GetCycles(JiraConnectInfo info,
                                                                           string projectKey,
                                                                           string folderId,
                                                                           string maxResults)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(projectKey))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "projectKey=" + projectKey;
            }
            if (!string.IsNullOrEmpty(folderId))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "folderId=" + folderId;

            }
            if (!string.IsNullOrEmpty(maxResults))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "maxResults=" + maxResults;
            }

            var t = TmjCall(info, HttpMethod.Get, "testcycles" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetEnvirons(JiraConnectInfo info,
                                                                             string projectKey,
                                                                             string maxResults)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(projectKey))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "projectKey=" + projectKey;
            }
            if (!string.IsNullOrEmpty(maxResults))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "maxResults=" + maxResults;
            }

            var t = TmjCall(info, HttpMethod.Get, "environments" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetExecs(JiraConnectInfo info,
                                                                          string projectKey,
                                                                          string testCase, 
                                                                          string testCycle, 
                                                                          string folderId, 
                                                                          string maxResults)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(projectKey))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "projectKey=" + projectKey;
            }
            if (!string.IsNullOrEmpty(folderId))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "folderId=" + folderId;
            }
            if (!string.IsNullOrEmpty(testCase))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "testCase=" + testCase;
            }
            if (!string.IsNullOrEmpty(testCycle))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "testCycle=" + testCycle;
            }
            if (!string.IsNullOrEmpty(maxResults))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "maxResults=" + maxResults;
            }

            var t = TmjCall(info, HttpMethod.Get, "testexecutions" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetFolders(JiraConnectInfo info,
                                                                           string projectKey, string folderType, string maxResults)
        //public static async Task<Tuple<HttpStatusCode, JObject>> GetFolders(JiraConnectInfo info, string projectKey, FolderType folderType, string maxResults )
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(projectKey))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "projectKey=" + projectKey;
            }
            if (!string.IsNullOrEmpty(folderType)) // Valid: "TEST_CASE" "TEST_PLAN" "TEST_CYCLE"
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "folderType=" + folderType;

            }
            if (!string.IsNullOrEmpty(maxResults))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "maxResults=" + maxResults;
            }

            var t = TmjCall(info, HttpMethod.Get, "folders" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetPrios(JiraConnectInfo info, string projectKey, string maxResults)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(projectKey))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "projectKey=" + projectKey;
            }
            if (!string.IsNullOrEmpty(maxResults))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "maxResults=" + maxResults;
            }

            var t = TmjCall(info, HttpMethod.Get, "priorities" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetProjects(JiraConnectInfo info, string maxResults)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(maxResults))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "maxResults=" + maxResults;
            }

            var t = TmjCall(info, HttpMethod.Get, "projects" + query, null);
            await t;
            return t.Result;
        }

        

        public static async Task<Tuple<HttpStatusCode, JObject>> GetStatuses(JiraConnectInfo info,
                                                                             string projectKey,
                                                                             string statusType,
                                                                             string maxResults)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(projectKey))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "projectKey=" + projectKey;
            }
            if (!string.IsNullOrEmpty(statusType)) // Valid: "TEST_CASE" "TEST_PLAN" "TEST_CYCLE" "TEST_EXECUTION"
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "statusType=" + statusType;
            }
            if (!string.IsNullOrEmpty(maxResults))
            {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += "maxResults=" + maxResults;
            }

            var t = TmjCall(info, HttpMethod.Get, "statuses" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, object>> GetVersions(JiraConnectInfo info,
                                                                                string projectIdOrKey)
        {
            //GET issue/ JRACLOUD - 34423 ? expand = names,renderedFields
            //operations Returns actions that can be performed on the specified version.

            //string query = "?operations";
            string query = string.Empty;

            var t = JiraCall(info, HttpMethod.Get, "project/" + projectIdOrKey + "/versions" + query, null);
            await t;
            return t.Result;
        }




    }

}


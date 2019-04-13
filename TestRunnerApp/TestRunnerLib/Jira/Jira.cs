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

    public static class Jira
    {
        public enum FolderType { All, Case, Plan, Cycle }

        //// Jira cloud
        //static string jiraUser = null;
        //static string accountID = null;
        //static string jiraToken = null;
        //static string baseURL = null;
        //// TM4J cloud
        //static string tmjIdToken = null;
        //static string tmjKeyToken = null;


        public static async Task<Tuple<HttpStatusCode, JObject>> GetServerInfo(JiraConnectInfo info)
        {
            var t = JiraCall(info, HttpMethod.Get, "serverInfo", null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> CurrentUser(JiraConnectInfo info)
        {
            var t = JiraCall(info, HttpMethod.Get, "myself", null);
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

        public static async Task<Tuple<HttpStatusCode, JObject>> GetCase(JiraConnectInfo info, string testCaseKey)
        {
            string path = "/" + testCaseKey;

            var t = TmjCall(info, HttpMethod.Get, "testcases" + path, null);
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

        public static async Task<Tuple<HttpStatusCode, JObject>> GetProjJira(JiraConnectInfo info, string projectIdOrKey)
        {
            string path = "/" + projectIdOrKey;

            var t = JiraCall(info, HttpMethod.Get, "project" + path, null);
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
        public static async Task<Tuple<HttpStatusCode, JObject>> GetVersion(JiraConnectInfo info, string versionId)
        {
            string path = "/" + versionId;

            var t = JiraCall(info, HttpMethod.Get, "version" + path, null);
            await t;
            return t.Result;
        }


        public static async Task<Tuple<HttpStatusCode, JObject>> GetFolders(JiraConnectInfo info, 
                                                                            string projectKey, string folderType, string maxResults )
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

        public static async Task<Tuple<HttpStatusCode, JObject>> CreateCase(JiraConnectInfo info,
                                                                            string projectKey,
                                                                            string name,
                                                                            string objective,
                                                                            string precondition,
                                                                            int? estimatedTime,
                                                                            int? componentId,
                                                                            string priorityName,
                                                                            string statusName,
                                                                            int? folderId,
                                                                            string ownerId,
                                                                            string[] labels)
        {
            var data = new Dictionary<string, object>();

            // Required
            if (string.IsNullOrEmpty(projectKey) || string.IsNullOrEmpty(name))
                return null;
            data.Add("projectKey", projectKey);
            data.Add("name", name);

            // Optional
            if (!string.IsNullOrEmpty(objective))
                data.Add("objective", objective);

            if (!string.IsNullOrEmpty(precondition))
                data.Add("precondition", precondition);

            if (estimatedTime != null)
                data.Add("estimatedTime", estimatedTime);

            if (componentId != null)
                data.Add("componentId", componentId);

            if (!string.IsNullOrEmpty(priorityName))
                data.Add("priorityName", priorityName);

            if (!string.IsNullOrEmpty(statusName))
                data.Add("statusName", statusName);

            if (folderId != null)
                data.Add("folderId", folderId);

            if (!string.IsNullOrEmpty(ownerId))
                data.Add("ownerId", ownerId);

            if (labels != null))
                data.Add("labels", labels);


            var t = TmjCall(info, HttpMethod.Post, "testcases", data);
            await t;
            return t.Result;
        }



        public static async Task<Tuple<HttpStatusCode, JObject>> CreateCycle(JiraConnectInfo info,
                                                                             string projectKey,
                                                                             string name, 
                                                                             string description,
                                                                             string plannedStartDate,
                                                                             string plannedEndDate,
                                                                             int? jiraProjectVersion,
                                                                             string statusName,
                                                                             int? folderId, 
                                                                             string ownerId)
        {
            var data = new Dictionary<string, object>();

            // Required
            if (string.IsNullOrEmpty(projectKey) || string.IsNullOrEmpty(name))
                return null;
            data.Add("projectKey", projectKey);
            data.Add("name", name);

            // Optional
            if (!string.IsNullOrEmpty(description))
                data.Add("description", description);

            if (!string.IsNullOrEmpty(plannedStartDate))
                data.Add("plannedStartDate", plannedStartDate);

            if (!string.IsNullOrEmpty(plannedEndDate))
                data.Add("plannedEndDate", plannedEndDate);

            if (jiraProjectVersion != null)
                data.Add("jiraProjectVersion", jiraProjectVersion);

            if (!string.IsNullOrEmpty(statusName))
                data.Add("statusName", statusName);

            if (folderId != null)
                data.Add("folderId", folderId);

            if (!string.IsNullOrEmpty(ownerId))
                data.Add("ownerId", ownerId);


            var t = TmjCall(info, HttpMethod.Post, "testcycles", data);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> CreateExec(JiraConnectInfo info,
                                                                            string projectKey,
                                                                            string testCycleKey,
                                                                            string testCaseKey,
                                                                            Outcome? outcome,              //string statusName, 
                                                                            WebDriverType? webDriverType,  //string environmentName,
                                                                            DateTime? endTime,             //string actualEndDate,
                                                                            long? executionTime,
                                                                            string executedById,
                                                                            string comment)
        {
            var data = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(projectKey))
                data.Add(nameof(projectKey), projectKey);

            if (!string.IsNullOrEmpty(testCaseKey))
                data.Add("testCaseKey", testCaseKey);

            if (!string.IsNullOrEmpty(testCycleKey))
                data.Add("testCycleKey", testCycleKey);

            if (outcome != null)
                data.Add("statusName", Outcome2string(outcome));

            if (webDriverType != null && webDriverType != WebDriverType.None)
                data.Add("environmentName", WebDriverType2string(webDriverType));

            if (endTime != null)
                data.Add("actualEndDate", JiraDate(endTime));

            if (executionTime != null)
                data.Add("executionTime", executionTime);

            if (!string.IsNullOrEmpty(executedById))
                data.Add("executedById", executedById);

            if (!string.IsNullOrEmpty(comment))
                data.Add("comment", comment);

//            data.Add("testScriptResults", new Array({
//"statusName": "Pass",
//"actualEndDate": "2019-04-06T12:49:03Z",
//"actualResult": "User logged in successfully"
//})



            var t = TmjCall(info, HttpMethod.Post, "testexecutions", data);
            await t;
            return t.Result;
        }

       





        /* Private methods */
        private static string BuildQuery(params string[] p)
        {
            string query = string.Empty;
            foreach (string s in p)
            {
                if (!string.IsNullOrEmpty(s))
                    query += string.IsNullOrEmpty(query) ? "?" : "&";
            }
            return query;
        }

        private static Dictionary<string, object> BuildData(Dictionary<string, object> data, object obj, string name)
        {
            return null;
        }

        //private async static Task<bool> JiraPreflight(string JiraUser, bool accountIdSet, string JiraAccountId, string JiraToken, string JiraInstance)
        //{
        //    string message = string.Empty;

        //    if (!string.IsNullOrEmpty(JiraUser))
        //    {
        //        jiraUser = JiraUser;
        //    }
        //    else
        //        message += "Jira user not set." + Environment.NewLine;

        //    if (accountIdSet)
        //    {
        //        if (!string.IsNullOrEmpty(JiraAccountId))
        //        {
        //            accountID = JiraAccountId;
        //        }
        //        else
        //        {
        //            //message += "Jira Account ID not set." + Environment.NewLine;
        //            ////Settings.accountIdSet = false;
        //            ////if (!await Jira.SetAccountId())
        //            //    message += "Error retrieving Account ID." + Environment.NewLine;
        //            //else
        //                accountID = JiraAccountId;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(JiraToken))
        //    {
        //        jiraToken = JiraToken;
        //    }
        //    else
        //        message += "Jira API Token not set." + Environment.NewLine;

        //    if (!string.IsNullOrEmpty(JiraInstance))
        //    {
        //        baseURL = @"https://" + JiraInstance + @"/rest/api/3/";
        //    }
        //    else
        //        message += "Jira instance not set." + Environment.NewLine;

        //    if (!string.IsNullOrEmpty(message))
        //    {
        //        MessageBox.Show(message, "TestAppRunner with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //    else
        //        return true;
        //}


        private static async Task<Tuple<HttpStatusCode, Newtonsoft.Json.Linq.JObject>> JiraCall(JiraConnectInfo info, HttpMethod method, string api, Dictionary<string, string> data)
        {
            if (!info.ready)
            {
                return null;
            }

            var baseURL = @"https://" + info.jiraInstance + @"/rest/api/3/";

            var itemAsJson = JsonConvert.SerializeObject(data);
            var query = new StringContent(itemAsJson);
            var uri = new Uri(baseURL + api);
            var client = GetClientByUserPw(method, uri, info.jiraUser, info.jiraToken);
            query = new StringContent(itemAsJson, System.Text.Encoding.UTF8, "application/json");
            var response = new HttpResponseMessage();
            try
            {
                if (method == HttpMethod.Get)
                    response = await client.GetAsync(uri);
                else if (method == HttpMethod.Post)
                    response = await client.PostAsync(uri, query);
                else if (method == HttpMethod.Put)
                    response = await client.PutAsync(uri, query);
                else if (method == HttpMethod.Delete)
                    response = await client.DeleteAsync(uri);
                else return new Tuple<HttpStatusCode, JObject>(HttpStatusCode.MethodNotAllowed, null);

                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    Debug.WriteLine($"Call OK: {uri}{Environment.NewLine}Query: {JsonConvert.SerializeObject(data)}");
                    return new Tuple<HttpStatusCode, JObject>(response.StatusCode, jsonObj);
                }
                else
                {
                    Debug.WriteLine("Call failed. Statuscode: " + response.StatusCode);
                    Debug.WriteLine("Request message: " + response.RequestMessage);
                    Debug.WriteLine("Query: " + query.ToString());
                    Debug.WriteLine("Response content: " + response.Content.ReadAsStringAsync().Result.ToString());
                    return new Tuple<HttpStatusCode, JObject>(response.StatusCode, jsonObj);
                }
            }
            catch (ArgumentNullException ex) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "ArgumentNullException: " + ex); }
            catch (HttpRequestException ex) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "HttpRequestException: " + ex); }
            catch (OperationCanceledException ex) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "OperationCanceledException: " + ex); }
            catch (Exception ex) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "General exception: " + ex); }
            finally
            {
                client.Dispose();
            }
            return new Tuple<HttpStatusCode, JObject>(HttpStatusCode.Unused, null);
        }


        private static async Task<Tuple<HttpStatusCode, Newtonsoft.Json.Linq.JObject>> TmjCall(JiraConnectInfo info, HttpMethod method, string api, Dictionary<string, object> data)
        {
            if (!info.ready)
            {
                return null;
                //return new Tuple<HttpStatusCode, JObject>(HttpStatusCode.PaymentRequired, null); // Using this statuscode for intenal purposes
            }

            string baseURL = @"https://api.adaptavist.io/tm4j/v2/";
            var uri = new Uri(baseURL + api);
            var client = GetClient(method, uri, "Bearer", info.tmjKeyToken);

            StringContent query = null;
            if (data != null)
            {
                var itemAsJson = JsonConvert.SerializeObject(data);
                query = new StringContent(itemAsJson, System.Text.Encoding.UTF8, "application/json");
            }

            var response = new HttpResponseMessage();
            try
            {
                if (method == HttpMethod.Get)
                    response = await client.GetAsync(uri);
                else if (method == HttpMethod.Post)
                    response = await client.PostAsync(uri, query);
                else if (method == HttpMethod.Put)
                    response = await client.PutAsync(uri, query);
                else if (method == HttpMethod.Delete)
                    response = await client.DeleteAsync(uri);
                else return new Tuple<HttpStatusCode, JObject>(HttpStatusCode.MethodNotAllowed, null);

                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    Debug.WriteLine($"Call OK: {uri}{Environment.NewLine}Query: {JsonConvert.SerializeObject(data)}");
                    Debug.WriteLine("Response content: " + response.Content.ReadAsStringAsync().Result.ToString());
                    return new Tuple<HttpStatusCode, JObject>(response.StatusCode, jsonObj);
                }
                else
                {
                    Debug.WriteLine("Call failed. Statuscode: " + response.StatusCode);
                    Debug.WriteLine("Request message: " + response.RequestMessage);
                    Debug.WriteLine("Query: " + JsonConvert.SerializeObject(data));
                    Debug.WriteLine("Response content: " + response.Content.ReadAsStringAsync().Result.ToString());
                    return new Tuple<HttpStatusCode, JObject>(response.StatusCode, jsonObj);
                }
            }
            catch (ArgumentNullException ex) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "ArgumentNullException: " + ex); }
            catch (HttpRequestException ex) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "HttpRequestException: " + ex); }
            catch (OperationCanceledException ex) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "OperationCanceledException: " + ex); }
            catch (Exception ex) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "General exception: " + ex); }
            finally
            {
                client.Dispose();
            }
            return new Tuple<HttpStatusCode, JObject>(HttpStatusCode.Unused, null);
        }


        private static HttpClient GetClientByUserPw(HttpMethod method, Uri uri, string user, string pw)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, uri);
            var byteArray = Encoding.ASCII.GetBytes($"{user}:{pw}");
            var base64token = Convert.ToBase64String(byteArray);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64token);
            client.DefaultRequestHeaders.Authorization = request.Headers.Authorization;
            return client;
        }

        private static HttpClient GetClient(HttpMethod method, Uri uri, string authType, string authToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, uri);
            // Auth types: Basic, Bearer
            request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            client.DefaultRequestHeaders.Authorization = request.Headers.Authorization;
            return client;
        }

        // Get the strong type out
        //System.Diagnostics.Debug.WriteLine("One of the Authorization values: {0}={1}",
        //    request.Headers.Authorization.Scheme,
        //    request.Headers.Authorization.Token);

        //// The ToString() is useful for diagnostics, too.
        //System.Diagnostics.Debug.WriteLine("The Authorization ToString() results: {0}", request.Headers.Authorization.ToString());

        /* Helper methods */
        private static string Outcome2string(Outcome o)
        {
            switch (o)
            {
                case Outcome.Warning:
                    return "Warning";
                case Outcome.Pass:
                    return "Pass";
                case Outcome.Fail:
                    return "Fail";
                case Outcome.NotRun:
                    return "Not Executed";
                default:
                    return "Unknown";

            }
        }

        private static string Outcome2string(Outcome? nullableO)
        {
            if (nullableO == null)
                return null;
            else
            {
                Outcome o = (Outcome)nullableO;
                switch (o)
                {
                    case Outcome.Warning:
                        return "Warning";
                    case Outcome.Pass:
                        return "Pass";
                    case Outcome.Fail:
                        return "Fail";
                    case Outcome.NotRun:
                        return "Not Executed";
                    default:
                        return "Unknown";

                }
            }
        }

        private static string WebDriverType2string(WebDriverType w)
        {
            switch (w)
            {
                case WebDriverType.Chrome:
                    return "Chrome";
                case WebDriverType.Firefox:
                    return "Firefox";
                default:
                    return "Unknown";

            }
        }

        private static string WebDriverType2string(WebDriverType? nullableWdt)
        {
            if (nullableWdt == null)
                return null;
            else
            {
                WebDriverType wdt = (WebDriverType)nullableWdt;
                switch (wdt)
                {
                    case WebDriverType.Chrome:
                        return "Chrome";
                    case WebDriverType.Firefox:
                        return "Firefox";
                    default:
                        return "Unknown";

                }
            }
        }

        private static string JiraDate(DateTime? nullableDt)
        {
            if (nullableDt == null)
                return null;
            else
            {
                DateTime dt = (DateTime)nullableDt;
                return dt.ToUniversalTime().ToString("s") + "Z";
            }
        } 

    }

}


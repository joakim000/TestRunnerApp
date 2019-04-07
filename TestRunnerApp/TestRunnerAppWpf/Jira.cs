using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestRunnerAppWpf
{
    

    public static class Jira
    {
        public enum FolderType { All, Case, Plan, Cycle }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetServerInfo()
        {
            var t = JiraCall(HttpMethod.Get, "serverInfo", null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> CurrentUser()
        {
            var t = JiraCall(HttpMethod.Get, "myself", null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetCases(string projectKey, string folderId, string maxResults)
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

            var t = TmjCall(HttpMethod.Get, "testcases" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetCase(string testCaseKey)
        {
            string path = "/" + testCaseKey;

            var t = TmjCall(HttpMethod.Get, "testcases" + path, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetProj(string projectIdOrKey)
        {
            string path = "/" + projectIdOrKey;

            var t = TmjCall(HttpMethod.Get, "projects" + path, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetCycle(string testCycleIdOrKey)
        {
            string path = "/" + testCycleIdOrKey;

            var t = TmjCall(HttpMethod.Get, "testcycles" + path, null);
            await t;
            return t.Result;
        }


        public static async Task<Tuple<HttpStatusCode, JObject>> GetFolders(string projectKey, string folderType, string maxResults )
        //public static async Task<Tuple<HttpStatusCode, JObject>> GetFolders(string projectKey, FolderType folderType, string maxResults )
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

            var t = TmjCall(HttpMethod.Get, "testcases" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetExecs(string projectKey,
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

            var t = TmjCall(HttpMethod.Get, "testexecutions" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetPrios(string projectKey, string maxResults)
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

            var t = TmjCall(HttpMethod.Get, "priorities" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetCycles(string projectKey,
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

            var t = TmjCall(HttpMethod.Get, "testcycles" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetEnvirons(string projectKey,
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

            var t = TmjCall(HttpMethod.Get, "environments" + query, null);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> GetStatuses(string projectKey,
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

            var t = TmjCall(HttpMethod.Get, "statuses" + query, null);
            await t;
            return t.Result;
        }



        public static async Task<Tuple<HttpStatusCode, JObject>> CreateCycle(string projectKey,
                                                                             string name, 
                                                                             string description, 
                                                                             string folderId, 
                                                                             bool setOwner)
        {
            var data = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(projectKey))
                data.Add("projectKey", projectKey);
            if (!string.IsNullOrEmpty(name))
                data.Add("name", name);
            if (!string.IsNullOrEmpty(description))
                data.Add("description", description);
            if (!string.IsNullOrEmpty(folderId))
                data.Add("folderId", folderId);


            var t = TmjCall(HttpMethod.Post, "testcycles", data);
            await t;
            return t.Result;
        }

        public static async Task<Tuple<HttpStatusCode, JObject>> CreateExec(string projectKey,
                                                                            string testCycleKey,
                                                                            string testCaseKey,
                                                                            string statusName, 
                                                                            string environmentName,
                                                                            string actualEndDate,
                                                                            long? executionTime,
                                                                            string executedById,
                                                                            string comment)
        {
            var data = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(projectKey))
                data.Add("projectKey", projectKey);
            if (!string.IsNullOrEmpty(testCaseKey))
                data.Add("testCaseKey", testCaseKey);
            if (!string.IsNullOrEmpty(testCycleKey))
                data.Add("testCycleKey", testCycleKey);
            if (!string.IsNullOrEmpty(statusName))
                data.Add("statusName", statusName);
            if (!string.IsNullOrEmpty(environmentName))
                data.Add("environmentName", environmentName);
            if (!string.IsNullOrEmpty(actualEndDate))
                data.Add("actualEndDate", actualEndDate);
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



            var t = TmjCall(HttpMethod.Post, "testexecutions", data);
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


        private static async Task<Tuple<HttpStatusCode, Newtonsoft.Json.Linq.JObject>> JiraCall(HttpMethod method, string api, Dictionary<string, string> data)
        {
            /* Get from settings */
            string jiraInstance = @"unicus-sverige.atlassian.net";
            string user = "joakim.odermalm@unicus.no";
            string userID = "5c580424b76d5c34f3599572";
            string apiToken = "KJjFX9E2PcJnq9Bp4rF71410";
            /* Get from settings */

            string baseURL = @"https://" + jiraInstance + @"/rest/api/3/";

            var itemAsJson = JsonConvert.SerializeObject(data);
            var query = new StringContent(itemAsJson);
            var uri = new Uri(baseURL + api);
            var client = GetClientByUserPw(method, uri, user, apiToken);
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

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    JObject jsonObj = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());
                    //Debug.WriteLine($"Call OK:{Environment.NewLine}{jsonObj}");
                    Debug.WriteLine($"Call OK: {uri}{Environment.NewLine}Query: {JsonConvert.SerializeObject(data)}");
                    return new Tuple<HttpStatusCode, JObject>(response.StatusCode, jsonObj);
                }
                else
                {
                    Debug.WriteLine("Call failed. Statuscode: " + response.StatusCode);
                    Debug.WriteLine("Request message: " + response.RequestMessage);
                    Debug.WriteLine("Query: " + query.ToString());
                    Debug.WriteLine("Response content: " + response.Content.ReadAsStringAsync().Result.ToString());
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


        private static async Task<Tuple<HttpStatusCode, Newtonsoft.Json.Linq.JObject>> TmjCall(HttpMethod method, string api, Dictionary<string, object> data)
        {
            string baseURL = @"https://api.adaptavist.io/tm4j/v2/";

            /* Get from settings */
            string idToken = "JZMVlWAEC";
            string keyToken = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjb250ZXh0Ijp7InV1aWQiOiJKWk1WbFdBRUMiLCJhcGlHd0tleSI6ImNwZG1PYnB3OE05elRuNnQyYlk3ODJhRFg5c2g5aHI1OGN4WHVtWGkiLCJiYXNlVXJsIjoiaHR0cHM6Ly91bmljdXMtc3ZlcmlnZS5hdGxhc3NpYW4ubmV0IiwidXNlciI6eyJhY2NvdW50SWQiOiI1YzU4MDQyNGI3NmQ1YzM0ZjM1OTk1NzIiLCJkaXNwbGF5TmFtZSI6IkpvYWtpbSBPZGVybWFsbSIsInVzZXJLZXkiOiJqb2FraW0ub2Rlcm1hbG0iLCJ1c2VybmFtZSI6ImpvYWtpbS5vZGVybWFsbSJ9fSwiaWF0IjoxNTU0NDQ0NzIxLCJleHAiOjE1ODYwMDIzMjEsImlzcyI6ImNvbS5rYW5vYWgudGVzdC1tYW5hZ2VyIiwic3ViIjoiNTdlMTU4OGQtNGY1Zi0zN2RkLTkyYWItM2MwZGQzYTc0MDMxIn0.Lt6T-zcgdHA6sMhUOS7ueYLazbBYAiG26EBIRWEb7iw";
            /* Get from settings */

            var uri = new Uri(baseURL + api);
            var client = GetClient(method, uri, "Bearer", keyToken);

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

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    JObject jsonObj = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());
                    //Debug.WriteLine($"Call OK:{Environment.NewLine}{jsonObj}");
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













    }
}

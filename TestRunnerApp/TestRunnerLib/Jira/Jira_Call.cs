using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ViewModelSupport;

namespace TestRunnerLib.Jira
{
    public partial class Jira
    {
        public enum FolderType { All, Case, Plan, Cycle }

        private JiraConnectInfo jiraCloudInfo { get; set; }
        private JiraConnectInfo tmjCloudInfo { get; set; }
        public JiraLoad load { get; set; }

        public Jira(JiraConnectInfo jiraCloud, JiraConnectInfo tmjCloud)
        {
            jiraCloudInfo = jiraCloud;
            tmjCloudInfo = tmjCloud;
            load = new JiraLoad(this);
        }


        private async Task<Tuple<HttpStatusCode, object>> JiraCall(HttpMethod method, string api, Dictionary<string, string> data)
        {
            JiraConnectInfo info = jiraCloudInfo;

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
                else return new Tuple<HttpStatusCode, object>(HttpStatusCode.MethodNotAllowed, null);

                var jsonObj = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    Debug.WriteLine($"Call OK: {uri}{Environment.NewLine}Query: {JsonConvert.SerializeObject(data)}");
                     return new Tuple<HttpStatusCode, object>(response.StatusCode, jsonObj);
                }
                else
                {
                    Debug.WriteLine("Call failed. Statuscode: " + response.StatusCode);
                    Debug.WriteLine("Request message: " + response.RequestMessage);
                    Debug.WriteLine("Query: " + query.ToString());
                    Debug.WriteLine("Response content: " + response.Content.ReadAsStringAsync().Result.ToString());
                    return new Tuple<HttpStatusCode, object>(response.StatusCode, jsonObj);
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
            return new Tuple<HttpStatusCode, object>(HttpStatusCode.Unused, null);
        }


        private async Task<Tuple<HttpStatusCode, Newtonsoft.Json.Linq.JObject>> TmjCall(JiraConnectInfo info_notused, HttpMethod method, string api, Dictionary<string, object> data)
        {
            JiraConnectInfo info = tmjCloudInfo;

            if (!info.ready)
            {
                return null;
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


        private  HttpClient GetClientByUserPw(HttpMethod method, Uri uri, string user, string pw)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, uri);
            var byteArray = Encoding.ASCII.GetBytes($"{user}:{pw}");
            var base64token = Convert.ToBase64String(byteArray);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64token);
            client.DefaultRequestHeaders.Authorization = request.Headers.Authorization;
            return client;
        }

        private  HttpClient GetClient(HttpMethod method, Uri uri, string authType, string authToken)
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

/* PreFlight / Prep is in app for the time being, needs to access user settings */

//private async  Task<bool> JiraPreflight(string JiraUser, bool accountIdSet, string JiraAccountId, string JiraToken, string JiraInstance)
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
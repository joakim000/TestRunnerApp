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
    public class Jira
    {
        public Jira() {}

        public async Task<Tuple<HttpStatusCode, Newtonsoft.Json.Linq.JObject>> JiraCall(HttpMethod method, string api, Dictionary<string, string> data)
        {
            /* Get from settings */
            string baseURL = @"https://unicus-sverige.atlassian.net/rest/api/3/";
            string user = "joakim.odermalm@unicus.no";
            string pw = "Madrid2000";
            string authToken = "get from settings";
            /* Get from settings */

            var itemAsJson = JsonConvert.SerializeObject(data);
            var query = new StringContent(itemAsJson);
            var uri = new Uri(baseURL + api);
            var client = GetClientByUserPw(method, uri, user, pw);
            //var client = GetClient(method, uri, authToken);
            query.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject jsonObj = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());
                    Debug.WriteLine($"Call OK:{Environment.NewLine}{jsonObj}");
                    return new Tuple<HttpStatusCode, JObject>(HttpStatusCode.OK, jsonObj);
                }
                else
                {
                    Debug.WriteLine("Call failed. Statuscode: " + response.StatusCode);
                    Debug.WriteLine("Request message: " + response.RequestMessage);
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


        public async Task<Tuple<HttpStatusCode, Newtonsoft.Json.Linq.JObject>> TM4JCall(HttpMethod method, string api, Dictionary<string, string> data)
        {
            /* Get from settings */
            //string baseURL = @"https://unicus-sverige.atlassian.net/rest/atm/1.0/";
            string baseURL = @"https://api.adaptavist.io/tm4j/v2/";
            string authToken = "get from settings";
            /* Get from settings */

            var itemAsJson = JsonConvert.SerializeObject(data);
            var query = new StringContent(itemAsJson);
            var uri = new Uri(baseURL + api);
            var client = GetClient(method, uri, "Bearer", authToken);
            query.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject jsonObj = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.ToString());
                    Debug.WriteLine($"Call OK:{Environment.NewLine}{jsonObj}");
                    return new Tuple<HttpStatusCode, JObject>(HttpStatusCode.OK, jsonObj);
                }
                else
                {
                    Debug.WriteLine("Call failed. Statuscode: " + response.StatusCode);
                    Debug.WriteLine("Request message: " + response.RequestMessage);
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


        public HttpClient GetClientByUserPw(HttpMethod method, Uri uri, string user, string pw)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, uri);
            var byteArray = Encoding.ASCII.GetBytes($"{user}:{pw}");
            var base64token = Convert.ToBase64String(byteArray);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64token);
            client.DefaultRequestHeaders.Authorization = request.Headers.Authorization;
            return client;
        }

        public HttpClient GetClient(HttpMethod method, Uri uri, string authType, string authToken)
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

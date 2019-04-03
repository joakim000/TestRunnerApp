using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

//using Windows.Web.Http;
//using Windows.Web.Http.Filters;
//using Windows.Web.Http.Headers;


namespace TestRunnerAppWpf
{
    class Jira
    {
        //https://unicus-sverige.atlassian.net/jira/software/projects/TA/boards/28
        string baseURLTest = @"https://unicus-sverige.atlassian.net/jira/rest/atm/1.0/";
        string baseURL = @"https://unicus-sverige.atlassian.net/jira/rest/api/2/";



        public async Task<String> CreateJiraCall()
        {
            var postData = new Dictionary<string, string>();
            postData.Add("caller_id", "Driftkontakt Axians AB");
         


            var itemAsJson = JsonConvert.SerializeObject(postData);
            var query = new StringContent(itemAsJson);

            var uri = new Uri("https://axprod.service-now.com/api/now/table/incident");
            string user = "xymon-webservice";
            string pw = "kWDRw5NcAP-M4&$d";

            string r = String.Empty;
            var client = new HttpClient();
            try
            {
                //var request = new HttpRequestMessage("POST", ServiceNowUri, ServiceNowUser, ServiceNowPw);

                string username = user;
                string password = pw;

                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                var byteArray = Encoding.ASCII.GetBytes("{user}:{pw}");
                var base64token = Convert.ToBase64String(byteArray);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64token);
                client.DefaultRequestHeaders.Authorization = request.Headers.Authorization;

                query.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                

                var response = await client.PostAsync(uri, query);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    r = response.Content.ToString();
                    Debug.WriteLine("ServiceNowCall success: " + response.Content.ToString());
                }
                else
                {
                    r = "ServiceNow call failed. Statuscode: " + response.StatusCode;
                    Debug.WriteLine("ServiceNow call failed. Statuscode: " + response.StatusCode);
                    Debug.WriteLine(response.RequestMessage);
                    Debug.WriteLine(response.Content);
                }
            }
            catch (OperationCanceledException e) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "OperationCanceledException: " + e); }
            catch (Exception e) { Debug.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss") + " " + "General exception: " + e); }
            finally
            {
                client.Dispose();
            }
            return r;


        }


        public HttpRequestMessage RequestMessage(string method, Uri uri, string user, string pw)
        {
            HttpMethod m = new HttpMethod(method);
            var request = new HttpRequestMessage(m, uri);

            // Set the header with a strong type.
            if (user != null && pw != null)
            {
                string username = user;
                string password = pw;

                //var bufferUWP = System.Security.Cryptography.CryptographicBuffer.ConvertStringToBinary(username + ":" + password, System.Security.Cryptography.BinaryStringEncoding.Utf8);
                //string base64token = Windows.Security.Cryptography.CryptographicBuffer.EncodeToBase64String(buffer);
                //request.Headers.Authorization = new HttpCredentialsHeaderValue("Basic", base64token);

                var byteArray = Encoding.ASCII.GetBytes("{user}:{pw}");


            }

            // Get the strong type out
            //System.Diagnostics.Debug.WriteLine("One of the Authorization values: {0}={1}",
            //    request.Headers.Authorization.Scheme,
            //    request.Headers.Authorization.Token);

            //// The ToString() is useful for diagnostics, too.
            //System.Diagnostics.Debug.WriteLine("The Authorization ToString() results: {0}", request.Headers.Authorization.ToString());

            return request;
        }

    }
}

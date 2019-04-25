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
    public  partial class Jira
    {
        private  Dictionary<string, object> BuildData(Dictionary<string, object> data, object obj, string name)
        {
            return null;
        }

        public  async Task<Tuple<HttpStatusCode, JObject>> CreateCase(string projectKey,
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

            if (labels != null)
                data.Add("labels", labels);


            var t = TmjCall(HttpMethod.Post, "testcases", data);
            await t;
            return t.Result;
        }



        public  async Task<Tuple<HttpStatusCode, JObject>> CreateCycle(string projectKey,
                                                                             string name, 
                                                                             string description,
                                                                             DateTime? plannedStartDate,
                                                                             DateTime? plannedEndDate,
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

            if (plannedStartDate != null)
                data.Add("plannedStartDate", JiraDate(plannedStartDate));

            if (plannedEndDate != null)
                data.Add("plannedEndDate", JiraDate(plannedEndDate));

            if (jiraProjectVersion != null)
                data.Add("jiraProjectVersion", jiraProjectVersion);

            if (!string.IsNullOrEmpty(statusName))
                data.Add("statusName", statusName);

            if (folderId != null)
                data.Add("folderId", folderId);

            if (!string.IsNullOrEmpty(ownerId))
                data.Add("ownerId", ownerId);


            var t = TmjCall(HttpMethod.Post, "testcycles", data);
            await t;
            return t.Result;
        }

        public  async Task<Tuple<HttpStatusCode, JObject>> CreateExec(string projectKey,
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

            /* Not sure how to work with array */
            //            data.Add("testScriptResults", new Array({
            //"statusName": "Pass",
            //"actualEndDate": "2019-04-06T12:49:03Z",
            //"actualResult": "User logged in successfully"
            //})



            var t = TmjCall(HttpMethod.Post, "testexecutions", data);
            await t;
            return t.Result;
        }

       
    }

}


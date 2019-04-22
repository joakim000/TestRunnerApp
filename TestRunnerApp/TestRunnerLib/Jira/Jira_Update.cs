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
    public partial class Jira
    {

        //public async Task<Tuple<HttpStatusCode, JObject>> UpdateCase(string testCaseKey,
        //                                                             JiraCase.UpdateCase jiraCase)
                                                                            
        //{
        //    var data = new Dictionary<string, object>();

        //    // Required
        //    if (string.IsNullOrEmpty(testCaseKey) || jiraCase == null)
        //        return null;

        //    data.Add("JiraCase", jiraCase);

        //    string path = "/" + testCaseKey;

        //    var t = TmjCall(HttpMethod.Put, "testcases" + path, data);
        //    await t;
        //    return t.Result;
        //}

        private Dictionary<string, object> IdSelfDict(object o)
        {
            var d = new Dictionary<string, object>();
            Type t = o.GetType();
            //object p = new IdSelf();

            if (t == typeof(IdSelf))
            {
                var p = o as IdSelf;
                d.Add("id", p.id);
                d.Add("self", p.self);
            }
            if (t == typeof(JiraProject))
            {
                var p = o as JiraProject;
                d.Add("id", p.id);
                d.Add("self", p.self);
            }
            else if (t == typeof(JiraPrio))
            {
                var p = o as JiraPrio;
                d.Add("id", p.id);
                d.Add("self", p.self);
            }
            else if (t == typeof(JiraStatus))
            {
                var p = o as JiraStatus;
                d.Add("id", p.id);
                d.Add("self", p.self);
            }
            else if (t == typeof(JiraComponent))
            {
                var p = o as JiraComponent;
                d.Add("id", p.id);
                d.Add("self", p.self);
            }
            else if (t == typeof(JiraFolder))
            {
                var p = o as JiraFolder;
                d.Add("id", p.id);
                d.Add("self", p.self);
            }
            return d;
        }



        public async Task<Tuple<HttpStatusCode, JObject>> UpdateCase(int? id,
                                                                      string key,
                                                                      string name,
                                                                      IdSelf project,
                                                                      JiraPrio priority,
                                                                      JiraStatus status,
                                                                      string createdOn,
                                                                      string objective,
                                                                      string precondition,
                                                                      int? estimatedTime,
                                                                      string[] labels,
                                                                      JiraComponent component,
                                                                      JiraFolder folder,
                                                                      JiraUser owner)
        {
            var data = new Dictionary<string, object>();

            // Required
            if (string.IsNullOrEmpty(key) ||
                id == null ||
                string.IsNullOrEmpty(name) ||
                project == null ||
                priority == null ||
                status == null)
                return null;

            string path = "/" + key;
            data.Add("id", id);
            data.Add("key", key);
            data.Add("name", name);
            data.Add("project", IdSelfDict(project));
            data.Add("priority", IdSelfDict(priority));
            data.Add("status", IdSelfDict(status));

            // Optional

            //if (!string.IsNullOrEmpty(createdOn))
            //    data.Add("createdOn", createdOn);
            // Required format:  "createdOn": "2018-05-15T13:15:13.000+0000"


            if (!string.IsNullOrEmpty(objective))
                data.Add("objective", objective);

            if (!string.IsNullOrEmpty(precondition))
                data.Add("precondition", precondition);

            if (estimatedTime != null)
                data.Add("estimatedTime", estimatedTime);

            if (labels != null && labels.Length > 0)
                data.Add("labels", labels);

            if (component != null)
                data.Add("component", IdSelfDict(component));

            if (folder != null)
                data.Add("folder", IdSelfDict(folder));

            if (owner != null)
            {
                var d = new Dictionary<string, string>();
                d.Add("self", owner.self);
                d.Add("accountId", owner.accountId);
                data.Add("owner", d);
            }

            var t = TmjCall(HttpMethod.Put, "testcases" + path, data);
            await t;
            return t.Result;
        }


        public async Task<Tuple<HttpStatusCode, JObject>> UpdateCycle(string testCycleIdOrKey,
                                                                     JiraCycle jiraCycle)

        {
            var data = new Dictionary<string, object>();

            // Required
            if (string.IsNullOrEmpty(testCycleIdOrKey) || jiraCycle == null)
                return null;

            data.Add("JiraCycle", jiraCycle);

            string path = "/" + testCycleIdOrKey;

            var t = TmjCall(HttpMethod.Put, "testcycles" + path, data);
            await t;
            return t.Result;
        }

    }
}


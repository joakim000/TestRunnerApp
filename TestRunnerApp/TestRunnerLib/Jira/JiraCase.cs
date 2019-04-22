using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModelSupport;

namespace TestRunnerLib.Jira
{
    [JsonObject(MemberSerialization.OptOut)]
    public class JiraCase : ViewModelBase
    {
        /* Properties returned by TMJ */
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        
        public string key
        {
            get => Get(() => key);
            set => Set(() => key, value);
        }
        public string name
        {
            get => Get(() => name);
            set => Set(() => name, value);
        }
        public IdSelf project
        {
            get => Get(() => project);
            set => Set(() => project, value);
        }
        public string createdOn
        {
            get => Get(() => createdOn);
            set => Set(() => createdOn, value);
        }
        public DateTime createdOnDT
        {
            get => Get(() => createdOnDT);
            set => Set(() => createdOnDT, value);
        }
        public string objective
        {
            get => Get(() => objective);
            set => Set(() => objective, value);
        }
        public string precondition
        {
            get => Get(() => precondition);
            set => Set(() => precondition, value);
        }
        public int? estimatedTime
        {
            get => Get(() => estimatedTime);
            set => Set(() => estimatedTime, value);
        }

        // labels
        public string[] labels
        {
            get => Get(() => labels);
            set => Set(() => labels, value);
        }

        // Test version, is this not in TMJ API?
        public string version
        {
            get => Get(() => version);
            set => Set(() => version, value);
        }


        public JiraComponent component
        {
            get => Get(() => component);
            set => Set(() => component, value);
        }
        public JiraPrio priority
        {
            get => Get(() => priority);
            set => Set(() => priority, value);
        }
        public JiraStatus status
        {
            get => Get(() => status);
            set => Set(() => status, value);
        }
        public JiraFolder folder
        {
            get => Get(() => folder);
            set => Set(() => folder, value);
        }
        public JiraUser owner
        {
            get => Get(() => owner);
            set => Set(() => owner, value);
        }




        public JiraCase()
        {
            //this.PropertyChanged += JiraCase_PropertyChanged;

        }

        public void JiraCase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"JiraCase property changed: key {key}  property {e.PropertyName}");

            Update();
        }

        //public void TogglePropertyChanged(bool toggleOn)
        //{
        //    if (toggleOn)
        //        if (this.JiraCase_PropertyChanged.Get )
        //    t.jiraCase.PropertyChanged += t.jiraCase.JiraCase_PropertyChanged;
        //}

        public async void Update()
        {
            if (JiraAccessor.jiraObj == null)
                Debug.WriteLine("JiraAccessor.jiraObj was null");
            else
            {
                //var updateObj = new UpdateCase();
                //updateObj.id = id;
                //updateObj.key = key;
                //updateObj.name = name;
                ////updateObj.project = new IdSelf(project.id, project.self);
                ////updateObj.priority = new IdSelf(priority.id, priority.self);
                ////updateObj.status = new IdSelf(status.id, status.self);
                //updateObj.project = new UpdateCase.SelfId(project.id, project.self);
                //updateObj.priority = new UpdateCase.SelfId(priority.id, priority.self);
                //updateObj.status = new UpdateCase.SelfId(status.id, status.self);


                //var response = await JiraAccessor.jiraObj.UpdateCase(key, updateObj);

                var response = await JiraAccessor.jiraObj.UpdateCase(id,
                                                                    key,
                                                                    name,
                                                                    project,
                                                                    priority,
                                                                    status,
                                                                    createdOn,
                                                                    objective,
                                                                    precondition,
                                                                    estimatedTime,
                                                                    labels,
                                                                    component,
                                                                    folder,
                                                                    owner);

                if (response == null)
                   Debug.WriteLine($"JiraCase updated failed early: {key}");
                else if (response.Item1 == HttpStatusCode.OK)
                {
                    Debug.WriteLine($"JiraCase successfully updated: {key}");
                }
                else
                    Debug.WriteLine($"JiraCase update failed: {key}");

            }

        }

        //public class UpdateCase
        //{
        //    //public int id, estimatedTime;
        //    //public string key, name, createdOn, objective, precondition;
        //    //public string[] labels;
        //    //public SelfUrl project, component, priority, status, folder, owner;
        //    public int id; //, estimatedTime;
        //    public string key, name; //, createdOn, objective, precondition;
        //    // public string[] labels;
        //    public SelfId project, priority, status; //, component, folder, owner;
        //    //public SelfUrl project, priority, status; //, component, folder, owner;


        //    public class SelfUrl
        //    {
        //       public string self;

        //        public SelfUrl(string self)
        //        {
        //            this.self = self;
        //        }

        //    }

        //    public class SelfId
        //    {
        //        public string self;
        //        public int id;

        //        public SelfId(int id, string self)
        //        {
        //            this.self = self;
        //            this.id = id;
        //        }

        //    }

        //}


    }


    

}
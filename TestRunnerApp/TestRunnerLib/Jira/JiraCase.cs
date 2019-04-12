using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public IdSelf component
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
        public Owner owner
        {
            get => Get(() => owner);
            set => Set(() => owner, value);
        }




        public JiraCase() { }


    }

}
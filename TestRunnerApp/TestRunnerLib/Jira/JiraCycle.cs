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
    public class JiraCycle : ViewModelBase
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
        public string description
        {
            get => Get(() => description);
            set => Set(() => description, value);
        }
        public string plannedStartDate
        {
            get => Get(() => plannedStartDate);
            set => Set(() => plannedStartDate, value);
        }
        public string plannedEndDate
        {
            get => Get(() => plannedEndDate);
            set => Set(() => plannedEndDate, value);
        }
        public IdSelf project
        {
            get => Get(() => project, new IdSelf());
            set => Set(() => project, value);
        }
        public IdSelf jiraProjectVersion
        {
            get => Get(() => jiraProjectVersion, new IdSelf());
            set => Set(() => jiraProjectVersion, value);
        }
        public JiraStatus status
        {
            get => Get(() => status, new JiraStatus());
            set => Set(() => status, value);
        }
        public IdSelf folder
        {
            get => Get(() => folder, new IdSelf());
            set => Set(() => folder, value);
        }


        public JiraCycle() { }

        
    }

    

}
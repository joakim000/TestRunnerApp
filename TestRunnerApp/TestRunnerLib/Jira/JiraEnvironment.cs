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
    public class JiraEnvironment : ViewModelBase
    {
        /* Properties returned by TMJ */
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public IdSelf project
        {
            get => Get(() => project);
            set => Set(() => project, value);
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
        public int index
        {
            get => Get(() => index);
            set => Set(() => index, value);
        }
        public bool archived
        {
            get => Get(() => archived);
            set => Set(() => archived, value);
        }
        

        public JiraEnvironment() { }


    }

}
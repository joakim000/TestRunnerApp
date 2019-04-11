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
    public class JiraStatus : ViewModelBase
    {
        /* Properties returned by TMJ */
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public IdSelf project
        {
            get => Get(() => project, new IdSelf());
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
        public string color
        {
            get => Get(() => color);
            set => Set(() => color, value);
        }
        public bool archived
        {
            get => Get(() => archived);
            set => Set(() => archived, value);
        }
        public bool isDefault  // key "default" in API
        {
            get => Get(() => isDefault);
            set => Set(() => isDefault, value);
        }


        public string self
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }


        public JiraStatus() { }


    }

}
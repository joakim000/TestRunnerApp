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
    public class IdSelf : ViewModelBase
    {
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public string self
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
        // Status
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
        public string color
        {
            get => Get(() => color);
            set => Set(() => color, value);
        }

        public IdSelf() { }
    }

    

}
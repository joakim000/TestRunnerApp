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
       
        public IdSelf() { }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class Owner : ViewModelBase
    {
        public string accountId
        {
            get => Get(() => accountId);
            set => Set(() => accountId, value);
        }
        public string self
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
       
        public Owner() { }
    }

    

}
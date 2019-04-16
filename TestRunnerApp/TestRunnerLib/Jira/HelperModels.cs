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

    //[JsonObject(MemberSerialization.OptOut)]
    //public class Owner : ViewModelBase
    //{
    //    public string accountId
    //    {
    //        get => Get(() => accountId);
    //        set => Set(() => accountId, value);
    //    }
    //    public string self
    //    {
    //        get => Get(() => self);
    //        set => Set(() => self, value);
    //    }
       
    //    public Owner() { }
    //}

    [JsonObject(MemberSerialization.OptOut)]
    public class JiraUser : ViewModelBase
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
        public string displayName
        {
            get => Get(() => displayName);
            set => Set(() => displayName, value);
        }
        public string mail
        {
            get => Get(() => mail);
            set => Set(() => mail, value);
        }
        public bool active
        {
            get => Get(() => active);
            set => Set(() => active, value);
        }
        public JiraAvatar avatar
        {
            get => Get(() => avatar);
            set => Set(() => avatar, value);
        }
        public JiraUser()
        {
            
        }
    }

        [JsonObject(MemberSerialization.OptOut)]
        public class JiraAvatar : ViewModelBase
        {
            public string url16x16
            {
                get => Get(() => url16x16);
                set => Set(() => url16x16, value);
            }
            public string url24x24
            {
                get => Get(() => url24x24);
                set => Set(() => url24x24, value);
            }
            public string url32x32
            {
                get => Get(() => url32x32);
                set => Set(() => url32x32, value);
            }
            public string url48x48
            {
                get => Get(() => url48x48);
                set => Set(() => url48x48, value);
            }

            public JiraAvatar()
            {
            }
        }
}
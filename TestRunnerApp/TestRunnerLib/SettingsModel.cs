using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ViewModelSupport;

namespace TestRunnerLib
{
    [JsonObject(MemberSerialization.OptOut)]
    public class SettingsModel : ViewModelBase
    {
        public string jiraBase
        {
            get => Get(() => jiraBase);
            set => Set(() => jiraBase, value);
        }
        public string tm4jBase
        {
            get => Get(() => tm4jBase);
            set => Set(() => tm4jBase, value);
        }
        public string user
        {
            get => Get(() => user);
            set => Set(() => user, value);
        }
        public string pw
        {
            get => Get(() => pw);
            set => Set(() => pw, value);
        }
        public string token
        {
            get => Get(() => token);
            set => Set(() => token, value);
        }
        
        public SettingsModel() { }

    }
}

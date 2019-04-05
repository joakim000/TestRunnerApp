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
        public string jiraUser
        {
            get => Get(() => jiraUser);
            set => Set(() => jiraUser, value);
        }
        public string jiraUserId
        {
            get => Get(() => jiraUserId);
            set => Set(() => jiraUserId, value);
        }
        public string jiraToken
        {
            get => Get(() => jiraToken);
            set => Set(() => jiraToken, value);
        }


        public string tmjBase
        {
            get => Get(() => tmjBase);
            set => Set(() => tmjBase, value);
        }
        public string TMJid
        {
            get => Get(() => TMJid);
            set => Set(() => TMJid, value);
        }

        public string TMJkey
        {
            get => Get(() => TMJkey);
            set => Set(() => TMJkey, value);
        }



        public SettingsModel() { }
    }
}

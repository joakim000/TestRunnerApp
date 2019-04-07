using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRunnerLib;
using ViewModelSupport;

namespace TestRunnerAppWpf
{
    class SettingsManagerViewModel : ViewModelBase
    {
        public bool jiraCloudMgmt
        {
            get => Get(() => jiraCloudMgmt, false);
            set => Set(() => jiraCloudMgmt, value);
        }
        public string jiraUser
        {
            get => Get(() => jiraUser);
            set => Set(() => jiraUser, value);
        }
        public string jiraToken
        {
            get => Get(() => jiraToken);
            set => Set(() => jiraToken, value);
        }
        public string tmjIdToken
        {
            get => Get(() => tmjIdToken);
            set => Set(() => tmjIdToken, value);
        }
        public string tmjKeyToken
        {
            get => Get(() => tmjKeyToken);
            set => Set(() => tmjKeyToken, value);
        }



        public bool reqTestMgmt
        {
            get => Get(() => reqTestMgmt, false);
            set => Set(() => reqTestMgmt, value);
        }


    }
}

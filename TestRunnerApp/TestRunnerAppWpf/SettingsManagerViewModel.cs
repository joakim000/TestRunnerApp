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
        public string[] optionsArray = { "None", "Jira Cloud with TM4J", "ReqTest" };

        public string[] mgmtOptions_old
        {
            get => Get(() => mgmtOptions_old, optionsArray);
            set => Set(() => mgmtOptions_old, value);
        }


        public List<Managment> mgmtOptions
        {
            get => Get(() => mgmtOptions, Enums.Mgmt);
            set => Set(() => mgmtOptions, value);
        }
        public bool jiraCloudMgmt
        {
            get => Get(() => jiraCloudMgmt, false);
            set => Set(() => jiraCloudMgmt, value);
        }
        public bool reqTestMgmt
        {
            get => Get(() => reqTestMgmt, false);
            set => Set(() => reqTestMgmt, value);
        }
        //public Mgmt mgmt
        //{
        //    get => Get(() => mgmt, Mgmt.None);
        //    set => Set(() => mgmt, value);
        //}
        public Managment mgmt
        {
            get => Get(() => mgmt, Enums.Mgmt.Find(x => x.key == "None"));
            set => Set(() => mgmt, value);
        }




        public string jiraInstance
        {
            get => Get(() => jiraInstance);
            set => Set(() => jiraInstance, value);
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



        //public void Execute_SetFirefoxCmd()
        //{
        //    checkedChrome = false; checkedFirefox = true; checkedIE = false; checkedPhantomJS = false; checkedEdge = false;
        //    Properties.Settings.Default.WebDriver = "firefox";
        //}
        //public bool CanExecute_SetFirefoxCmd()
        //{
        //    return true;
        //}

        

    }
}

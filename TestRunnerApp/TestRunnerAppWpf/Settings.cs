using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestRunnerLib;
using TestRunnerLib.Jira;

namespace TestRunnerAppWpf
{
    public static class Settings
    {
        // Settings from Suite
        //public static string jiraInstance = @"unicus-sverige.atlassian.net";
        public static string jiraProject = @"TEM";
        public static string jiraFolder = @"";

        public static ObservableCollection<JiraProject> jiraAvailableProjects = new ObservableCollection<JiraProject>();

        public static bool accountIdSet = false;
        //public static string jiraAccountId;
        

        public static WebDriverType GetWebDriverType()
        {
            WebDriverType driver = WebDriverType.None;
            try
            {
                switch (Properties.Settings.Default.WebDriver)
                {
                    case "chrome":
                        driver = WebDriverType.Chrome;
                        break;
                    case "firefox":
                        driver = WebDriverType.Firefox;
                        break;
                    case "ie":
                        driver = WebDriverType.IE;
                        break;
                    default:
                        driver = WebDriverType.None;
                        break;
                }
            }
            catch (NullReferenceException ex)
            {
                // In case of problems with settings-file
                Debug.WriteLine($"Null-ref getting webdriversetting: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General exception getting webdriversetting:: {ex.Message}");
            }
            Debug.WriteLine($"GetWebDriverType returned: {driver.ToString()}");

            return driver;
        }


        public static void GetSettings(MainViewModel caller)
        {
            try
            {
                // Get WebDriver-setting
                switch (Settings.GetWebDriverType())
                {
                    case WebDriverType.Chrome:
                        caller.Execute_SetChromeCmd();
                        break;
                    case WebDriverType.Firefox:
                        caller.Execute_SetFirefoxCmd();
                        break;
                    case WebDriverType.IE:
                        caller.Execute_SetIECmd();
                        break;
                    default:
                        caller.Execute_SetChromeCmd();
                        break;
                }

                // Get on-top-when-running setting
                caller.checkedOnTop = Properties.Settings.Default.OnTop;

                caller.multiCycleView = Properties.Settings.Default.multiCycleView;

                switch (Properties.Settings.Default.Theme)
                {
                    case "Light":
                        Themes.SetLight();
                        caller.checkedDarkTheme = false;
                        break;
                    case "Dark":
                        Themes.SetDark();
                        caller.checkedDarkTheme = true;
                        break;
                    default:
                        Themes.SetLight();
                        caller.checkedDarkTheme = false;
                        break;
                }



                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraInstance))
                    caller.jiraInstance = Properties.Settings.Default.JiraInstance;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraUser))
                    caller.jiraUser = Properties.Settings.Default.JiraUser;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraAccountId))
                {
                    caller.jiraAccountId = Properties.Settings.Default.JiraAccountId;
                    Settings.accountIdSet = true;
                }
                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraToken))
                    caller.jiraToken = Properties.Settings.Default.JiraToken;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.TmjIdToken))
                    caller.tmjIdToken = Properties.Settings.Default.TmjIdToken;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.TmjKeyToken))
                    caller.tmjKeyToken = Properties.Settings.Default.TmjKeyToken;


                if (Properties.Settings.Default.MgmtSystem == "None")
                {
                    caller.jiraCloudMgmt = false;
                    caller.reqTestMgmt = false;
                }
                else if (Properties.Settings.Default.MgmtSystem == "Jira Cloud with TM4J")
                {
                    caller.jiraCloudMgmt = true;
                    caller.reqTestMgmt = false;
                    //if (string.IsNullOrEmpty(caller.jiraAccountId))
                    //{
                    //    if (!await Jira.SetAccountId())
                    //        MessageBox.Show("Error retrieving Account ID.", "TestAppRunner with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                }
                else if (Properties.Settings.Default.MgmtSystem == "ReqTest")
                {
                    caller.jiraCloudMgmt = false;
                    caller.reqTestMgmt = true;
                }

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraAvailableProjects))
                {
                    //var c = new ObservableCollection<JiraProject>();
                    var tokenArray = (JArray)FileMgmt.Deserialize(Properties.Settings.Default.JiraAvailableProjects);
                    foreach (JToken t in tokenArray)
                        jiraAvailableProjects.Add(t.ToObject<JiraProject>());

                    //caller.detailsViewModel.jiraAvailableProjects.Add(t.ToObject<JiraProject>());
                    //caller.detailsViewModel.jiraAvailableProjects = c;
                    
                }



            }
            catch (NullReferenceException e)
            {
                // In case of problems with settings-file
                Debug.WriteLine($"Null-ref getting settings: {e.Message}");
            }
        }

    }
}

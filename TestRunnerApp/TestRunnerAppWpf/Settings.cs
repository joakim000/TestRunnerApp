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
                caller.showLog = Properties.Settings.Default.ShowLog;
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


                /* Check if we should enable JiraCloudTmj */
                bool JiraCloudTmj_enabled = true;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraInstance))
                    caller.jiraInstance = Properties.Settings.Default.JiraInstance;
                else JiraCloudTmj_enabled = false;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraUser))
                    caller.jiraUser = Properties.Settings.Default.JiraUser;
                else JiraCloudTmj_enabled = false;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraAccountId))
                {
                    caller.jiraAccountId = Properties.Settings.Default.JiraAccountId;
                    Settings.accountIdSet = true;
                }
                // else JiraCloudTmj_enabled = false; // Enable anyway and try to get accountid in preflight

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraToken))
                    caller.jiraToken = Properties.Settings.Default.JiraToken;
                else JiraCloudTmj_enabled = false;

                //if (!string.IsNullOrEmpty(Properties.Settings.Default.TmjIdToken))
                //    caller.tmjIdToken = Properties.Settings.Default.TmjIdToken;
                //else JiraCloudTmj_enabled = false;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.TmjKeyToken))
                    caller.tmjKeyToken = Properties.Settings.Default.TmjKeyToken;
                else JiraCloudTmj_enabled = false;

                Enums.Mgmt.Find(x => x.key == "JiraCloudTmj").enabled = JiraCloudTmj_enabled;
                /* end: Check if we should enable JiraCloudTmj */



                /* Deprecated - Mgmt now set per suite */
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
                /* Deprecated - Mgmt now set per suite */

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraAvailableProjects))
                {
                    var tokenArray = (JArray)FileMgmt.Deserialize(Properties.Settings.Default.JiraAvailableProjects);
                    foreach (JToken t in tokenArray)
                        jiraAvailableProjects.Add(t.ToObject<JiraProject>());
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

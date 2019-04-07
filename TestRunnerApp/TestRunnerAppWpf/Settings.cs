using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public static class Settings
    {
        public static string[] mgmtOptions = { "None", "Jira Cloud with TM4J", "ReqTest" };

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

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraUser))
                    caller.jiraUser = Properties.Settings.Default.JiraUser;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraToken))
                    caller.jiraToken = Properties.Settings.Default.JiraToken;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.TmjIdToken))
                    caller.tmjIdToken = Properties.Settings.Default.TmjIdToken;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.TmjKeyToken))
                    caller.tmjKeyToken = Properties.Settings.Default.TmjKeyToken;


            }
            catch (NullReferenceException e)
            {
                // In case of problems with settings-file
                Debug.WriteLine($"Null-ref getting settings: {e.Message}");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewModelSupport;
using TestRunnerLib;

namespace TestRunnerAppCon
{
    public static class Settings
    {
        // Report
        public static string[] columns { get; set; }
        // Run
        public static WebDriverType webDriverType {get; set;}
        // Mail
        public static string smtpHost { get; set; }
        public static int smtpPort { get; set; }
        public static int smtpTimeout { get; set; }
        public static bool enableSsl { get; set; }
        public static bool useDefaultCredentials { get; set; }
        public static string sendFrom { get; set; }
        public static string subject { get; set; }
        public static string sendFromAccount { get; set; }
        public static string sendFromAccountPw { get; set; }
        // Test managment
        //public static string mgmt { get; set; }
        public static bool exportAfterExecution { get; set; }
        public static bool createNewCycle { get; set; }
        // JiraCloudTmj
        public static string jiraCloudInstance { get; set; }
        public static string jiraCloudUser { get; set; }
        public static string jiraCloudKey { get; set; }
        public static string tmjCloudToken { get; set; }


    }

    public class SettingsManager : ViewModelBase
    {
        // Run
        public string webDriverType
        {
            get => Get(() => webDriverType);
            set => Set(() => webDriverType, value);
        }
        // Report columns
        public string columns
        {
            get => Get(() => columns);
            set => Set(() => columns, value);
        }
        // Mail
        public string smtpHost
        {
            get => Get(() => smtpHost);
            set => Set(() => smtpHost, value);
        }
        public int smtpPort
        {
            get => Get(() => smtpPort);
            set => Set(() => smtpPort, value);
        }
        public int smtpTimeout
        {
            get => Get(() => smtpTimeout);
            set => Set(() => smtpTimeout, value);
        }
        public bool enableSsl
        {
            get => Get(() => enableSsl);
            set => Set(() => enableSsl, value);
        }
        public bool useDefaultCredentials
        {
            get => Get(() => useDefaultCredentials);
            set => Set(() => useDefaultCredentials, value);
        }
        public string sendFrom
        {
            get => Get(() => sendFrom);
            set => Set(() => sendFrom, value);
        }
        public string subject
        {
            get => Get(() => subject);
            set => Set(() => subject, value);
        }
        public string sendFromAccount
        {
            get => Get(() => sendFromAccount);
            set => Set(() => sendFromAccount, value);
        }
        public string sendFromAccountPw
        {
            get => Get(() => sendFromAccountPw);
            set => Set(() => sendFromAccountPw, value);
        }
        // Test managment
        //public string mgmt
        //{
        //    get => Get(() => mgmt);
        //    set => Set(() => mgmt, value);
        //}
        public bool exportAfterExecution
        {
            get => Get(() => exportAfterExecution);
            set => Set(() => exportAfterExecution, value);
        }
        public bool createNewCycle
        {
            get => Get(() => createNewCycle);
            set => Set(() => createNewCycle, value);
        }
        // JiraCloudTmj
        public string jiraCloudInstance
        {
            get => Get(() => jiraCloudInstance);
            set => Set(() => jiraCloudInstance, value);
        }
        public string jiraCloudUser
        {
            get => Get(() => jiraCloudUser);
            set => Set(() => jiraCloudUser, value);
        }
        public string jiraCloudKey
        {
            get => Get(() => jiraCloudKey);
            set => Set(() => jiraCloudKey, value);
        }
        public string tmjCloudToken
        {
            get => Get(() => tmjCloudToken);
            set => Set(() => tmjCloudToken, value);
        }


        public void Transfer()
        {
            // Report
            Settings.columns = columns.Split(' ');
            // Run
            Settings.webDriverType = Report.readWebDriver(webDriverType);
            // Mail
            Settings.smtpHost = smtpHost;
            Settings.smtpPort = smtpPort;
            Settings.smtpTimeout = smtpTimeout;
            Settings.enableSsl = enableSsl;
            Settings.useDefaultCredentials = useDefaultCredentials;
            Settings.sendFrom = sendFrom;
            Settings.subject = subject;
            Settings.sendFromAccount = sendFromAccount;
            Settings.sendFromAccountPw = sendFromAccountPw;
            // Test managment
            //Settings.mgmt = mgmt;
            Settings.exportAfterExecution = exportAfterExecution;
            Settings.createNewCycle = createNewCycle;
            // JiraCloudTmj
            Settings.jiraCloudInstance = jiraCloudInstance;
            Settings.jiraCloudUser = jiraCloudUser;
            Settings.jiraCloudKey = jiraCloudKey;
            Settings.tmjCloudToken = tmjCloudToken;
        }


    }

    public class SettingsGetter 
    {
        public void Restore(SettingsManager sm)
        {
            //SettingsManager sm = new SettingsManager();

            string fileToOpen = AppDomain.CurrentDomain.BaseDirectory + "TRAC.cfg";

            string configImport = String.Empty;

            try
            {
                configImport = File.ReadAllText(fileToOpen);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error opening file: {e}");
            }

            Match m;
            string importPattern;
            string patternPre = @"(?<=(?<!#)";
            string patternPost = @"="").+?(?=""\s*(#|\r|\n))";
            var settings = sm.GetType().GetProperties();
            //var settings2 = Settings.GetType().GetProperties();
            foreach (PropertyInfo setting in settings)
            {
                //Debug.WriteLine("sm." + setting.Name + " " + setting.PropertyType);

                importPattern = patternPre + setting.Name + patternPost;
                m = Regex.Match(configImport, importPattern);
                if (m.Success)
                {
                    if (setting.PropertyType.FullName == "System.String")
                    {
                        sm.GetType().GetProperty(setting.Name).SetValue(sm, m.Value, null);
                        Debug.WriteLine("Import setting: " + setting.Name + "=" + m.Value + "  type: System.String");
                    }
                    else if (setting.PropertyType.FullName == "System.Int32")
                    {
                        if (Int32.TryParse(m.Value, out int parseVal))
                        {
                            sm.GetType().GetProperty(setting.Name).SetValue(sm, parseVal, null);
                            Debug.WriteLine("Import setting: " + setting.Name + "=" + m.Value + "  type: System.Int32");
                        }
                        else
                            Debug.WriteLine("Import setting: " + setting.Name + " found: " + m.Value + "  type: System.Int32 - FAILED TO PARSE");
                    }
                    else if (setting.PropertyType.FullName == "System.Boolean")
                    {
                        if (Boolean.TryParse(m.Value, out bool parseVal))
                        {
                            sm.GetType().GetProperty(setting.Name).SetValue(sm, parseVal, null);
                            Debug.WriteLine("Import setting: " + setting.Name + "=" + m.Value + "  type: System.Boolean");
                        }
                        else
                            Debug.WriteLine("Import setting: " + setting.Name + " found: " + m.Value + "  type: System.Boolean - FAILED TO PARSE");
                    }
                    else
                        Debug.WriteLine("Import setting: " + setting.Name + " - unknown type: " + setting.PropertyType.FullName);

                }
                else
                {
                    Debug.WriteLine("Import setting: " + setting.Name + " - not found");
                    Console.WriteLine("Config: " + setting.Name + " - not found");
                }
            }
            sm.Transfer();
        }


    }

}

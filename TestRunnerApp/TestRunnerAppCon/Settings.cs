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

namespace TestRunnerAppCon
{
    public static class Settings
    {

        //public string sendFrom
        //{
        //    get => Get(() => sendFrom);
        //    set => Set(() => sendFrom, value);
        //}

        public static string sendFrom { get; set; }
        public static string sendFromPw { get; set; }
        

    }

    public class SettingsManager : ViewModelBase
    {

        public string sendFrom
        {
            get => Get(() => sendFrom);
            set => Set(() => sendFrom, value);
        }

        public string sendFromPw
        {
            get => Get(() => sendFromPw);
            set => Set(() => sendFromPw, value);
        }

        //public static string sendFrom { get; set; }
        //public static string sendFromPw { get; set; }

        public void Transfer()
        {
            Settings.sendFrom = sendFrom;
            Settings.sendFromPw = sendFromPw;

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
            string patternPost = @"="").+?(?=""(\r|\n))";
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
                }
            }
            sm.Transfer();
        }


    }

}

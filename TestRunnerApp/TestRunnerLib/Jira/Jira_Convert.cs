using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
using ViewModelSupport;

namespace TestRunnerLib.Jira
{
    public static partial class Jira
    {
        
        private static string Outcome2string(Outcome o)
        {
            switch (o)
            {
                case Outcome.Warning:
                    return "Warning";
                case Outcome.Pass:
                    return "Pass";
                case Outcome.Fail:
                    return "Fail";
                case Outcome.NotRun:
                    return "Not Executed";
                default:
                    return "Unknown";

            }
        }

        private static string Outcome2string(Outcome? nullableO)
        {
            if (nullableO == null)
                return null;
            else
            {
                Outcome o = (Outcome)nullableO;
                switch (o)
                {
                    case Outcome.Warning:
                        return "Warning";
                    case Outcome.Pass:
                        return "Pass";
                    case Outcome.Fail:
                        return "Fail";
                    case Outcome.NotRun:
                        return "Not Executed";
                    default:
                        return "Unknown";

                }
            }
        }

        private static string WebDriverType2string(WebDriverType w)
        {
            switch (w)
            {
                case WebDriverType.Chrome:
                    return "Chrome";
                case WebDriverType.Firefox:
                    return "Firefox";
                default:
                    return "Unknown";

            }
        }

        private static string WebDriverType2string(WebDriverType? nullableWdt)
        {
            if (nullableWdt == null)
                return null;
            else
            {
                WebDriverType wdt = (WebDriverType)nullableWdt;
                switch (wdt)
                {
                    case WebDriverType.Chrome:
                        return "Chrome";
                    case WebDriverType.Firefox:
                        return "Firefox";
                    default:
                        return "Unknown";

                }
            }
        }

        private static string JiraDate(DateTime? nullableDt)
        {
            if (nullableDt == null)
                return null;
            else
            {
                DateTime dt = (DateTime)nullableDt;
                return dt.ToUniversalTime().ToString("s") + "Z";
            }
        } 

    }

}


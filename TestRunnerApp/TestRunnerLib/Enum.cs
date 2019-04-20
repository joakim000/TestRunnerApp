using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerLib
{
    public enum Outcome { Pass, Fail, Warning, NotRun };

    public enum TestKind { Other, Web, AD, Exchange, ADO };

    public enum WebDriverType { None, Chrome, Firefox, IE, PhantomJS, Edge };

    public enum Api { Jira, Tmj }

    //public enum Mgmt { None, JiraCloudTmj, ReqTest }

    public static class Enums
    {
        private static Managment[] mgmtInit = { new Managment("None", "None", true),
                                               new Managment("JiraCloudTmj", "Jira Cloud with Adaptavist TM4J", false),
                                               //new Managment("Foo", "Foo management", true),
                                               //new Managment("Bar", "Bar management", true),
                                               //new Managment("JiraServerTmj", "Jira Server with Adaptavist TM4J", true),
                                               new Managment("ReqTest", "ReqTest", false)
                                                };

        public static List<Managment> Mgmt { get; set; } = new List<Managment>(mgmtInit);
    }

    public class Managment
    {
        public string key { get; }
        public string name { get; }
        public bool enabled { get; set; }

        public Managment(string key, string name, bool enabled)
        {
            this.key = key;
            this.name = name;
            this.enabled = enabled;
        }

    }
}

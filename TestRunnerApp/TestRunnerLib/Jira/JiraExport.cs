using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net;

using Newtonsoft.Json.Linq;
using TestRunnerLib;
using TestRunnerLib.Jira;
using System.Collections.ObjectModel;
using ViewModelSupport;

namespace TestRunnerLib.Jira
{
    public class JiraExport
    {
        private Jira Jira { get; set; }

        public JiraExport(Jira jira)
        {
            Jira = jira;
        }

      

    }
}


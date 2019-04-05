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

}

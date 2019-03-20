using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerLib
{
    public interface IWebTest
    {
        TestResult Test(WebDriverType webDriverType, string param, string param2, string param3, string param4);
    }
}

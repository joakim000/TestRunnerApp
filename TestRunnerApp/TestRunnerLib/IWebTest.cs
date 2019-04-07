using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerLib
{
    public interface IWebTest
    {
        TestResult Test(WebDriverType webDriverType, string[] testData);
    }
}

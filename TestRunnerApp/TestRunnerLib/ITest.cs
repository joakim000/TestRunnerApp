using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerLib
{
    public interface ITest
    {
        TestResult Test(string[] testData);
        TestKind Kind { get; } // { Other, Web, AD, Exchange, ADO };
    }
}

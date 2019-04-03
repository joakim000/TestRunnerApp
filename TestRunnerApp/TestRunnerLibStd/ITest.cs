using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerLib
{
    public interface ITest
    {
        TestKind Kind { get; } // { Other, AD, Exchange }
        TestResult Test(string param, string param2, string param3, string param4);
    }
}

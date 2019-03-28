using AppDomainToolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace TestRunnerLib
{
    class Runner8
    {
        public class Caller : MarshalByRefObject
        {
            private readonly Type called;

            public Caller(
                       string assName,
                       string namespaceName,
                       string typeName)
            {
                called = Type.GetType(namespaceName + "." + typeName + "," + assName);
            }

            public TestResult RunTest(WebDriverType webDriver, string param, string param2, string param3, string param4)
            {
                if (called == null)
                    return new TestResult(Outcome.Warning, "Type not found. Check namespace / type.");

                Debug.WriteLine(called.Name);
                string message = called.Name.ToString();

                // This will fall through if no match is found.
                TestResult tr = new TestResult(Outcome.Warning, $"{called} is not a test: {message}");

                object callObj;
                callObj = Activator.CreateInstance(called) as ITest;
                if (callObj != null)
                {
                    Debug.WriteLine($"{called} is ITest");
                    ITest test = (ITest)callObj;
                    tr = test.Test(param, param2, param3, param4);
                    tr.kind = test.Kind;
                    return tr;
                }
                callObj = Activator.CreateInstance(called) as IWebTest;
                if (callObj != null)
                {
                    Debug.WriteLine($"{called} is IWebTest");
                    IWebTest test = (IWebTest)callObj;
                    tr = test.Test(webDriver, param, param2, param3, param4);
                    tr.webDriver = webDriver;
                    tr.kind = TestKind.Web;
                    return tr;
                }
                return tr;
            }
        }

        public static TestResult InvokeTest(
                       string assName,
                       string namespaceName,
                       string typeName,
                       WebDriverType webDriver,
                       string param1,
                       string param2,
                       string param3,
                       string param4)
        {
            string stringInput = param1 == null ? string.Empty : param1;
            string stringInput2 = param2 == null ? string.Empty : param2;
            string stringInput3 = param3 == null ? string.Empty : param3;
            string stringInput4 = param4 == null ? string.Empty : param4;

            try
            {
                string assFile = AppDomain.CurrentDomain.BaseDirectory + @"Tests\" + assName + ".dll";
                //Debug.WriteLine(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.TestsDir + assName + ".dll");


                var rootDir = AppDomain.CurrentDomain.BaseDirectory;
                var setupInfo = new AppDomainSetup()
                {
                    ApplicationName = "Runner",
                    ApplicationBase = rootDir,
                    PrivateBinPath = rootDir + @"Tests\",
                    ShadowCopyFiles = "true"
                };

                using (var context = AppDomainContext.Create(setupInfo))
                {
                    context.LoadAssembly(LoadMethod.LoadFrom, assFile);

                    var remote = RemoteFunc.Invoke(
                        context.Domain,
                        assName,
                        namespaceName,
                        typeName,
                        (_assName, _namespaceName, _typeName) =>
                        {   
                            return new Caller(_assName, _namespaceName, _typeName);
                        });

                    // Executes in the remote domain.
                    return remote.RunTest(webDriver, stringInput, stringInput2, stringInput3, stringInput4);
                }
            }
            catch (System.TypeLoadException ex)
            {
                return new TestResult(Outcome.Warning, "Type not found. Check namespace / type.", ex);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                return new TestResult(Outcome.Warning, "File not found. Check assembly.", ex);
            }
            catch (Exception ex)
            {
                return new TestResult(Outcome.Warning, ex);
            }

        }

    }
}


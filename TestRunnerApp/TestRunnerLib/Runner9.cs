using AppDomainToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TestRunnerLib
{
    class Runner9
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

            public TestResult RunTest(WebDriverType webDriver, string[] testdata)
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
                    tr = test.Test(testdata);
                    tr.kind = test.Kind;
                }
                callObj = Activator.CreateInstance(called) as IWebTest;
                if (callObj != null)
                {
                    Debug.WriteLine($"{called} is IWebTest");
                    IWebTest test = (IWebTest)callObj;
                    tr = test.Test(webDriver, testdata);
                    tr.webDriver = webDriver;
                    tr.kind = TestKind.Web;
                }

                // Exceptions returned from IronPython causes System.ArgumentNullException on serialization of Data-property
                // Will just clear it for now. TODO: Check if we need to clear it. Alt. serialize it some other way (XMLSerializer?)
                if (tr.e != null)
                    tr.e.Data.Clear();

                return tr;
            }
        }

        public static TestResult InvokeTest(
                       string assName,
                       string namespaceName,
                       string typeName,
                       WebDriverType webDriver,
                       ObservableCollection<TestDataItem> testdata)
        {
            int highestIndex = 0;
            if (testdata.Count > 0)
                highestIndex = testdata.Max<TestDataItem, int>(x => x.index);
            string[] td = new string[highestIndex + 1];
            foreach (TestDataItem tdi in testdata)
            {
                td[tdi.index] = tdi.data;
            }
            
            //for (int i = 0; i < td.Length; i++) 
            //{
            //    td[i] = string.IsNullOrEmpty(testdata[i]) ? string.Empty : testdata[i];
            //}
                
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
                    return remote.RunTest(webDriver, td);
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
            //catch (Exception ex)
            //{
            //    return new TestResult(Outcome.Warning, ex);
            //}

        }

    }
}


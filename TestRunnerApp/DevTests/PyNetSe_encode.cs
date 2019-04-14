
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

/// <summary>
///     Experimental code for running python test scripts 
/// </summary>
/// 
/// <todo>
///     DONE: Access selenium module from script
///     DONE: Receive exceptions raised in script
///     DONE: Access enums from script, workaround with switch statements for now
/// </todo>
/// 
/// <remarks>
///     Copy both .dll and .py to Tests directory
/// 
///     Required:
///         Install-Package IronPython -Version 2.7.9
///     Possibly helpful:
///         Install-Package IronPython.StdLib -Version 2.7.9
/// </remarks>


namespace PythonTest
{
    // Python
    using Microsoft.Scripting;
    using Microsoft.Scripting.Hosting;
    using Microsoft.Scripting.Utils;
    using IronPython.Hosting;

    // TestRunnerApp
    using TestRunnerLib;

    // Selenium
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    

    public class PyNetSe_encode : IWebTest
    {
        public TestResult Test(WebDriverType webDriverType, string[] testData)
        {
            ScriptEngine engine = Python.CreateEngine();

            // For py modules
            ICollection<string> searchPaths = engine.GetSearchPaths();
            searchPaths.Add("..\\.."); // project root
            // Add module paths as needed:
            //searchPaths.Add(""); 
            engine.SetSearchPaths(searchPaths);

            // Create python module TestApp that contains Outcome and TestResult
            var testModuleSource = String.Join(
                Environment.NewLine,
                "import clr",
               $"clr.AddReferenceToFileAndPath(r'{typeof(Outcome).Assembly.Location}')",
               $"clr.AddReferenceToFileAndPath(r'{typeof(TestResult).Assembly.Location}')",
               $"import {typeof(Outcome).FullName} as Outcome",
               $"import {typeof(TestResult).FullName} as TestResult",
               $"clr.AddReferenceToFileAndPath(r'WebDriver.dll')",
               $"clr.AddReferenceToFileAndPath(r'WebDriver.Support.dll')"
            );
            var testModule = engine.CreateModule("TestAppLib");
            engine.Execute(testModuleSource, testModule);

            // Scope for variables
            ScriptScope scope = engine.CreateScope();

            // Script
            ScriptSource script = engine.CreateScriptSourceFromFile("Tests\\PyNetSe_encode.py");

            // Run variables, to be overwritten by script return
            int step = 0;
            string message = String.Empty;
            Outcome outcome = Outcome.NotRun;

            /* Access from script */
            // Make sure we can get these later
            scope.SetVariable("outcome", outcome);
            scope.SetVariable("step", step);
            scope.SetVariable("message", message);
            // Actual input params
            scope.SetVariable("testData", testData);

            try
            {
                using (IWebDriver driver = WebDriver.Get(webDriverType))
                {
                    scope.SetVariable("webdriver", driver);
                    script.Execute(scope);
                    TestResult result = scope.GetVariable<TestResult>("result");

                    // Exceptions returned from IronPython causes System.ArgumentNullException on serialization of Data-property
                    // Will just clear it for now. TODO: Check if we need to clear it. Alt. serialize it some other way.
                    if (result.e != null)
                        result.e.Data.Clear();

                    return result;
                }
            }

            // Web driver error
            catch (OpenQA.Selenium.DriverServiceNotFoundException ex)
            {
                return new TestResult(Outcome.Warning, step, ex);
            }
            // Selenium exception during action
            catch (OpenQA.Selenium.WebDriverException ex)
            {
                return new TestResult(Outcome.Fail, step, ex);
            }
            // Other errors
            catch (Exception ex)
            {
                return new TestResult(Outcome.Warning, step, ex);
            }

            
        }
    }
}
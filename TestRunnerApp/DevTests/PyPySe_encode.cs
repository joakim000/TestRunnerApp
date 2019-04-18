
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
/// 
///     Workaround implemented: Receive exceptions raised in script    
///     DONE: Access selenium module from script
///     DONE: Access enums from script, workaround with switch statements for now
/// </todo>
/// 
/// <remarks>
///     Install IronPython from https://ironpython.net/  and edit path to Lib below
///     Install Selenium module using pip or easy_install, edit path to module below
///     Install-Package IronPython -Version 2.7.9
/// 
///     Copy both .dll and .py to Tests directory
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
    

    public class PyPySe_encode : IWebTest
    {
        public TestResult Test(WebDriverType webDriverType, string[] testData)
        {
            ScriptEngine engine = Python.CreateEngine();

            // Python paths
            ICollection<string> searchPaths = engine.GetSearchPaths();
            // Project root
            searchPaths.Add("..\\.."); // 
           // IronPython lib
            searchPaths.Add(@"C:\Program Files\IronPython 2.7\Lib");
            // Selenium module
            searchPaths.Add(@"C:\Program Files\IronPython 2.7\Lib\site-packages");
            //searchPaths.Add(@"C:\Users\joakim\AppData\Local\Programs\Python\Python37-32\Lib\site-packages");
            engine.SetSearchPaths(searchPaths);

            // Create python module that contains Outcome, WebDriverType, TestResult
            var testModuleSource = String.Join(
                Environment.NewLine,
                "import clr",
               $"clr.AddReferenceToFileAndPath(r'{typeof(Outcome).Assembly.Location}')",
               $"clr.AddReferenceToFileAndPath(r'{typeof(TestResult).Assembly.Location}')",
               $"clr.AddReferenceToFileAndPath(r'{typeof(WebDriverType).Assembly.Location}')",
               $"import {typeof(Outcome).FullName} as Outcome",
               $"import {typeof(TestResult).FullName} as TestResult",
               $"import {typeof(WebDriverType).FullName} as WebDriverType"
            );
            var testModule = engine.CreateModule("TestAppLib");
            engine.Execute(testModuleSource, testModule);

            // Scope for variables
            ScriptScope scope = engine.CreateScope();

            // Script
            ScriptSource script = engine.CreateScriptSourceFromFile("Tests\\PyPySe_encode.py");

            // Run variables, to be overwritten by script return
            int step = 0;
            string message = String.Empty;
            Outcome outcome = Outcome.NotRun;
            Exception rex = null;

            var los = new List<string>();
            foreach (string s in testData)
            {
                Debug.WriteLine("string: " + s);
                byte[] bytes = Encoding.Default.GetBytes(s);
                los.Add(Encoding.UTF7.GetString(bytes));
                Debug.WriteLine("handled string: " + los.Last());
            }
            testData = los.ToArray();

            //byte[] bytes = Encoding.Unicode.GetBytes(testData[2]);
            //testData[2] = Encoding.UTF8.GetString(bytes);


            /* Access from script */
            // Make sure we can get these later
            scope.SetVariable("outcome", outcome);
            scope.SetVariable("step", step);
            scope.SetVariable("message", message);
            scope.SetVariable("rex", rex);
            // Actual input params
            scope.SetVariable("webdrivertype", webDriverType);
            scope.SetVariable("testData", testData);


            

            try
            {
               script.Execute(scope);
               TestResult result = scope.GetVariable<TestResult>("result");

                // Work around any mistakenly returned Python exceptions
                result.e = null;

                // Exception data from Python exception string
                int exIndex = result.message.IndexOf("Exception:");
               if (exIndex > -1)
               {
                   result.eType = result.message.Substring(0, exIndex + 9);
                   result.eMessage = result.message.Substring(exIndex + 11);
                   result.message = "Exception";
               }

               return result;
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
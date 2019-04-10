/// <summary>
///     How to create a test for running in TestRunnerApp
/// 
///     Test something:
///         Optionally using a Webdriver
///         Optionally using string input parameter(s)
///         Optionally counting steps
/// 
///     Return a TestResult
/// </summary>

/// <include>
///     Get library from NuGet:
///         Install-Package NetOp.TestRunnerLib
/// </include>

/// <remarks>
///     Web tests, implement:
///         interface IWebTest
///         {
///             TestResult Test(WebDriverType webDriverType, string[] testdata);
///         }
///         
///     Other tests, implement:
///         interface ITest
///         {
///             TestKind Kind { get; }       // { Other, Web, AD, Exchange, ADO }
///             TestResult Test(string[] testdata;
///         }
/// </remarks>

/// <param type=WebDriver >
///     WebDriver provided by app
/// </param>
/// <param type=string >
///     Parameter string provided by app
/// </param>

/// <returns>
///     TestResult with:
///         (required) Outcome { Pass, Fail, Warning, NotRun }
///         (optional) int testStep
///         (optional) string someMessage
///         (optional) Exception exceptionWhileTesting
/// </returns>

/// <example>
///     return new TestResult(Outcome o);
///     return new TestResult(Outcome o, int step)
///     return new TestResult(Outcome o, string message)
///     return new TestResult(Outcome o, Exception ex)
///     return new TestResult(Outcome o, int step, string message)
///     return new TestResult(Outcome o, int step, Exception ex)
///     return new TestResult(Outcome o, string message, Exception ex)
///     return new TestResult(Outcome o, int step, string message, Exception ex)
/// </example>

using System;
using System.Linq;
using System.Text;

namespace MyTestSuite
{
    // TestApp
    using TestRunnerLib;
    // Selenium
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class MyWebTest : IWebTest
    {
        public TestResult Test(WebDriverType webDriverType, string[] testdata)
        {
            int testStep = 0;

            try
            {
                using (IWebDriver driver = WebDriver.Get(webDriverType))
                {
                    // Go somewhere
                    testStep++;
                    driver.Navigate().GoToUrl("http://www.sitetotest.com/");

                    // Test something & return result
                    testStep++;
                    if (driver.Title.Contains(testdata[1], StringComparison.OrdinalIgnoreCase))
                        return new TestResult(Outcome.Pass, $"Page title is: {driver.Title}");
                    else
                        return new TestResult(Outcome.Fail, $"Page title is: {driver.Title}");
                }
            }
            // Web driver error
            catch (OpenQA.Selenium.DriverServiceNotFoundException e)
            {
                return new TestResult(Outcome.Warning, testStep, e);
            }
            // Selenium exception during action
            catch (OpenQA.Selenium.WebDriverException e)
            {
                return new TestResult(Outcome.Fail, testStep, e);
            }
            // Other errors
            catch (Exception e)
            {
                return new TestResult(Outcome.Warning, testStep, e);
            }
        }
    } // class

    public class MyGeneralTest : ITest
    {
        public TestKind Kind { get; } = TestKind.Other;

        public TestResult Test(string[] testdata)
        {
            int testStep = 0;

            try
            {
                // Test something
                testStep++;
                string testReturn = "Something my test returns";
                bool MyParameterTest =
                    testReturn.Contains(testdata[1], StringComparison.OrdinalIgnoreCase) ? true : false;

                // Return result
                if (MyParameterTest)
                    return new TestResult(Outcome.Pass);
                else
                    return new TestResult(Outcome.Fail, testReturn);
            }
            // Exception while testing
            catch (Exception e)
            {
                return new TestResult(Outcome.Warning, testStep, e);
            }
        }
    }  // class

} 

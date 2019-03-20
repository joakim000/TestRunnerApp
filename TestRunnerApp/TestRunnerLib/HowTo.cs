/// <summary>
///     How to create a test for running in TestApp
/// 
///     Test something:
///         Optionally using a Webdriver
///         Optionally using a string input parameter
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
///             TestResult Test(WebDriverType webDriverType, string param1, string param2, string param3, string param4);
///         }
///         
///     Other tests, implement:
///         interface ITest
///         {
///             TestKind Kind { get; }       // { Other, AD, Exchange }
///             TestResult Test(string param1, string param2, string param3, string param4);
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
///     return new TestResult(Outcome o, string s)
///     return new TestResult(Outcome o, Exception e)
///     return new TestResult(Outcome o, int step, string s)
///     return new TestResult(Outcome o, int step, Exception e)
///     return new TestResult(Outcome o, string s, Exception e)
///     return new TestResult(Outcome o, int step, string s, Exception e)
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
        public TestResult Test(WebDriverType webDriverType, string param1, string param2, string param3, string param4)
        {
            int testStep = 0;

            try
            {
                using (IWebDriver driver = WebDriver.Get(w))
                {
                    // Go somewhere
                    testStep++;
                    driver.Navigate().GoToUrl("http://www.sitetotest.com/");

                    // Test something & return result
                    testStep++;
                    if (driver.Title.Contains(param1, StringComparison.OrdinalIgnoreCase))
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

        public TestResult Test(WebDriverType webDriverType, string param1, string param2, string param3, string param4)
        {
            int testStep = 0;

            try
            {
                // Test something
                testStep++;
                string testReturn = "Something my test returns";
                bool MyParameterTest =
                    testReturn.Contains(stringParam, StringComparison.OrdinalIgnoreCase) ? true : false;

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

} // namespace

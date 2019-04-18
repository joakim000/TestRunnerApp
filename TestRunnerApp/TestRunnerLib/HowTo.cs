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


    /* WebTest with Selenium */
    public class MyWebTest : IWebTest
    {
        public TestResult Test(WebDriverType webDriverType, string[] testdata)
        {
            // Init variables for use with TestResult
            int testStep = 0;
            string nonCriticalMessages = string.Empty;

            // Assign testdata, return warning if missing
            string expectedTitle;
            try
            {
                expectedTitle = testdata[1];
            }
            catch (IndexOutOfRangeException ex)
            {
                return new TestResult(Outcome.Warning, testStep, "Missing test data", ex);
            }

            try
            {
                using (IWebDriver driver = WebDriver.Get(webDriverType))
                {
                    // Increment when starting new step in test
                    testStep++;

                    // Go somewhere
                    driver.Navigate().GoToUrl("http://www.sitetotest.com/");

                    // Test something & return fail if it failed
                    testStep++;
                    if (!driver.Title.Contains(expectedTitle, StringComparison.OrdinalIgnoreCase))
                        return new TestResult(Outcome.Fail, testStep, $"Page title is: {driver.Title}");

                    // All tests passed
                    return new TestResult(Outcome.Pass, nonCriticalMessages);
                }
            }
            // Web driver error
            catch (OpenQA.Selenium.DriverServiceNotFoundException ex)
            {
                return new TestResult(Outcome.Warning, testStep, ex);
            }
            // Selenium exception during action
            catch (OpenQA.Selenium.WebDriverException ex)
            {
                return new TestResult(Outcome.Fail, testStep, ex);
            }
            // Other errors
            catch (Exception ex)
            {
                return new TestResult(Outcome.Warning, testStep, ex);
            }
        }
    } 



    /* General test */
    public class MyGeneralTest : ITest
    {
        public TestKind Kind { get; } = TestKind.Other;

        public TestResult Test(string[] testdata)
        {
            // Init variables for use with TestResult
            int testStep = 0;
            string nonCriticalMessages = string.Empty;

            // Assign testdata, return warning if missing
            string expected;
            try
            {
                expected = testdata[1];
            }
            catch (IndexOutOfRangeException ex)
            {
                return new TestResult(Outcome.Warning, testStep, "Missing test data", ex);
            }


            try
            {
                // Test something
                testStep++;
                string testReturn = "Something my test returns";
                bool MyParameterTest =
                    testReturn.Contains(expected, StringComparison.OrdinalIgnoreCase) ? true : false;

                // Return result
                if (!MyParameterTest)
                    return new TestResult(Outcome.Fail, testReturn);

                // All tests passed
                return new TestResult(Outcome.Pass, nonCriticalMessages);
            }

            // Unexpected errors while testing
            catch (Exception ex)
            {
                return new TestResult(Outcome.Warning, testStep, ex);
            }
        }
    }  



} 

using System;
using System.Linq;
using System.Text;

namespace DevTests
{
    using System.Diagnostics;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using TestRunnerLib;

    public class Runner9Test : IWebTest
    {
        public TestResult Test(WebDriverType w, string[] testData)
        {
            int step = 0;
            try
            {
                Debug.WriteLine($"I'm a test, this is my BaseDir: {AppDomain.CurrentDomain.BaseDirectory}");

                using (IWebDriver driver = WebDriver.Get(w))
                {
                    driver.Navigate().GoToUrl("http://www.google.com/");
                    IWebElement query = driver.FindElement(By.Name("q"));
                    query.SendKeys("Cheese");
                    query.Submit();

                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => d.Title.StartsWith("cheese", StringComparison.OrdinalIgnoreCase));

                    Debug.WriteLine("Page title is: " + driver.Title);

                    if (driver.Title.Contains(testData[1], StringComparison.OrdinalIgnoreCase))
                        return new TestResult(Outcome.Pass, $"{testData[1]}");
                    else
                        return new TestResult(Outcome.Fail, $"{testData[1]}");

                }
            }
            catch (OpenQA.Selenium.DriverServiceNotFoundException ex)
            {
                string error = $"Driver error: {ex.Message}";
                Debug.WriteLine(error);
                return new TestResult(Outcome.Warning, ex);
            }
            // Selenium exception during action
            catch (OpenQA.Selenium.WebDriverException ex)
            {
                return new TestResult(Outcome.Fail, step, ex);
            }
            catch (Exception ex)
            {
                string error = $"General error: {ex.Message}";
                Debug.WriteLine(error);
                return new TestResult(Outcome.Fail, ex);
            }
        }
                     


    }  // end: class Ex1
} // end: namespace

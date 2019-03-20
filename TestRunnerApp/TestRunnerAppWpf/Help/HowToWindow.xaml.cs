using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class HowToWindow : Window
    {
        string howTo = string.Empty;

        public HowToWindow()
        {
            InitializeComponent();
            DataContext = this;

            try
            {
                //Type t = typeof(TestModel);
                var ass = Assembly.GetAssembly(typeof(TestModel));
                string resourceName = ass.GetManifestResourceNames().Single(str =>
                    str.EndsWith("HowTo.cs"));
                using (Stream stream = ass.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        howTo = reader.ReadToEnd();
                    }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception reading HowTo.cs: {e}");
                howTo = fillString();
            }


            HowToTB.Text = howTo;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new SaveFileDialog
            {
                Filter = "Text files (*.cs)|*.cs|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = "TestAppHowTo.cs"
        };
            if (picker.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(picker.FileName, howTo);
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Error saving file:{Environment.NewLine}{exception.Message}");
                    Debug.WriteLine($"Error saving file:{Environment.NewLine}{exception.Message}");
                }
            }
            else
            {
                Debug.WriteLine("Save file cancelled");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string fillString()
        {
            return @"using System;
using System.Linq;
using System.Text;

namespace MyTestProject
{
    // TestApp
    using TestRunnerLib;
    // Selenium
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class MyTestSuite
    {
        /// <summary>
        ///     How to write a test for running in TestApp
        /// 
        ///     Test something:
        ///         Optionally counting steps
        ///         Optionally with a string input parameter
        ///         Optionally using a Webdriver
        /// 
        ///     Return a TestResult
        /// </summary>
        
        /// <remarks>
        ///     Nuget packages:
        ///         (required) Install-Package Selenium.WebDriver
        ///         (optional) Install-Package Selenium.Support
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


        /* Selenium test: 'Use web driver' checked */
        public static TestResult MySeleniumTest(WebDriverType w)
            {
                int testStep = 0;
                string message = 'A useful message, maybe something your test returned?';

                try
                {
                    using (IWebDriver driver = WebDriver.Get(w))
                    {
                        // Go somewhere
                        testStep++;
                        driver.Navigate().GoToUrl('http://www.sitetotest.com/');

                        // Test something
                        testStep++;
                        bool MyThingToTest = true;

                        // Return result
                        if (MyThingToTest)
                            return new TestResult(Outcome.Pass);
                        else
                            return new TestResult(Outcome.Fail, message);
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


        /* Selenium test with parameter: 'Use web driver' checked and parameter set on test */
        public static TestResult MySeleniumTest(WebDriverType w, string testParameter)
        {
            int testStep = 0;

            try
            {
                using (IWebDriver driver = WebDriver.Get(w))
                {
                    // Go somewhere
                    testStep++;
                    driver.Navigate().GoToUrl('http://www.sitetotest.com/');

                    // Test something & return result
                    testStep++;
                    if (driver.Title.Contains(testParameter, StringComparison.OrdinalIgnoreCase))
                        return new TestResult(Outcome.Pass, $'Page title is: {driver.Title}');
                    else
                        return new TestResult(Outcome.Fail, $'Page title is: {driver.Title}');
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


        /* General test: 'Use web driver' unchecked */
        public static TestResult MyGeneralTest()
        {
            int testStep = 0;
            string message = 'A useful message, maybe something your test returned?';

            try
            {
                // Test something
                testStep++;
                bool MyFirstThingToTest = true;

                // Test some more
                testStep++;
                bool MySecondThingToTest = true;

                // Return result
                if (MyFirstThingToTest && MySecondThingToTest)
                    return new TestResult(Outcome.Pass);
                else
                    return new TestResult(Outcome.Fail, message);
            }
            // Exception while testing
            catch (Exception e)
            {
                return new TestResult(Outcome.Warning, testStep, e);
            }
        }


        /* General test with parameter: 'Use web driver' unchecked and parameter set on test */
        public static TestResult MyGeneralTest(string testParameter)
        {
            int testStep = 0;

            try
            {
                // Test something
                testStep++;
                string testReturn = 'Something my test returns';
                bool MyParameterTest =
                    testReturn.Contains(testParameter, StringComparison.OrdinalIgnoreCase) ? true : false;

                // Return result
                if (MyParameterTest)
                    return new TestResult(Outcome.Pass, testReturn);
                else
                    return new TestResult(Outcome.Fail, testReturn);
            }
            // Exception while testing
            catch (Exception e)
            {
                return new TestResult(Outcome.Warning, testStep, e);
            }
        }

    } // class
} // namespace
";
        }

       
    } // class
} // namespace

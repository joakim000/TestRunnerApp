using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;

namespace TestRunnerLib
{
    

    public static class WebDriver
    {
        //public static bool chromeAvailable = false;
        public static bool chromeAvailable { get; set; } = false;
        public static bool firefoxAvailable = false;
        public static bool ieAvailable = false;

        public static IWebDriver Get(WebDriverType webDriver)
        {
            switch (webDriver)
            {
                case WebDriverType.Chrome:
                    return new ChromeDriver();
                case WebDriverType.Firefox:
                    return new FirefoxDriver();
                case WebDriverType.IE:
                    //return new PhantomJSDriver();
                    return new InternetExplorerDriver();
                //case WebDriverType.PhantomJS:
                //return new PhantomJSDriver();
                default:
                    return new ChromeDriver();
            }
        }

        public static void checkAvailibility()
        {
            IWebDriver w = null;

            // Check chrome availibility
            try
            {
                Debug.WriteLine("Checking availibility of Chrome");
                w = new ChromeDriver();
                chromeAvailable = true;
            }
            catch (OpenQA.Selenium.DriverServiceNotFoundException e)
            {
                string error = $"Driver error: {e.Message}";
                Debug.WriteLine(error);
            }
            catch (Exception e)
            {
                string error = $"General error: {e.Message}";
                Debug.WriteLine(error);
            }
            finally
            {
                w.Dispose();
            }

            // Check firefox availibility
            try
            {
                Debug.WriteLine("Checking availibility of Firefox");
                w = new FirefoxDriver();
                firefoxAvailable = true;
            }
            catch (OpenQA.Selenium.DriverServiceNotFoundException e)
            {
                string error = $"Driver error: {e.Message}";
                Debug.WriteLine(error);
            }
            catch (Exception e)
            {
                string error = $"General error: {e.Message}";
                Debug.WriteLine(error);
            }
            finally
            {
                w.Dispose();
            }

            // Check firefox availibility
            try
            {
                Debug.WriteLine("Checking availibility of IE");
                w = new FirefoxDriver();
                firefoxAvailable = true;
            }
            catch (OpenQA.Selenium.DriverServiceNotFoundException e)
            {
                string error = $"Driver error: {e.Message}";
                Debug.WriteLine(error);
            }
            catch (Exception e)
            {
                string error = $"General error: {e.Message}";
                Debug.WriteLine(error);
            }
            finally
            {
                w.Dispose();
            }

            // Check ie availibility
            try
            {
                w = new InternetExplorerDriver();
                ieAvailable = true;
            }
            catch (OpenQA.Selenium.DriverServiceNotFoundException e)
            {
                string error = $"Driver error: {e.Message}";
                Debug.WriteLine(error);
            }
            catch (Exception e)
            {
                string error = $"General error: {e.Message}";
                Debug.WriteLine(error);
            }
            finally
            {
                w.Dispose();
            }


        }

    }
}

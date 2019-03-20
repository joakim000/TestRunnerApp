using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerLib
{
    public static class ImageTools
    {
        public static void TakeFullScreenshot(IWebDriver driver, String filename)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            //screenshot.SaveAsFile(filename, ScreenshotImageFormat.Png);
        }

        public static void TakeScreenshotOfElement(IWebDriver driver, By by, string fileName)
        {
            // 1. Make screenshot of all screen
            var screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            var bmpScreen = new Bitmap(new MemoryStream(screenshot.AsByteArray));
            // 2. Get screenshot of specific element
            IWebElement element = driver.FindElement(by);
            var cropArea = new Rectangle(element.Location, element.Size);
            cropArea.X = cropArea.X + cropArea.Width;
            cropArea.Y = cropArea.Y - cropArea.Height;

            var bitmap = bmpScreen.Clone(cropArea, bmpScreen.PixelFormat);
            bitmap.Save(fileName);
        }

        /*
        [TestMethod]
        public void WebDriverAdvancedUsage_TakingFullScrenenScreenshot()
        {
            this.driver.Navigate().GoToUrl(@"http://automatetheplanet.com");
            this.WaitUntilLoaded();
            string tempFilePath = Path.GetTempFileName().Replace(".tmp", ".png");
            this.TakeFullScreenshot(this.driver, tempFilePath);
        }
        [TestMethod]
        public void WebDriverAdvancedUsage_TakingElementScreenshot()
        {
            this.driver.Navigate().GoToUrl(@"http://automatetheplanet.com");
            this.WaitUntilLoaded();
            string tempFilePath = Path.GetTempFileName().Replace(".tmp", ".png");
            this.TakeScreenshotOfElement(this.driver,
            By.XPath("//*[@id='tve_editor']/div[2]/div[2]/div/div"), tempFilePath);
        }
        */

    }
}

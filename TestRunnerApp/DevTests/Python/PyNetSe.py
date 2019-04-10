import clr
import time
import sys
import array

from TestAppLib import TestResult, Outcome
from OpenQA.Selenium import *
from OpenQA.Selenium.Support.UI import *


# Test runner
def Run(driver, testdata):
    server = testdata[1]; 
    input = testdata[2];

    step = 1;
    try:
       driver.Navigate().GoToUrl(server)
       time.sleep(1)
       el = driver.FindElement(By.Name("q"))
       el.SendKeys(input)
       el.Submit()

    except WebDriverException as ex:
       return TestResult(Outcome.Fail, step, "My failure message", ex)

    else:
        return TestResult(Outcome.Pass, step, "Page title is: {title}".format(title=webdriver.Title))

# Execution
result = Run(webdriver, testdata)




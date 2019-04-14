import clr
import time
import sys
import array

from TestAppLib import TestResult, Outcome
from OpenQA.Selenium import *
from OpenQA.Selenium.Support.UI import *


# Test runner
def Run(driver, testData):
    server = testData[1]; 
    input = testData[2];

    step = 1;
    try:
       driver.Navigate().GoToUrl(server)
       time.sleep(1)
       el = driver.FindElement(By.Name("q"))
       step = 2;
       el.SendKeys(input)
       step = 3;
       el.Submit()

    except WebDriverException as ex:
       return TestResult(Outcome.Fail, step, "My failure message", ex)

    else:
        return TestResult(Outcome.Pass, step, "Page title is: {title}".format(title=webdriver.Title))

# Execution
result = Run(webdriver, testData)




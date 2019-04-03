import clr
import time
import sys

from TestAppLib import TestResult, Outcome
from OpenQA.Selenium import *
from OpenQA.Selenium.Support.UI import *


# Test runner
def Run(driver, param1, param2, param3, param4):
    server = param1; 
    input = param2;

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
result = Run(webdriver, param1, param2, param3, param4 )




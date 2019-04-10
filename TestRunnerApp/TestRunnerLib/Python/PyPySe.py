import time
import sys
import clr

from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import *
from selenium.webdriver.support import *
from selenium.webdriver.support import expected_conditions

from TestAppLib import TestResult, Outcome, WebDriverType

# Format exception string
def ExceptionFormat(ex):
    try:
        name = repr(ex).replace("()", "")
        msg = str(ex).replace("Message", "")    
        return name + msg
    except Exception:
        return "Error formatting exception"

# Get webdriver from UI selection
def GetDriver(webdrivertype):
    try:
      if webdrivertype == WebDriverType.Chrome:
          return webdriver.Chrome()
      elif webdrivertype == WebDriverType.Firefox:
          return webdriver.Firefox()
      elif webdrivertype == WebDriverType.IE:
          return webdriver.Ie()
      # Not found
      raise RemoteDriverServerException

    except Exception:
        result = TestResult(Outcome.Warning, 0, "WebDriver error")

# Test runner
def Run(driver, testdata):
      server = testdata[1]; 
      input = testdata[2];

      step = 1;
      try:
          driver.get(server)
          el = driver.find_element_by_name("q");    
          el.send_keys(input)
          el.submit()
          WebDriverWait(driver, 10).until(expected_conditions.title_contains(input))
          
          return TestResult(Outcome.Pass, "Page title is: {title}".format(title=driver.title))

       # Selenium exception during action
      except WebDriverException as ex:
           #s = repr(ex) + str(ex);
           return TestResult(Outcome.Fail, step, ExceptionFormat(ex))
       
       # Other errors
      except Exception as ex:
           return TestResult(Outcome.Fail, step, ExceptionFormat(ex))

      finally:
             driver.quit()

# Execution
result = Run(GetDriver(webdrivertype), param1, param2, param3, param4)

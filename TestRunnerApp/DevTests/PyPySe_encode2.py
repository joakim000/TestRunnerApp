# -- coding: utf-8 --
import time
import sys
import clr

from TestAppLib import TestResult, Outcome, WebDriverType

# Format exception string
def ExceptionFormat(ex):
    try:
        name = repr(ex).replace("()", "")
        msg = str(ex).replace("Message", "")    
        return name + msg
    except Exception:
        return "Error formatting exception"



def Run(testData):
      server = testData[1]
      #input = testData[2].decode("utf-8");
      input = testData[2]

      step = 1
      try:
          return TestResult(Outcome.Pass, "data1: " + server + "   data2: " + input)

       # Other errors
      except Exception as ex:
           return TestResult(Outcome.Fail, step, ExceptionFormat(ex))

     
# Execution
#reload(sys)  
#sys.setdefaultencoding('utf8')
result = Run(testData)

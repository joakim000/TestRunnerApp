using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ViewModelSupport;
using System.IO;
using System.Diagnostics;

namespace TestRunnerAppWpf
{
    public class LogViewModel : ViewModelBase
    {
        public string logText
        {
            get => Get(() => logText);
            set => Set(() => logText, value);
        }

        DispatcherTimer timer;

        string logsDir = AppDomain.CurrentDomain.BaseDirectory + @"Logs\";
        string logFile;

       public  LogViewModel()
        {
            logFile = logsDir + DateTime.Now.ToString("yyyyMMdd") + ".log";

            TestRunnerLib.Log.Line();
            TestRunnerLib.Log.AddNoWrite($"Session start {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}");

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            //timer.Start();

            Debug.Listeners.Add(new TextWriterTraceListener(logsDir + "TraceOutput.log", "myListener"));


       }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            //string s = TestRunnerLib.Log.WriteScratch();
            //logText += s;


            //logText = TestRunnerLib.Log.Get();
            try
            {
                logText = File.ReadAllText(logsDir + "RunnerTraceOutput.log");
            }
            catch (IOException)
            {
                timer.Stop();
                await Task.Delay(5000);
            }

            Debug.Flush();
        }

    }
}

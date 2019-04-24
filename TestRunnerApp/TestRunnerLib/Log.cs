using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelSupport;

namespace TestRunnerLib
{
    public class Log 
    {
        private static string full = string.Empty;
        private static string scratch = string.Empty;

        private static string logsDir = AppDomain.CurrentDomain.BaseDirectory + @"Logs\";
        private static string logFile = logsDir + DateTime.Now.ToString("yyyyMMdd") + ".log";
        private static string logFileSession = logsDir + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";

        public static void AddNoWrite(string logEntry)
        {
            full += logEntry + Environment.NewLine;
            scratch += logEntry + Environment.NewLine;
        }

        // Add continue line
        public static void AddContinue(string logEntry)
        {
            full += logEntry;
            scratch += logEntry;
        }


        public static void Line()
        {
            AddNoWrite("==============================");
        }

        public static void Stars()
        {
            AddContinue("*** ");
        }

        public static string Get()
        {
            return full;
        }

        public static string Scratch()
        {
            string s = scratch;
            scratch = string.Empty;
            return s;
        }

        public static string WriteScratch()
        {
            string s = scratch;
            scratch = string.Empty;
            File.AppendAllText(logFile, s);
            return s;
        }

        public static void Write()
        {
            string file = logsDir + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "full.log";
            Debug.WriteLine("Writing log to file " + file + "Text: " + file);
            File.AppendAllText(logFile, full);
        }

        public static void Add(string logEntry)
        {
            AddNoWrite(logEntry);
            File.AppendAllText(logFileSession, logEntry + Environment.NewLine);
        }
    }
}

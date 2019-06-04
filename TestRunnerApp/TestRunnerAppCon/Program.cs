using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestRunnerLib;

namespace TestRunnerAppCon
{
    class Program
    {
        public static string dataExt = "testapp";
        //SynchronizationContext syncContext = SynchronizationContext.Current;
        public static BackgroundWorker runTestWorker = new BackgroundWorker();
        private static EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.AutoReset);

        static void Main(string[] args)
        {
            Console.WriteLine("TestRunnerApp v2.1rc7" + Environment.NewLine);

            // Prepare for threaded processing
            var context = new CustomSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(context);
            

            // Check argument list
            var argsList = new List<string>(args);
            int argCount = args.Length;
            if (argCount < 1)
            {
                Console.WriteLine("No arguments. Try 'help'.");
                Environment.Exit(-1);
            }

            // First argument should be either "help" or a file with supported extension
            string file = args[0];
            if (string.Equals("help", file))
            {
                ShowHelp();
                Environment.Exit(0);
                
            }
            if (!string.Equals(dataExt, FileMgmt.FileExt(file)))
            {
                Console.WriteLine("Unsupported file type. Try 'help'.");
                Environment.Exit(-1);
            }

            // Check for existence of needed dirs (try to create if not existant)
            CheckDirs();

            // Get settings from file
            SettingsManager settings = new SettingsManager();
            SettingsGetter getter = new SettingsGetter();
            getter.Restore(settings);
            
            // Read data from file in first argument
            Console.WriteLine("Reading data from: " + file);
            Model model = new Model();
            LoadData.OpenFileSetup(file, model);

            // Check for /id argument
            string idPattern = null;
            int idIndex = argsList.IndexOf("/id");
            if (idIndex >= 0)
            {
                if (argCount > idIndex + 1) // Avoid case of /id arg with nothing after
                    idPattern = argsList[idIndex + 1];
            }

            // Check for /o argument
            string[] outcomes = { };
            int oIndex = argsList.IndexOf("/o");
            if (oIndex >= 0)
            {
                if (argCount > oIndex + 1) // Avoid case of /o arg with nothing after
                {
                    outcomes = args.Skip(oIndex + 1).ToArray();
                    int slashIndex = Array.FindIndex(outcomes, a => a.Contains("/"));
                    if (slashIndex >= 0)
                    {
                        outcomes = outcomes.Take(slashIndex).ToArray();
                    }
                }
            }

            // Check for /d argument
            var testData = new Dictionary<string, Tuple<int, string>>();
            string[] dataArguments = { };
            int dIndex = argsList.IndexOf("/d");
            if (dIndex >= 0)
            {
                if (argCount > dIndex + 1) // Avoid case of /d arg with nothing after
                {
                    dataArguments = args.Skip(dIndex + 1).ToArray();
                    int slashIndex = Array.FindIndex(dataArguments, a => a.Contains("/"));
                    if (slashIndex >= 0)
                    {
                        dataArguments = dataArguments.Take(slashIndex).ToArray();
                    }
                    foreach (string arg in dataArguments)
                    {
                        int hash = arg.IndexOf("#");
                        string id = arg.Substring(0, hash);
                        int colon = arg.IndexOf(":");
                        bool couldParseIndex = int.TryParse(arg.Substring(hash + 1, colon -2 ), out int index);

                        string data = null;
                        if (arg.Length > colon)
                        {
                            data = arg.Substring(colon + 1);
                        }
                        if ((!string.IsNullOrEmpty(id)) && couldParseIndex && (!string.IsNullOrEmpty(data)))
                        {
                            testData.Add(id, new Tuple<int, string>(index, data));
                        }
                    }
                    Console.WriteLine($"Testdata, count: {testData.Count()}");
                    foreach (var td in testData)
                    {
                        Console.WriteLine($"Test-id:{td.Key} Index:{td.Value.Item1} Data:{td.Value.Item2} ");
                    }
                }
            }


            // Find command argument
            if (argCount < 2)
            {
                Console.WriteLine("No command argument. Try 'help'.");
                Environment.Exit(-1);
            }
            string cmd = args[1];

            // Find filters and execute command
            switch (cmd)
            {
                case "list":
                    Report.ListTests(model.suite, outcomes, idPattern);
                    break;

                case "run":
                    // Temporary
                    WebDriverType webDriverType = Settings.webDriverType;


                    Report.ListTests(model.suite, outcomes, idPattern);

                    context = (CustomSynchronizationContext)SynchronizationContext.Current;
                    runTestWorker = new BackgroundWorker();
                    runTestWorker.RunWorkerCompleted += RunTestWorker_RunWorkerCompleted;

                    RunTests r = new RunTests();
                    List<TestModel> testsRun = r.Run(model, outcomes, idPattern, testData, webDriverType, context, runTestWorker);
                    ewh.WaitOne();
                    
                    Console.WriteLine("Saving suite to file.");
                    FileMgmt.SaveSuite(model.suite);
                    Report.ListTests(testsRun);
                    break;

                case "mail":
                    // Check for /to argument
                    string sendTo = null;
                    int toIndex = Array.FindIndex(args, a => a.Equals("/to", StringComparison.OrdinalIgnoreCase));
                    if (toIndex >= 0)
                    {
                        if (argCount > toIndex + 1) // Avoid case of /to arg with nothing after
                            sendTo = argsList[toIndex + 1];
                    }

                    if (sendTo == null)
                    {
                        Console.WriteLine("No adress specified. Try 'help'.");
                        Environment.Exit(-1);
                    }
                    else
                    {
                        try
                        {
                            var toAddr = new System.Net.Mail.MailAddress(sendTo);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Environment.Exit(-1);
                        }
                    }

                    Report.ListTests(model.suite, outcomes, idPattern);
                    Mail.SuiteStatus(model.suite, sendTo, outcomes, idPattern);
                    break;

                case "save":
                    FileMgmt.SaveSuite(model.suite);
                    break;

                case "help":
                    ShowHelp();
                    break;

                default:
                    Console.WriteLine($"Unrecognized command '{cmd}'. Try 'help'.");
                    Environment.Exit(-1);
                    break;

            }

            //Console.ReadKey();

        }

        private static void RunTestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ewh.Set();
        }

        static void CheckDirs()
        {
            // Always want to have these dirs available
            string testsDir = AppDomain.CurrentDomain.BaseDirectory + "Tests";
            string cacheDir = AppDomain.CurrentDomain.BaseDirectory + "Cache";
            string logsDir = AppDomain.CurrentDomain.BaseDirectory + "Logs";
            try
            {
                if (!Directory.Exists(testsDir))
                    Directory.CreateDirectory(testsDir);
                if (!Directory.Exists(cacheDir))
                    Directory.CreateDirectory(cacheDir);
                if (!Directory.Exists(logsDir))
                    Directory.CreateDirectory(logsDir);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating dir: {ex}");
                Console.WriteLine($"Error creating dir: {ex.Message}");
                Environment.Exit(-1);
            }
        }


        static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Help for TestRunnerApp console");
            Console.WriteLine("==============================");
            Console.WriteLine("Usage: TestRunnerApp Example.testapp command params");
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine("list [filter options]");
            Console.WriteLine("run [filter options]");
            Console.WriteLine("mail /to 'email' [filter options]");
            //Console.WriteLine("mail 'address' [additional addresses] [filter options]");
            Console.WriteLine();
            Console.WriteLine("Filter:");
            Console.WriteLine("  by last outcome: /o [Pass] [Fail] [Warning] [NotRun]");
            Console.WriteLine("  by test id: /id 'regEx'");
            Console.WriteLine();
            Console.WriteLine("Change testdata before run:");
            Console.WriteLine("  /d TestId#TestDataIndex:NewTestData");
            Console.WriteLine("  Note: New testdata cannot contain forward slash.");
            Console.WriteLine("  Ex: /d T1#1:foo T1#2:bar T2#1:far T2#2:'b o o'");
            Console.WriteLine("==============================");
        }
       

    }
}




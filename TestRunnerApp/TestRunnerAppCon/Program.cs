using System;
using System.Collections.Generic;
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

        
             


        static void Main(string[] args)
        {
            Console.WriteLine("TestRunnerApp v2.1rc7" + Environment.NewLine);
            var context = new CustomSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(context);
            int argCount = args.Length;

            SettingsManager settings = new SettingsManager();
            SettingsGetter getter = new SettingsGetter();
            getter.Restore(settings);


            if (argCount < 1)
            {
                Console.WriteLine("No arguments. Try 'help'.");
                Environment.Exit(-1);
            }

            string file = args[0];

            if (string.Equals("help", file))
            {

                Console.WriteLine("Usage: TestRunnerApp MySuite.testapp command [parameter]");
                Console.WriteLine("Available commands: list, save, runall, mail");
                Console.WriteLine("list [Pass] [Fail] [Warning] [NotRun]");
                Console.WriteLine("mail to-adress [Pass] [Fail] [Warning] [NotRun]");
                Environment.Exit(0);
            }

            if (!string.Equals(dataExt, FileMgmt.FileExt(file)))
            {
                Console.WriteLine("Unsupported file type. Try 'help'.");
                Environment.Exit(-1);
            }

            CheckDirs();

            Console.WriteLine("Reading data from: " + file);
            Model model = new Model();
            LoadData.OpenFileSetup(file, model);

            if (argCount < 2)
            {
                Console.WriteLine("No command argument. Try 'help'.");
                Environment.Exit(-1);
            }

            string cmd = args[1];

            switch (cmd)
            {
                case "list":
                    var listFilters = args.Skip(2).ToArray();
                    ListTests(model.suite, listFilters);
                    break;

                case "save":
                    FileMgmt.SaveSuite(model.suite);
                    break;

                case "runall":
                    //ListTests(model.suite);
                    RunTests r = new RunTests();
                    context = (CustomSynchronizationContext)SynchronizationContext.Current;
                    r.Run(model, WebDriverType.Chrome, context);
                    break;

                case "mail":
                    if (argCount < 3)
                    {
                        Console.WriteLine("No adress specified. Try 'help'.");
                        Environment.Exit(-1);
                    }

                    string sendTo = args[2];
                    var mailFilters = args.Skip(3).ToArray();
                    ListTests(model.suite, mailFilters);
                    Mail.SuiteStatus(model.suite, sendTo, mailFilters);
                    break;

                default:
                    Console.WriteLine("Unrecognized command. Try 'help'.");
                    Environment.Exit(-1);
                    break;

            }

            //Console.ReadKey();

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


        static void ListTests(SuiteModel suite, string[] filters)
        {

            //Outcome[] filter = { Outcome.Fail, Outcome.Warning };
            Col[] selection = { Col.id, Col.name, Col.previousDateTime, Col.webDriverType, Col.previousOutCome,
                                Col.failStep, Col.message, Col.eType};

            Console.WriteLine(Report.SuiteToTable(suite, false, Report.readFilters(filters), selection));

        }


    }
}




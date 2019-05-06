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

#if DEBUG
            //for (int i = 0; i < argCount; i++)
            //{
            //    Console.WriteLine($"arg {i}: [{args[i]}]");
            //}
#endif

            if (argCount < 1)
            {
                Console.WriteLine("No arguments. Try 'help'.");
                Environment.Exit(-1);
            }

            string file = args[0];

            if (string.Equals("help", file))
            {

                Console.WriteLine("Usage: TestRunnerApp MySuite.testapp command [parameter]");
                Console.WriteLine("Available commands: list, save, runall, mailstatus");
                Console.WriteLine("list (all) | list fail (failed tests) | list warning (warned tests) | list error (failed + warned)");
                Console.WriteLine("mailstatus to-adress [optional filter, cf. list command]");
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
                    ListTests(model.suite);
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

                case "mailstatus":
                    Mail.SuiteStatus(model.suite);
                    break;

                default:
                    Console.WriteLine("Unrecognized command. Try 'help'.");
                    Environment.Exit(-1);
                    break;

            }

            //Console.ReadKey();
            //ListTests(model.suite);
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


        static void ListTests(SuiteModel suite)
        {
            if (suite.tests.Count() < 1)
            {
                Console.WriteLine("No tests found.");
                return;
            }

            string[] columns = { "ID", "Name", "Last run", "Last outcome", "Ex message" };

            var table = suite.tests.ToStringTable(columns,
                     t => t.id,
                     t => t.name,
                     t => t.previousDateTime == null ? string.Empty : t.previousDateTime.ToString(),
                     t => t.previousOutcome,
                     t => t.runs.Count() > 0 ? t.runs.Last()?.resultObj?.eMessage : string.Empty

                     
                 );


            Console.WriteLine(table);

        }



        //static void ListTests_old(SuiteModel suite)
        //{
        //    string table = String.Empty;

        //    string header = string.Format("|{0,10}|{1,10}|{2,10}|{3,10}|",
        //            "ID",
        //            "Name",
        //            "Last run",
        //            "Last outcome"
        //            );
        //    header += Environment.NewLine;

        //    foreach (TestModel t in suite.tests)
        //    {
        //        table += string.Format("|{0,10}|{1,10}|{2,10}|{3,10}|",
        //            t.id,
        //            t.name,
        //            t.previousDateTime.ToString(),
        //            t.previousOutcome.ToString()
        //            );
        //        table += Environment.NewLine;
        //    }



        //    if (string.IsNullOrEmpty(table))
        //        Console.WriteLine("No tests found.");
        //    else
        //        Console.WriteLine(header + table);

        //}

    }
}




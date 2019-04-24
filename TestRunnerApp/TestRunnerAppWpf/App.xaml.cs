using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow w = new MainWindow();

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
                MessageBox.Show($"Error creating dir: {ex.Message}");
            }

            w.Show();

            // Open file by association or continue session
            if (e.Args.Length == 1)
            {
                System.Diagnostics.Debug.WriteLine("Startup opening file: \n\n" + e.Args[0]);

                string fileToOpen = e.Args[0];
                FileMgmt.OpenFileSetup(fileToOpen, w.model);

            }
            else
                w.model.ContinueSession();
        }

    }
        
}

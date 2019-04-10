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

            // Always want to have this dir available
            try
            {
                string testsDir = AppDomain.CurrentDomain.BaseDirectory + "Tests";
                if (!Directory.Exists(testsDir))
                    Directory.CreateDirectory(testsDir);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating dir: {ex}");
                MessageBox.Show($"Error creating dir: {ex.Message}");
            }


            // Open file by association
            if (e.Args.Length == 1)
            {
                System.Diagnostics.Debug.WriteLine("Startup opening file: \n\n" + e.Args[0]);

                string fileToOpen = e.Args[0];
                Tuple<string, SuiteModel> fileopen = FileMgmt.OpenSuite(fileToOpen);
                if (fileopen.Item2 != null)
                {
                    w.model.gridViewModel.suite = fileopen.Item2;
                    w.model.SelectedItems_PropertyChanged(null, null);
                    FileMgmt.unsavedChanges = false;
                    w.model.unsavedChanges = false;

                    if (w.model.gridViewModel.suite.currentCycle != null)
                        w.model.gridViewModel.suite.currentCycle = w.model.gridViewModel.suite.currentCycle;

                    string file = FileMgmt.ShortFilename(fileToOpen);
                    string libFile = FileMgmt.PreviousDir(fileToOpen) + @"\" + file.Substring(0, file.LastIndexOf(".")) + ".dll";
                    if (File.Exists(libFile))
                    {
                        System.Diagnostics.Debug.WriteLine("Found matching libfile opening associated file: " + libFile);
                        try
                        {
                            string testsDir = AppDomain.CurrentDomain.BaseDirectory + "Tests";

                            if (!Directory.Exists(testsDir))
                                Directory.CreateDirectory(testsDir);

                            File.Copy(libFile,
                                testsDir + @"\" + FileMgmt.ShortFilename(libFile),
                                true);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error copying file: {ex}");
                            MessageBox.Show($"Error copying file: {ex.Message}");
                        }
                    }

                }
                if (fileopen.Item1 != null)
                    w.model.gridViewModel.suite.filename = fileopen.Item1;
            }

            w.Show();
        }

    }
        
}

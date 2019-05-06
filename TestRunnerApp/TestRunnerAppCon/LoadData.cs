using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestRunnerLib;
using ViewModelSupport;

namespace TestRunnerAppCon
{
    class LoadData
    {

        //public static void OpenFileSetup(string fileToOpen,  MainViewModel mvm)
        public static void OpenFileSetup(string fileToOpen, Model model)
        {
            //DetailsViewModel dvm = mvm.detailsViewModel;
            //GridViewModel gvm = mvm.gridViewModel;



            Tuple<string, SuiteModel> openResult = FileMgmt.OpenSuite(fileToOpen);

            if (openResult.Item2 != null)
            {
                // Avoid jira updates while loading
                //mvm.ToggleJiraCaseUpdates(false);

                // Load the suite
                model.suite = openResult.Item2;

                if (openResult.Item1 != null)
                    model.suite.filename = openResult.Item1;

                // Reset views
                //dvm.test = new TestModel();
                //dvm.cycle = new CycleModel();

                //dvm.PropertyChanged -= dvm.DetailsViewModel_PropertyChanged;
                //dvm.jiraSelectedProject = null;
                //dvm.PropertyChanged += dvm.DetailsViewModel_PropertyChanged;
                // More?

                // Set selected cycle
                if (model.suite.currentCycle != null)
                {
                    Guid cc = model.suite.currentCycle.uid;
                    model.suite.currentCycle = model.suite.cycles.Where(x => x.uid == cc).First();
                }

                if (model.suite.mgmt == null)
                    model.suite.mgmt = Enums.Mgmt.Find(x => x.key == "None");
                else
                    model.suite.mgmt = Enums.Mgmt.Find(x => x.key == model.suite.mgmt.key);


                // Test managment handling (depending on usersetting) - Jira Cloud with TM4J
                if (model.suite.mgmt == Enums.Mgmt.Find(x => x.key == "JiraCloudTmj"))
                {

                    Console.WriteLine("Console version doesn't support test management yet.");

                    //mvm.JiraSetup();

                    // Select matching mgmt option in detailsview
                    //model.suite.mgmt = dvm.mgmtOptions.Find(x => x.key == model.suite.mgmt.key);

                    //if (model.suite.jiraProject != null && model.suite.jiraProject.key != null)
                    //{
                    //    string key = model.suite.jiraProject.key;
                    //    if (dvm.jiraAvailableProjects.Where(x => x.key == key).Count() > 0)
                    //    {
                    //        dvm.jiraSelectedProject = dvm.jiraAvailableProjects.Where(x => x.key == key).First();
                    //    }
                    //    else
                    //    {
                    //        if (dvm.CanExecute_JiraGetAvailableProjectsCmd())
                    //        {
                    //            dvm.Execute_JiraGetAvailableProjectsCmd();
                    //        }
                    //    }
                    //    if (dvm.jiraSelectedProject == null)
                    //        //Console.WriteLine("Project selected in Suite not available from Jira.", "TestRunnerApp with Jira",
                    //        //MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    //        // Set SelectedItems on tests
                    //        foreach (TestModel t in model.suite.tests)
                    //        {
                    //            t.SetSelectedItems(model.suite.jiraProject);
                    //        }

                    //}

                }



                // Test managment handling (depending on usersetting) - ReqTest
                if (model.suite.mgmt == Enums.Mgmt.Find(x => x.key == "ReqTest"))
                {
                    // TODO: Reqtest-support
                    Console.WriteLine("Console version doesn't support test management yet.");
                }


                // Reset unsaved changes
                model.unsavedChanges = false;

                // Copy matching libfile in same dir as opened suitefile
                if (fileToOpen != null)
                {
                    string file = FileMgmt.ShortFilename(fileToOpen);
                    string libFile = FileMgmt.PreviousDir(fileToOpen) + @"\" + file.Substring(0, file.LastIndexOf(".")) + ".dll";
                    if (File.Exists(libFile))
                    {
                        Console.WriteLine("Found matching test library file: " + libFile);
                        Debug.WriteLine("Found matching libfile opening file: " + libFile);
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
                            Debug.WriteLine($"Error copying file: {ex}");
                            Console.WriteLine($"Error copying file: {ex.Message}");
                        }
                    }
                }

                // Enable jira updates after loading
                //mvm.ToggleJiraCaseUpdates(true);

                // Propagate LabelToIdSettings
                foreach (TestModel t in model.suite.tests)
                    t.jiraLabelToId = model.suite.jiraLabelToId;
                foreach (TestModel t in model.suite.tests)
                    t.jiraLabelToIdToken = model.suite.jiraLabelToIdToken;

            }
            else // returned suite was null
            {
                if (openResult.Item1 != null)
                {
                    Console.WriteLine("Failure opening suite from: unknown file");
                    Debug.WriteLine("Failure opening suite from: unknown file");
                    Environment.Exit(-1);
                }
                else
                {
                    Console.WriteLine($"Failure opening suite from: {openResult.Item1}");
                    Debug.WriteLine($"Failure opening suite from: {openResult.Item1}");
                    Environment.Exit(-1);
                }
            }

        }
    }

}

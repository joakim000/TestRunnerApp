using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    class LoadData
    {



        public static void OpenFileSetup(string fileToOpen,
                                             MainViewModel mvm)
        {
            DetailsViewModel dvm = mvm.detailsViewModel;
            GridViewModel gvm = mvm.gridViewModel;

            Tuple<string, SuiteModel> openResult = FileMgmt.OpenSuite(fileToOpen);

            if (openResult.Item2 != null)
            {
                // Avoid jira updates while loading
                mvm.ToggleJiraCaseUpdates(false);

                // Load the suite
                gvm.suite = openResult.Item2;

                if (openResult.Item1 != null)
                {
                    //gvm.suite.filename = openResult.Item1;
                    mvm.currentFilename = openResult.Item1;
                }
                else
                {
                    mvm.currentFilename = null;
                }


                // Reset views
                dvm.test = new TestModel();
                dvm.cycle = new CycleModel();

                dvm.PropertyChanged -= dvm.DetailsViewModel_PropertyChanged;
                dvm.jiraSelectedProject = null;
                dvm.PropertyChanged += dvm.DetailsViewModel_PropertyChanged;
                // More?

                // Set selected cycle
                if (gvm.suite.currentCycle != null)
                {
                    Guid cc = gvm.suite.currentCycle.uid;
                    gvm.suite.currentCycle = gvm.suite.cycles.Where(x => x.uid == cc).First();
                }

                if (gvm.suite.mgmt == null)
                    gvm.suite.mgmt = Enums.Mgmt.Find(x => x.key == "None");
                else
                    gvm.suite.mgmt = Enums.Mgmt.Find(x => x.key == gvm.suite.mgmt.key);


                // Test managment handling (depending on usersetting) - Jira Cloud with TM4J
                if (gvm.suite.mgmt == Enums.Mgmt.Find(x => x.key == "JiraCloudTmj"))
                {
                    mvm.JiraSetup();

                    // Select matching mgmt option in detailsview
                    gvm.suite.mgmt = dvm.mgmtOptions.Find(x => x.key == gvm.suite.mgmt.key);

                    if (gvm.suite.jiraProject != null && gvm.suite.jiraProject.key != null)
                    {
                        string key = gvm.suite.jiraProject.key;
                        if (dvm.jiraAvailableProjects.Where(x => x.key == key).Count() > 0)
                        {
                            dvm.jiraSelectedProject = dvm.jiraAvailableProjects.Where(x => x.key == key).First();
                        }
                        else
                        {
                            if (dvm.CanExecute_JiraGetAvailableProjectsCmd())
                            {
                                dvm.Execute_JiraGetAvailableProjectsCmd();
                            }
                        }
                        if (dvm.jiraSelectedProject == null)
                            //MessageBox.Show("Project selected in Suite not available from Jira.", "TestRunnerApp with Jira",
                            //MessageBoxButton.OK, MessageBoxImage.Exclamation);

                            // Set SelectedItems on tests
                            foreach (TestModel t in gvm.suite.tests)
                            {
                                t.SetSelectedItems(gvm.suite.jiraProject);
                            }

                    }

                }



                // Test managment handling (depending on usersetting) - ReqTest
                if (gvm.suite.mgmt == Enums.Mgmt.Find(x => x.key == "ReqTest"))
                {
                    // TODO: Reqtest-support
                }


                // Reset unsaved changes
                mvm.unsavedChanges = false;

                // Copy matching libfile in same dir as opened suitefile
                if (fileToOpen != null)
                {
                    string file = FileMgmt.ShortFilename(fileToOpen);
                    string libFile = FileMgmt.PreviousDir(fileToOpen) + @"\" + file.Substring(0, file.LastIndexOf(".")) + ".dll";
                    if (File.Exists(libFile))
                    {
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
                            MessageBox.Show($"Error copying file: {ex.Message}");
                        }
                    }
                }

                // Enable jira updates after loading
                mvm.ToggleJiraCaseUpdates(true);

                // Propagate LabelToIdSettings
                foreach (TestModel t in gvm.suite.tests)
                    t.jiraLabelToId = gvm.suite.jiraLabelToId;
                foreach (TestModel t in gvm.suite.tests)
                    t.jiraLabelToIdToken = gvm.suite.jiraLabelToIdToken;

            }
            else // returned suite was null
            {
                if (openResult.Item1 != null)
                    Debug.WriteLine("Failure opening suite from: unknown file");
                else
                    Debug.WriteLine($"Failure opening suite from: {openResult.Item1}");
            }

        }
    }

}

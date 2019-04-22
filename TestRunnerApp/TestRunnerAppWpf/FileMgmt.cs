using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json;
using ViewModelSupport;
using TestRunnerLib;
using System.Windows;
using Lib;

namespace TestRunnerAppWpf
{
    class FileMgmt : ViewModelBase
    {
        public static string filename { get; set; }
        public static string libFilename { get; set; }
        public static string shortFilename { get; set; }
        public static string previousDir { get; set; }
        public static bool unsavedChanges { get; set; }

        public static string ShortFilename(string filename)
        {
            return filename.Substring(filename.LastIndexOf(@"\") + 1);
        }
        public static string PreviousDir(string filename)
        {
            return filename.Substring(0, filename.LastIndexOf(@"\"));
        }

        public static SuiteModel LoadTestingSuite()
        {
            Debug.WriteLine("Getting suitemodel");

            var td = new SuiteModel();

            td.system = "troll";
            td.name = "Testningen av troll";
            td.filename = string.Empty;
            td.version = "1.0";
            td.developer = "dev";
            td.tester = "tester";
            td.notes = "Anteckningar om troll";

            var t = new TestModel();

            t.id = "1";
            t.name = "googleCheese";
            t.descExecution = "do this";
            t.descExpected = "expect that";

            t.callAss = "SeExperiments";
            t.callSpace = "SeExperiments";
            t.callType = "Ex5";
            //t.callParam = string.Empty;

            td.tests.Add(t);

            return td;

        }

        public static Tuple<string, SuiteModel> OpenSuite(string fileToOpen)
        {
            string serialized = string.Empty;
            SuiteModel openSuite = null;
            try
            {
                serialized = File.ReadAllText(fileToOpen);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error opening file: {e}");
                MessageBox.Show($"Error opening file: {e.Message}");
                return new Tuple<string, SuiteModel>(fileToOpen, null);
            }
            if (serialized.Length > 0)
            {
                try
                {
                    openSuite = JsonConvert.DeserializeObject<SuiteModel>(serialized);
                    filename = fileToOpen;
                    return new Tuple<string, SuiteModel>(fileToOpen, openSuite);
                }
                catch (JsonSerializationException e)
                {
                    Debug.WriteLine($"Error reading file: {e}");
                    MessageBox.Show($"Error reading file: {e.Message}");
                    return new Tuple<string, SuiteModel>(fileToOpen, null);
                }
            }
            else
            {
                Debug.WriteLine($"Error reading file: File empty.");
                MessageBox.Show($"Error reading file: File empty.");
                return new Tuple<string, SuiteModel>(fileToOpen, null);
            }
        }

        //public static Tuple<string, SuiteModel> OpenSuiteFrom()
        public static string OpenSuiteFrom()
        {
            //string serialized = string.Empty;
            //SuiteModel openSuite = null;
            var picker = new OpenFileDialog
            {
                Filter = "Test suites (*.testapp)|*.testapp|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            //if (!string.IsNullOrEmpty(filename))
            //    picker.InitialDirectory = PreviousDir(filename);
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousDir))
            {
                picker.InitialDirectory = Properties.Settings.Default.PreviousDir;
            }
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousFile))
            {
                picker.FileName = Properties.Settings.Default.PreviousFile;
            }
            if (picker.ShowDialog() == true)
            {
                Properties.Settings.Default.PreviousDir = PreviousDir(picker.FileName);
                Properties.Settings.Default.PreviousFile = ShortFilename(picker.FileName);
                return picker.FileName;
             
            }
            else
            {
                Debug.WriteLine("Open file cancelled");
                return null;
            }
            //filename = picker.FileName;
            //return new Tuple<string, SuiteModel>(picker.FileName, openSuite);
        }

        public static Tuple<bool, string> SaveSuite(SuiteModel suite)
        {
            if (string.IsNullOrEmpty(filename))
                return SaveAsSuite(suite);
            else
            {
                string serialized = null;
                try
                {
                    serialized = JsonConvert.SerializeObject(suite);
                }
                catch (JsonSerializationException e)
                {
                    Debug.WriteLine($"Serializing: {e}");
                }

                if (serialized != null)
                {
                    try
                    {
                        File.WriteAllText(filename, serialized);
                        return new Tuple<bool, string>(true, filename);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"Saving file: {e}");
                        return new Tuple<bool, string>(false, null);
                    }
                }
                else
                {
                    Debug.WriteLine("Serialization failed");
                    return new Tuple<bool, string>(false, null);
                }
            }
        }

        public static string Serialize(object obj)
        {
            string serialized = null;
            try
            {
                serialized = JsonConvert.SerializeObject(obj);
                //Debug.WriteLine(serialized);
            }
            catch (JsonSerializationException e)
            {
                MessageBox.Show($"Error serializing data:{Environment.NewLine}{e.Message}");
                Debug.WriteLine($"Error serializing data:{Environment.NewLine}{e}");
            }
            return serialized;
        }
        public static object Deserialize(string serialized)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject(serialized);
                return obj;
            }
            catch (JsonSerializationException e)
            {
                Debug.WriteLine($"Error deserializing: {e}");
                MessageBox.Show($"Error deserializing: {e.Message}");
                return null;
            }
        }
        public static SuiteModel DeserialSuite(string serialized)
        {
            SuiteModel openSuite = new SuiteModel();
            try
            {
                openSuite = JsonConvert.DeserializeObject<SuiteModel>(serialized);
                return openSuite;
            }
            catch (JsonSerializationException e)
            {
                Debug.WriteLine($"Error deserializing: {e}");
                MessageBox.Show($"Error deserializing: {e.Message}");
                return null;
            }
        }


        public static Tuple<bool, string> SaveAsSuite(SuiteModel suite)
        {
            string serialized = null;
            try
            {
                serialized = JsonConvert.SerializeObject(suite);
                //Debug.WriteLine(serialized);
            }
            catch (JsonSerializationException e)
            {
                MessageBox.Show($"Error serializing data:{Environment.NewLine}{e}");
                Debug.WriteLine($"Error serializing data:{Environment.NewLine}{e}");
            }

            if (serialized != null)
            {
                var picker = new SaveFileDialog
                {
                    Filter = "Test suites (*.testapp)|*.testapp|All files (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };
                //if (!string.IsNullOrEmpty(filename))
                //{
                //    picker.InitialDirectory = PreviousDir(filename);
                //    picker.FileName = ShortFilename(filename);
                //}
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousDir))
                {
                    picker.InitialDirectory = Properties.Settings.Default.PreviousDir;
                }

                //if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousFile))
                //{
                //    picker.FileName = Properties.Settings.Default.PreviousFile;
                //}
                if (!string.IsNullOrWhiteSpace(suite.filename))
                {
                    picker.FileName = ShortFilename(suite.filename);
                }
                else if (!string.IsNullOrWhiteSpace(suite.name))
                {
                    picker.FileName = suite.name;
                }
                else
                    picker.FileName = "Untitled suite";

                if (picker.ShowDialog() == true)
                {
                    try
                    {
                        File.WriteAllText(picker.FileName, serialized);
                        suite.filename = picker.FileName;
                        filename = picker.FileName; // ?
                        Properties.Settings.Default.PreviousDir = PreviousDir(picker.FileName);
                        Properties.Settings.Default.PreviousFile = ShortFilename(picker.FileName);
                        return new Tuple<bool, string>(true, picker.FileName);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"Saving file: {e}");
                        return new Tuple<bool, string>(false, null);
                    }
                }
                else
                {
                    Debug.WriteLine("Save file cancelled");
                    return new Tuple<bool, string>(false, null);
                }
            }
            else
            {
                Debug.WriteLine("Serialization failed");
                return new Tuple<bool, string>(false, null);
            }

        }

        public static string CopyTestLibraryFrom()
        {
            var picker = new OpenFileDialog
            {
                Filter = "Libraries (*.dll)|*.dll|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Import test library"
            };
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousLibDir))
            {
                picker.InitialDirectory = Properties.Settings.Default.PreviousLibDir;
            }
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousLibFile))
            {
                picker.FileName = Properties.Settings.Default.PreviousLibFile;
            }
            if (picker.ShowDialog() == true)
            {
                Properties.Settings.Default.PreviousLibDir = PreviousDir(picker.FileName);
                Properties.Settings.Default.PreviousLibFile = ShortFilename(picker.FileName);

                try
                {
                    string testsDir = AppDomain.CurrentDomain.BaseDirectory + "Tests";

                    if (!Directory.Exists(testsDir))
                        Directory.CreateDirectory(testsDir);

                    File.Copy(picker.FileName,
                        testsDir + @"\" + ShortFilename(picker.FileName),
                        true);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error copying file: {e}");
                    MessageBox.Show($"Error copying file: {e}");
                }
            }
            else
            {
                Debug.WriteLine("Copy file cancelled");
            }
            libFilename = picker.FileName;
            return picker.FileName;
        }


        public static void OpenFileSetup(string fileToOpen,
                                         MainViewModel mvm)
        {
            DetailsViewModel dvm = mvm.detailsViewModel;
            GridViewModel gvm = mvm.gridViewModel;

            Tuple<string, SuiteModel> openResult = FileMgmt.OpenSuite(fileToOpen);

            if (openResult.Item2 != null)
            {
                // Load the suite
                gvm.suite = openResult.Item2;
                
                if (openResult.Item1 != null)
                    gvm.suite.filename = openResult.Item1;

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
                            if (dvm.CanExecute_JiraGetAvailableProjectsCmd() )
                            {
                                dvm.Execute_JiraGetAvailableProjectsCmd();
                            }
                        }
                        if (dvm.jiraSelectedProject == null)
                            MessageBox.Show("Project selected in Suite not available from Jira.", "TestRunnerApp with Jira",
                                        MessageBoxButton.OK, MessageBoxImage.Exclamation);

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
            }
            else // returned suite was null
            {
                if (openResult.Item1 != null)
                    Debug.WriteLine("Failure opening suite from: unknown file");
                else
                    Debug.WriteLine($"Failure opening suite from: {openResult.Item1}");
            }

        }
    } // class FileMgmt
} // Namespace

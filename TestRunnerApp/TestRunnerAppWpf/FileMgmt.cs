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
            return filename.Substring(0,  filename.LastIndexOf(@"\"));
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
            t.callParam = string.Empty;

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
                return new Tuple<string, SuiteModel>(null, null);
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
                    return new Tuple<string, SuiteModel>(null, null);
                }
            }
            else
            {
                Debug.WriteLine($"Error reading file: File empty.");
                MessageBox.Show($"Error reading file: File empty.");
                return new Tuple<string, SuiteModel>(null, null);
            }
            //return new Tuple<string, SuiteModel>(fileToOpen, openSuite);
        }

        public static Tuple<string, SuiteModel> OpenSuiteFrom()
        {
            string serialized = string.Empty;
            SuiteModel openSuite = null;
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
                try
                {
                    serialized = File.ReadAllText(picker.FileName);

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error opening file: {e}");
                    MessageBox.Show($"Error opening file: {e.Message}");
                    return new Tuple<string, SuiteModel>(null, null);
                }
                Properties.Settings.Default.PreviousDir = PreviousDir(picker.FileName);
                Properties.Settings.Default.PreviousFile = ShortFilename(picker.FileName);
                if (serialized.Length > 0)
                {
                    try
                    {
                        openSuite = JsonConvert.DeserializeObject<SuiteModel>(serialized);
                    }
                    catch (JsonSerializationException e)
                    {
                        Debug.WriteLine($"Error reading file: {e}");
                        MessageBox.Show($"Error reading file: {e.Message}");
                        return new Tuple<string, SuiteModel>(null, null);
                    }
                }
                else
                {
                    Debug.WriteLine($"Error reading file: File empty.");
                    MessageBox.Show($"Error reading file: File empty.");
                    return new Tuple<string, SuiteModel>(null, null);
                }
            }
            else
            {
                Debug.WriteLine("Open file cancelled");
            }
            filename = picker.FileName;
            return new Tuple<string, SuiteModel>(picker.FileName, openSuite);
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
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.PreviousFile))
                {
                    picker.FileName = Properties.Settings.Default.PreviousFile;
                }
                if (picker.ShowDialog() == true)
                {
                    try
                    {
                        File.WriteAllText(picker.FileName, serialized);
                        filename = picker.FileName;
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




    }
}

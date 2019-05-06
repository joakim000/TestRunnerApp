using Microsoft.Win32;

using Newtonsoft.Json;

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

using ViewModelSupport;

namespace TestRunnerLib
{
    public class FileMgmt : ViewModelBase
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
                catch (JsonReaderException e)
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
        public static string OpenSuiteFrom(string previousDir, string previousFile)
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
            if (!string.IsNullOrWhiteSpace(previousDir))
            {
                picker.InitialDirectory = previousDir;
            }
            if (!string.IsNullOrWhiteSpace(previousFile))
            {
                picker.FileName = previousFile;
            }
            if (picker.ShowDialog() == true)
            {
                previousDir = PreviousDir(picker.FileName);
                previousFile = ShortFilename(picker.FileName);
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
                //return SaveAsSuite(suite);
                return null;
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


        public static Tuple<bool, string> SaveAsSuite(SuiteModel suite, string previousDir, string previousFile)
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
                if (!string.IsNullOrWhiteSpace(previousDir))
                {
                    picker.InitialDirectory = previousDir;
                }

                //if (!string.IsNullOrWhiteSpace(previousFile))
                //{
                //    picker.FileName = previousFile;
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
                        previousDir = PreviousDir(picker.FileName);
                        previousFile = ShortFilename(picker.FileName);
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

        public static string CopyTestLibraryFrom(string previousLibDir, string previousLibFile)
        {
            var picker = new OpenFileDialog
            {
                Filter = "Libraries (*.dll)|*.dll|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Import test library"
            };
            if (!string.IsNullOrWhiteSpace(previousLibDir))
            {
                picker.InitialDirectory = previousLibDir;
            }
            if (!string.IsNullOrWhiteSpace(previousLibFile))
            {
                picker.FileName = previousLibFile;
            }
            if (picker.ShowDialog() == true)
            {
                previousLibDir = PreviousDir(picker.FileName);
                previousLibFile = ShortFilename(picker.FileName);

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


        
    } // class FileMgmt
} // Namespace

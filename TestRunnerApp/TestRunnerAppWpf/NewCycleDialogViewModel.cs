using Newtonsoft.Json.Linq;

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TestRunnerLib;
using TestRunnerLib.Jira;

using ViewModelSupport;

namespace TestRunnerAppWpf
{
    public class CycleDialogViewModel : ViewModelBase
    {
        public MainViewModel mainViewModel
        {
            get => Get(() => mainViewModel);
            set => Set(() => mainViewModel, value);
        }
        // Needed to work with Calendar.SelectedDates
        public NewCycleDialog newCycleDialog
        {
            get => Get(() => newCycleDialog);
            set => Set(() => newCycleDialog, value);
        }
        public EditCycleDialog editCycleDialog
        {
            get => Get(() => editCycleDialog);
            set => Set(() => editCycleDialog, value);
        }
        public JiraCycle jiraSelectedCycle
        {
            get => Get(() => jiraSelectedCycle);
            set => Set(() => jiraSelectedCycle, value);
        }

        public JiraProject jiraProject
        {
            get => Get(() => jiraProject);
            set => Set(() => jiraProject, value);
        }

        public CycleModel newItem
        {
            get => Get(() => newItem);
            set => Set(() => newItem, value);
        }

        public bool noMgmt
        {
            get => Get(() => noMgmt, true);
            set => Set(() => noMgmt, value);
        }

        public bool managed
        {
            get => Get(() => managed, false);
            set => Set(() => managed, value);
        }

        // For development, later bind to newItem.jiraCycle props
        public DateTime plannedStartDateDT
        {
            get => Get(() => plannedStartDateDT);
            set => Set(() => plannedStartDateDT, value);
        }
        public DateTime plannedEndDateDT
        {
            get => Get(() => plannedEndDateDT);
            set => Set(() => plannedEndDateDT, value);
        }


        /* Commands */

        /* end: Commands */




        public CycleDialogViewModel(MainViewModel mainViewModel, NewCycleDialog newCycleDialog)
        {
            this.mainViewModel = mainViewModel;
            this.newCycleDialog = newCycleDialog;
            newItem = new CycleModel();

            if (mainViewModel.detailsViewModel.suite.mgmt?.key == "None")
            {

            }

            else if (mainViewModel.detailsViewModel.suite.mgmt?.key == "JiraCloudTmj")
            {
                if (mainViewModel.gridViewModel.suite.jiraProject == null)
                    mainViewModel.gridViewModel.suite.jiraProject = new JiraProject();
                jiraProject = mainViewModel.gridViewModel.suite.jiraProject;

                newItem.jiraCloud = true;
                newItem.jiraProjectKey = jiraProject.key;

                //if (jiraProject.cycles.Count() > 0)
                //    jiraSelectedCycle = jiraProject.cycles.First();

                if (newItem.jiraCycle == null)
                {
                    newItem.jiraCycle = new JiraCycle();
                    newItem.jiraCycle.plannedStartDateDT = DateTime.Now;
                    newItem.jiraCycle.plannedEndDateDT = DateTime.Now.AddDays(3);
                    
                }
                //newItem.jiraCycle.PropertyChanged += JiraCycle_PropertyChanged;

                //noMgmt = false;
                //managed = true;

            }
            else if (mainViewModel.detailsViewModel.suite.mgmt?.key == "ReqTest")
            {
                //noMgmt = false;
            }

            this.PropertyChanged += ViewModel_PropertyChanged;
            newItem.PropertyChanged += NewItem_PropertyChanged;

        }

        private void NewItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine("NewItem_PropertyChanged");
            CanExecute_CreateJiraCycleCmd();
        }

        private void JiraCycle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "plannedStartDateDT" || e.PropertyName == "plannedEndtDateDT")
                SetSelectedDateRange();


            if (newCycleDialog != null)
                newCycleDialog.Cal.SelectedDates.AddRange(newItem.jiraCycle.plannedStartDateDT,
                                                          newItem.jiraCycle.plannedEndDateDT);
        }

        public CycleDialogViewModel(MainViewModel mainViewModel, EditCycleDialog editCycleDialog)
        {
            this.editCycleDialog = editCycleDialog;
            this.mainViewModel = mainViewModel;
            newItem = new CycleModel();

            this.PropertyChanged += ViewModel_PropertyChanged;

            if (mainViewModel.detailsViewModel.suite.mgmt.key == "JiraCloudTmj")
            {
                if (mainViewModel.gridViewModel.suite.jiraProject == null)
                    mainViewModel.gridViewModel.suite.jiraProject = new JiraProject();
                jiraProject = mainViewModel.gridViewModel.suite.jiraProject;

                newItem.jiraCloud = true;
                newItem.jiraProjectKey = jiraProject.key;

                //if (jiraProject.cycles.Count() > 0)
                //    jiraSelectedCycle = jiraProject.cycles.First();

                if (newItem.jiraCycle == null)
                    newItem.jiraCycle = new JiraCycle();

                //noMgmt = false;
                //managed = true;

            }
            if (mainViewModel.detailsViewModel.suite.mgmt.key == "ReqTest")
            {
                //noMgmt = false;
            }
        }


        public void Execute_CreateJiraCycleCmd()
        {
            CreateJiraCycle();
        }
        public bool CanExecute_CreateJiraCycleCmd()
        {
            //if (string.IsNullOrEmpty(newItem.name))
            //    return false;


            return true;
        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"CycleDialogViewModel propchanged: {e.PropertyName}");

            if (e.PropertyName == "jiraSelectedCycle")
            {
                if (jiraSelectedCycle == null)
                {
                    managed = false;
                    return;
                }
                    

                mainViewModel.gridViewModel.suite.jiraCycle = jiraSelectedCycle;

                Debug.WriteLine($"jiracycle.key: {mainViewModel.gridViewModel.suite.jiraCycle.key}");

                // Find existing cycles with jiraCycle key
                var cyclesFromJira = mainViewModel.gridViewModel.suite.cycles.Where(x => x.jiraCycle != null);
                int existCount = cyclesFromJira.Where(x => x.jiraCycle.key == jiraSelectedCycle.key).Count();
                if (existCount > 0)
                    newItem = cyclesFromJira.Where(x => x.jiraCycle.key == jiraSelectedCycle.key).First();

                Debug.WriteLine("Found number of existing cycles with jiraCycleID: " + existCount);

                newItem.jiraCycle = jiraSelectedCycle;
                //newItem.jiraCycle.PropertyChanged += JiraCycle_PropertyChanged;

                //noMgmt = false; //Deprecated
                managed = true;

                SetSelectedDateRange();


            }

        }

        public void SetSelectedDateRange()
        {
            if (newCycleDialog != null)
            {
                 newCycleDialog.Cal.SelectedDates.AddRange(newItem.plannedStartDateDT,
                                                              newItem.plannedEndDateDT);
            }
        }

        public void Cal_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("SelcteddatesChanged");
            var cal = sender as Calendar;
            newItem.plannedStartDateDT = cal.SelectedDates.Min();
            newItem.plannedEndDateDT = cal.SelectedDates.Max();
        }

        public async void PlannedStart_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("PlannedStart_LostFocus");
            await Task.Delay(100);
            SetSelectedDateRange();
        }
        public async void PlannedEnd_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("PlannedEnd_LostFocus");
            await Task.Delay(100);
            SetSelectedDateRange();
        }

        //public string TmjDateString(DateTime dt)
        //{
        //    return dt.ToString("yyyy-MM-ddT")
        //}

        private async void CreateJiraCycle()
        {
            
            //string plannedStartDate = newItem.jiraCycle.plannedStartDate;
            //string plannedEndDate = newItem.jiraCycle.plannedEndDate;
            //string plannedStartDate = null;
            //string plannedEndDate = null;

            
            // Get accountID
            string accountId = null;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraAccountId))
                accountId = Properties.Settings.Default.JiraAccountId;
            else
            {
                var jc = new JiraConnect(mainViewModel);
                if (await jc.SetAccountId())
                {
                    Debug.WriteLine("CreateExec: Got accountId");
                    accountId = Properties.Settings.Default.JiraAccountId;
                }
                else
                {
                    string idMessage = "Create cycle: Error getting accountId, cycle will be created with no owner";
                    Debug.WriteLine(idMessage);
                    MessageBox.Show(idMessage, "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            SuiteModel suite = mainViewModel.detailsViewModel.suite;
            Tuple<HttpStatusCode, JObject> response =
                await mainViewModel.jira.CreateCycle(suite.jiraProject.key,
                                                     newItem.name,
                                                     newItem.description,
                                                     newItem.jiraCycle.plannedStartDateDT,
                                                     newItem.jiraCycle.plannedEndDateDT,
                                                     newItem.jiraCycle.jiraProjectVersion?.id,
                                                     newItem.jiraCycle.status?.name,
                                                     newItem.jiraCycle.folder?.id,
                                                     accountId);
            if (response.Item1 == HttpStatusCode.Created)
            {
                // Get cyclekey from response
                //Response content: { "id":124620,"self":"https://api.adaptavist.io/tm4j/v2/testcycles/124620","key":"TEM-R4"}
                string createdId = response.Item2.Value<string>("id");
                string createdKey = response.Item2.Value<string>("key");

                // Load cycle from jira into suite.jiraProject.cycles
                JiraCycle c = await mainViewModel.jira.LoadCycle(createdId);
                jiraProject.cycles.Add(c);

                // Select jiraCycle in NewCycleDialog
                jiraSelectedCycle = c;

            }
            else
            {
                mainViewModel.jira.ShowError(response, "creating test cycle", true);

                //int errorCode = response.Item2.Value<int>("errorCode");
                //string status = response.Item2.Value<string>("status");
                //string message = response.Item2.Value<string>("message");
                //string errorMsg = "Error creating test cycle on remote" + Environment.NewLine + Environment.NewLine
                //                  + $"{errorCode.ToString()} {status}: {message}{Environment.NewLine}";
                //MessageBox.Show(errorMsg, "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);

            }








        }

    }
}






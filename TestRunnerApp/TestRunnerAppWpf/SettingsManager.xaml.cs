using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestRunnerLib;

namespace TestRunnerAppWpf
{
    public partial class SettingsManager : Window
    {
        //public SettingsModel settings { get; set; }

        public SettingsManager()
        {
            InitializeComponent();

            //settings = new SettingsModel();
            //DataContext = settings;

            if (string.IsNullOrEmpty(Properties.Settings.Default.MgmtSystem))
                OptionsCombo.SelectedIndex = 0;
            else
                OptionsCombo.SelectedItem = Properties.Settings.Default.MgmtSystem;

            model.jiraInstance = Properties.Settings.Default.JiraInstance;
            model.jiraUser = Properties.Settings.Default.JiraUser;
            model.jiraToken = Properties.Settings.Default.JiraToken;
            model.tmjIdToken = Properties.Settings.Default.TmjIdToken;
            model.tmjKeyToken = Properties.Settings.Default.TmjKeyToken;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MgmtSystem = OptionsCombo.SelectedItem.ToString();

            Properties.Settings.Default.JiraInstance = model.jiraInstance;
            Properties.Settings.Default.JiraUser = model.jiraUser;
            Properties.Settings.Default.JiraToken = model.jiraToken;
            Properties.Settings.Default.TmjIdToken = model.tmjIdToken;
            Properties.Settings.Default.TmjKeyToken = model.tmjKeyToken;

            this.DialogResult = true;
        }



        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void OptionsCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OptionsCombo.SelectedIndex == 0)
            {
                model.jiraCloudMgmt = false;
                model.reqTestMgmt = false;
            }
            else if (OptionsCombo.SelectedIndex == 1)
            {
                model.jiraCloudMgmt = true;
                model.reqTestMgmt = false;
            }
            else if (OptionsCombo.SelectedIndex == 2)
            {
                model.jiraCloudMgmt = false;
                model.reqTestMgmt = true;
            }



        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViewModelSupport;
using TestRunnerLib;
using TestRunnerLib.Jira;
using System.Windows;
using System.Net;
using Newtonsoft.Json.Linq;

namespace TestRunnerAppWpf
{
    public partial class MainViewModel : ViewModelBase
    {
        private BackgroundWorker exportExecWorker = null;


        public void ExportCycleAsync(CycleModel cycle)
        {
            Debug.WriteLine("Export: Got cycle: " + cycle.id);

            if (gridViewModel.suite.jiraProject == null)
            {
                MessageBox.Show("Current suite has no Jira project.", "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!cycle.jiraCloud)
            {
                MessageBox.Show("Cycle is not enabled for Jira.", "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (cycle.jiraProjectKey != gridViewModel.suite.jiraProject.key)
            {
                MessageBox.Show("Cycle does not match current Jira project.", "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StartExportExecAsyncRunner(cycle);
        }

        private async void StartExportExecAsyncRunner(CycleModel c)
        {
            enableRun = false;
            progressBarValue = 0;
            runStatus = "Running";
            runTotal = c.cycleRuns.Count().ToString();
            runSlash = "/";
            runCurrent = "1";

            // Get accountID
            string accountIdForCreateExec = null;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.JiraAccountId))
                accountIdForCreateExec = Properties.Settings.Default.JiraAccountId;
            else
            {
                var jc = new JiraConnect(this);
                if (await jc.SetAccountId())
                {
                    Debug.WriteLine("CreateExec: Got accountId");
                    accountIdForCreateExec = Properties.Settings.Default.JiraAccountId;
                }
                else
                {
                    string idMessage = "CreateExec: Failed to get accountId, exec will be created with null user";
                    Debug.WriteLine(idMessage);
                    MessageBox.Show(idMessage, "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }



            string errorMsg = string.Empty;
            int successCalls = 0;
            int previouslyExported = 0;

            int done = 0;
            int total = c.cycleRuns.Count();

            foreach (CycleRun cr in c.cycleRuns)
            {
                Debug.WriteLine($"Exporting {cr.test.id} {cr.run.datetime.ToString()}");

                // Update counter
                runCurrent = (done + 1).ToString();

                if (cr.exported)
                {
                    previouslyExported++;
                }
                else
                {
                    RunModel r = cr.run;
                    Tuple<HttpStatusCode, JObject> response = await jira.CreateExec(
                                                                                    c.jiraProjectKey,
                                                                                    c.jiraCycle.key,
                                                                                    r.test.id,
                                                                                    r.result,
                                                                                    r.webDriverType,
                                                                                    r.datetimeEnd,
                                                                                    r.runTime,
                                                                                    accountIdForCreateExec,
                                                                                    r.resultObj.message);
                    Debug.WriteLine(response.Item2);
                    if (response.Item1 == HttpStatusCode.Created)
                    {
                        cr.exported = true;
                        successCalls++;
                    }
                    else
                    {
                        int errorCode = response.Item2.Value<int>("errorCode");
                        string status = response.Item2.Value<string>("status");
                        string message = response.Item2.Value<string>("message");
                        errorMsg += $"{errorCode.ToString()} {status}: {message}{Environment.NewLine}";
                    }

                }
                done++;

                // Update progressbar
                int progressPercentage = Convert.ToInt32(((double)done / total) * 100);
                progressBarValue = progressPercentage;
            } // end loop

            // Work done, time to clean up
            enableRun = true;
            runStatus = "Done";
            //runCurrent = runTotal;
            ResetProgress();


            // Show errormessages if any have been collected
            if (!string.IsNullOrEmpty(errorMsg))
            {
                if (previouslyExported > 0)
                    errorMsg += $"{Environment.NewLine}Previously exported: {previouslyExported.ToString()}";
                errorMsg += $"{Environment.NewLine}Number of successful calls: {successCalls.ToString()}";
                MessageBox.Show(errorMsg, "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (previouslyExported > 0)
            {
                errorMsg += $"{Environment.NewLine}Previously exported: {previouslyExported.ToString()}";
                errorMsg += $"{Environment.NewLine}Number of successful calls: {successCalls.ToString()}";
                MessageBox.Show(errorMsg, "TestRunnerApp with Jira", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

      

    } // MainViewModel
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ViewModelSupport;

namespace TestRunnerLib.Jira
{
    [JsonObject(MemberSerialization.OptOut)]
    public class JiraCycle : ViewModelBase
    {
        /* Properties returned by TMJ */
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public string key
        {
            get => Get(() => key);
            set => Set(() => key, value);
        }
        public string name
        {
            get => Get(() => name);
            set => Set(() => name, value);
        }
        public string description
        {
            get => Get(() => description);
            set => Set(() => description, value);
        }
        public string plannedStartDate
        {
            get => Get(() => plannedStartDate);
            set => Set(() => plannedStartDate, value);
        }
        public string plannedEndDate
        {
            get => Get(() => plannedEndDate);
            set => Set(() => plannedEndDate, value);
        }
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
        public SelectedDatesCollection plannedDates
        {
            get => Get(() => plannedDates);
            set => Set(() => plannedDates, value);
        }

        public IdSelf project
        {
            //get => Get(() => project, new IdSelf());
            get => Get(() => project);
            set => Set(() => project, value);
        }
        public JiraVersion jiraProjectVersion
        {
            //get => Get(() => jiraProjectVersion, new IdSelf());
            get => Get(() => jiraProjectVersion);
            set => Set(() => jiraProjectVersion, value);
        }
        public JiraStatus status
        {
            //get => Get(() => status, new JiraStatus());
            get => Get(() => status);
            set => Set(() => status, value);
        }
        public JiraFolder folder
        {
            //get => Get(() => folder, new IdSelf());
            get => Get(() => folder);
            set => Set(() => folder, value);
        }


        public JiraCycle() { }

        
    }

    

}
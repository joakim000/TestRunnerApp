using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelSupport;

namespace TestRunnerLib.Jira
{
    [JsonObject(MemberSerialization.OptOut)]
    public class JiraVersion : ViewModelBase
    {
        /* Properties returned by Jira API */
        public string self
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public string description
        {
            get => Get(() => description);
            set => Set(() => description, value);
        }
        public string name
        {
            get => Get(() => name);
            set => Set(() => name, value);
        }
        public bool archived
        {
            get => Get(() => archived);
            set => Set(() => archived, value);
        }
        public bool released
        {
            get => Get(() => released);
            set => Set(() => released, value);
        }
        public string releaseDate
        {
            get => Get(() => releaseDate);
            set => Set(() => releaseDate, value);
        }
        public bool overdue
        {
            get => Get(() => overdue);
            set => Set(() => overdue, value);
        }
        public string userReleaseDate
        {
            get => Get(() => releaseDate);
            set => Set(() => releaseDate, value);
        }
        public int projectId
        {
            get => Get(() => projectId);
            set => Set(() => projectId, value);
        }



        public JiraVersion() { }


    }

}
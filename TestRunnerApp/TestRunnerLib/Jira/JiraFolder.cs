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
    public class JiraFolder : ViewModelBase
    {
        /* Properties returned by TMJ */
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public string self
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }
        public int? parentId
        {
            get => Get(() => parentId);
            set => Set(() => parentId, value);
        }
        public string name
        {
            get => Get(() => name);
            set => Set(() => name, value);
        }
        public int index
        {
            get => Get(() => index);
            set => Set(() => index, value);
        }
        public string folderType
        {
            get => Get(() => folderType);
            set => Set(() => folderType, value);
        }
        public IdSelf project
        {
            get => Get(() => project);
            set => Set(() => project, value);
        }


        public JiraFolder() { }


    }

}
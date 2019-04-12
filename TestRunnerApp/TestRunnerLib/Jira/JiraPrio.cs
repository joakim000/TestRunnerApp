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
    public class JiraPrio : ViewModelBase
    {
        /* Properties returned by TMJ */
        public int id
        {
            get => Get(() => id);
            set => Set(() => id, value);
        }
        public IdSelf project
        {
            get => Get(() => project);
            set => Set(() => project, value);
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
        public int index
        {
            get => Get(() => index);
            set => Set(() => index, value);
        }
        public bool isDefault  // key "default" in API
        {
            get => Get(() => isDefault);
            set => Set(() => isDefault, value);
        }

        public string self
        {
            get => Get(() => self);
            set => Set(() => self, value);
        }


        public JiraPrio() { }


    }

}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRunnerLib;
using ViewModelSupport;

namespace TestRunnerAppCon
{
    class Model : ViewModelBase
    {
        public SuiteModel suite
        {
            get => Get(() => suite, new SuiteModel());
            set => Set(() => suite, value);
        }

        public bool unsavedChanges
        {
            get => Get(() => unsavedChanges, false);
            set => Set(() => unsavedChanges, value);
        }


        public Model()
        {

        }

    }
}

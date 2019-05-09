using Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestRunnerLib;
using ViewModelSupport;

namespace TestRunnerAppWpf
{
    class GridViewCols : ViewModelBase
    {
        public Dictionary<string, GridViewCol> cols
        {
            get => Get(() => cols, new Dictionary<string, GridViewCol>());
            set => Set(() => cols, value);
        }

        public GridViewCols(GridViewModel gvm)
        {
            // Always
            cols.Add("_id", new GridViewCol(gvm, true));
            cols.Add("_name", new GridViewCol(gvm, true));
            cols.Add("_runs", new GridViewCol(gvm, true));
            cols.Add("_previousDateTime", new GridViewCol(gvm, true));
            cols.Add("_previousOutcome", new GridViewCol(gvm, true));
            cols.Add("_runtime", new GridViewCol(gvm, true));

            // No mgmt
            cols.Add("None_prio", new GridViewCol(gvm, true, "None"));
            cols.Add("None_status", new GridViewCol(gvm, true, "None"));
            cols.Add("None_component", new GridViewCol(gvm, false, "None"));
            cols.Add("None_estimate", new GridViewCol(gvm, true, "None"));

            // JiraCloudTmj
            cols.Add("JiraCloudTmj_key", new GridViewCol(gvm, true, "JiraCloudTmj"));
            cols.Add("JiraCloudTmj_prio", new GridViewCol(gvm, true, "JiraCloudTmj"));
            cols.Add("JiraCloudTmj_status", new GridViewCol(gvm, true, "JiraCloudTmj"));
            cols.Add("JiraCloudTmj_folder", new GridViewCol(gvm, false, "JiraCloudTmj"));
            cols.Add("JiraCloudTmj_component", new GridViewCol(gvm, false, "JiraCloudTmj"));
            cols.Add("JiraCloudTmj_created", new GridViewCol(gvm, false, "JiraCloudTmj"));
            cols.Add("JiraCloudTmj_estimate", new GridViewCol(gvm, true, "JiraCloudTmj"));
        }

    }

    class GridViewCol : ViewModelBase
    {
        public Visibility visi
        {
            get => Get(() => visi);
            set => Set(() => visi, value);
        }

        public bool selected 
        {
            get => Get(() => selected, true);
            set => Set(() => selected, value);
        }

        public bool mgmtSelected
        {
            get => Get(() => mgmtSelected, false);
            set => Set(() => mgmtSelected, value);
        }

        public Managment mgmt
        {
            get => Get(() => mgmt);
            set => Set(() => mgmt, value);
        }

        public GridViewModel gvm
        {
            get => Get(() => gvm);
            set => Set(() => gvm, value);
        }


        public GridViewCol()
        {
            mgmt = Enums.Mgmt.Where(m => m.key.Equals("None")).Single();
            Setup();
        }
        public GridViewCol(string mgmtKey)
        {
            mgmt = Enums.Mgmt.Where(m => m.key.Equals(mgmtKey)).Single();
            Setup();
        }
        public GridViewCol(GridViewModel gvm, bool selected)
        {
            this.gvm = gvm;
            mgmt = Enums.Mgmt.Where(m => m.key.Equals("None")).Single();
            this.selected = selected;
            Setup();
        }
        public GridViewCol(GridViewModel gvm, bool selected, string mgmtKey)
        {
            this.gvm = gvm;
            mgmt = Enums.Mgmt.Where(m => m.key.Equals(mgmtKey)).Single();
            this.selected = selected;
            Setup();
        }

        private void Setup()
        {
            gvm.PropertyChanged += Gvm_PropertyChanged;
            gvm.suite.PropertyChanged += Suite_PropertyChanged;
            Update(gvm.suite.mgmt.key);
        }

        private void Update(string mgmtKey) =>
            visi = selected && (mgmt == null || mgmtKey.Equals(mgmt.key)) ?
                Visibility.Visible : Visibility.Collapsed;

        //private void Update(string mgmtKey)
        //{
        //    mgmtSelected = mgmtKey.Equals(mgmt.key) ? true : false;
        //    visi = selected && (mgmtSelected) ? Visibility.Visible : Visibility.Collapsed;
        //}

        private void Gvm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("suite"))
                gvm.suite.PropertyChanged += Suite_PropertyChanged;
        }

        private void Suite_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("mgmt"))
                Update(gvm.suite.mgmt.key);

        }

        

        



    }

}

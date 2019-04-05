using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViewModelSupport;

namespace TestRunnerAppWpf
{
    public partial class MainViewModel : ViewModelBase
    {
        public bool unsavedChanges
        {
            get => Get(() => unsavedChanges);
            set => Set(() => unsavedChanges, value);
        }

        /* Settings */
        public bool checkedChrome
        {
            get => Get(() => checkedChrome);
            set => Set(() => checkedChrome, value);
        }
        public bool checkedFirefox
        {
            get => Get(() => checkedFirefox);
            set => Set(() => checkedFirefox, value);
        }
        public bool checkedIE
        {
            get => Get(() => checkedIE);
            set => Set(() => checkedIE, value);
        }
        public bool checkedPhantomJS
        {
            get => Get(() => checkedPhantomJS);
            set => Set(() => checkedPhantomJS, value);
        }
        public bool checkedEdge
        {
            get => Get(() => checkedEdge);
            set => Set(() => checkedEdge, value);
        }

        public bool checkedOnTop
        {
            get => Get(() => checkedOnTop, true);
            set => Set(() => checkedOnTop, value);
        }
        public bool checkedDarkTheme
        {
            get => Get(() => checkedDarkTheme, false);
            set => Set(() => checkedDarkTheme, value);
        }

        
        /* Threading */
        private BackgroundWorker worker = null;
        SynchronizationContext uiContext = SynchronizationContext.Current;
        public int progressBarValue
        {
            get => Get(() => progressBarValue);
            set => Set(() => progressBarValue, value);
        }
        public string runStatus
        {
            get => Get(() => runStatus);
            set => Set(() => runStatus, value);
        }
        public string runCurrent
        {
            get => Get(() => runCurrent);
            set => Set(() => runCurrent, value);
        }
        public string runTotal
        {
            get => Get(() => runTotal);
            set => Set(() => runTotal, value);
        }
        public string runSlash
        {
            get => Get(() => runSlash);
            set => Set(() => runSlash, value);
        }
        public bool enableRun
        {
            get => Get(() => enableRun);
            set => Set(() => enableRun, value);
        }


        /* ViewModels */
        public GridViewModel gridViewModel
        {
            get => Get(() => gridViewModel);
            set => Set(() => gridViewModel, value);
        }
        public DetailsViewModel detailsViewModel
        {
            get => Get(() => detailsViewModel);
            set => Set(() => detailsViewModel, value);
        }


        /* Window */
        public bool topMost
        {
            get => Get(() => topMost);
            set => Set(() => topMost, value);
        }
        public System.Windows.Window appWindow
        {
            get => Get(() => appWindow);
            set => Set(() => appWindow, value);
        }
        public string windowTitle
        {
            get => Get(() => windowTitle, "TestApp by Unicus");
            set => Set(() => windowTitle, value);
        }

        /* WebDriver availibility */
        public bool chromeAvailable
        {
            get => Get(() => chromeAvailable);
            set => Set(() => chromeAvailable, value);
        }
        public bool firefoxAvailable
        {
            get => Get(() => firefoxAvailable);
            set => Set(() => firefoxAvailable, value);
        }
        public bool ieAvailable
        {
            get => Get(() => ieAvailable);
            set => Set(() => ieAvailable, value);
        }


        /* Jira */


    }
}

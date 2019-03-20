using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestRunnerAppWpf
{
    public enum Theme { Light, Dark }
    public static class Themes
    {

        public static Theme currentTheme = Theme.Light;

        //public static void ToggleTheme()
        //{
        //    if (currentTheme == Theme.Light)
        //    {
        //        Application.Current.Resources.MergedDictionaries[0].Source = new Uri($"/Themes/Dark.xaml", UriKind.Relative);
        //        currentTheme = Theme.Dark;
        //    }
        //    else
        //    {
        //        Application.Current.Resources.MergedDictionaries[0].Source = new Uri($"/Themes/Light.xaml", UriKind.Relative);
        //        currentTheme = Theme.Light;
        //    }
        //}

        public static void SetLight()
        {
            Application.Current.Resources.MergedDictionaries[0].Source = new Uri($"/Themes/Light.xaml", UriKind.Relative);
            Properties.Settings.Default.Theme = "Light";
        }

        public static void SetDark()
        {
            Application.Current.Resources.MergedDictionaries[0].Source = new Uri($"/Themes/Dark.xaml", UriKind.Relative);
            Properties.Settings.Default.Theme = "Dark";
        }


    }
}

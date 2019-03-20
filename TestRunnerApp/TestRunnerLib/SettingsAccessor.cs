

namespace TestRunnerLib
{
    public static class SettingsAccessor  
    {
        public static void SetWebDriver(string s) => Properties.Settings.Default.WebDriver = s;

        public static string GetWebDriver() => Properties.Settings.Default.WebDriver;

        public static void SaveSettings() => Properties.Settings.Default.Save();

    }
}

using AgendaLibrary.Definitions;

namespace AgendaManager.Libraries
{
    public class SettingModify
    {
        public static void ModifyLanguage(LanguagePreference lp)
        {
            Properties.Settings.Default.language = (int)lp;
            SaveSettings();
        }
        internal static void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }
    }
}

using System;
using System.Text;
using System.Globalization;
using AgendaLibrary.Definitions;
using Octokit;

namespace AgendaLibrary.Libraries
{
    public class LanguageLibrary
    {
        private LanguagePreference current_language;
        public LanguageLibrary(LanguagePreference language)
        {
            Console.OutputEncoding = Encoding.UTF8;
            current_language = language;
            LoadLocalization();
            //Console.WriteLine($"Language: {Resources.Localization.AgendaLibrary.Culture.NativeName}");
        }
        public static string GetString(string key)
        {
            string? result_string = Resources.Localization.AgendaLibrary.ResourceManager.GetString(key, Resources.Localization.AgendaLibrary.Culture);
            if (String.IsNullOrEmpty(result_string))
            {
                return "";
            } else
            {
                return result_string;
            }
        }
        public static string LPToCulture(LanguagePreference language) {
            switch (language)
            {
                case LanguagePreference.English:
                    return "en";
                case LanguagePreference.Vietnamese:
                    return "vi";
                default:
                    return "vi"; // default is Vietnamese
            }
        }
        public static LanguagePreference CultureToLP(string language)
        {
            switch (language)
            {
                case "en":
                    return LanguagePreference.English;
                case "vi":
                    return LanguagePreference.Vietnamese;
                default:
                    return LanguagePreference.Vietnamese;
            }
        }
        public void ChangeLanguage(LanguagePreference new_language)
        {
            current_language = new_language;
            LoadLocalization();
        }
        internal void LoadLocalization()
        {
            Resources.Localization.AgendaLibrary.Culture = CultureInfo.GetCultureInfo(LPToCulture(current_language));
        }
    }
}

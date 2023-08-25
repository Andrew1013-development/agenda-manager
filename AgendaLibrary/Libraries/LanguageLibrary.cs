using System;
using System.Text;
using System.Globalization;
using AgendaLibrary.Definitions;

namespace AgendaLibrary.Libraries
{
    public class LanguageLibrary
    {
        public LanguageLibrary(LanguagePreference language)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Resources.Localization.AgendaLibrary.Culture = CultureInfo.GetCultureInfo(LPToCulture(language));
            Console.WriteLine($"Language: {Resources.Localization.AgendaLibrary.Culture.NativeName}");
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
        internal static string LPToCulture(LanguagePreference language) {
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
    }
}

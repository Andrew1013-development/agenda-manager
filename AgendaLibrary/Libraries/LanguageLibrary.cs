using System;
using System.Text;
using System.Globalization;
using AgendaLibrary.Resources;

namespace AgendaLibrary.Libraries
{
    public class LanguageLibrary
    {
        public LanguageLibrary(string language)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Resources.AgendaLibrary.Culture = CultureInfo.GetCultureInfo(language);
            Console.WriteLine($"Language: {Resources.AgendaLibrary.Culture.NativeName}");
        }
        public static string GetString(string key)
        {
            string? result_string = Resources.AgendaLibrary.ResourceManager.GetString(key, Resources.AgendaLibrary.Culture);
            if (String.IsNullOrEmpty(result_string))
            {
                return "";
            } else
            {
                return result_string;
            }
        }
    }
}

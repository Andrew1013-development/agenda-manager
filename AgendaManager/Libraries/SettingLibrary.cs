using AgendaLibrary.Definitions;
using AgendaLibrary.Libraries;

namespace AgendaManager.Libraries
{
    public class SettingLibrary
    {
        public static void ChangeSettings()
        {
            string? role = "";
            string? loop = "";
            while (loop != "1")
            {
                while (String.IsNullOrEmpty(role))
                {
                    Tuple<int, bool> current_settings = ReadSettings();
                    Console.WriteLine($"{LanguageLibrary.GetString("current_settings")}:");
                    Console.WriteLine($"{LanguageLibrary.GetString("language")}: {(LanguagePreference)current_settings.Item1}");
                    Console.WriteLine($"{LanguageLibrary.GetString("debug_status")}: {current_settings.Item2}");
                    Console.WriteLine();
                    Console.WriteLine($"{LanguageLibrary.GetString("role_selection")}: ");
                    Console.WriteLine(
                        $"\t1: {LanguageLibrary.GetString("modify_language")}\n" +
                        $"\t2: {LanguageLibrary.GetString("modify_debug")}\n" +
                        $"\t3: {LanguageLibrary.GetString("exit")}"
                    );
                    Console.WriteLine($"{LanguageLibrary.GetString("specify_role_settings")}");
                    Console.Write($"{LanguageLibrary.GetString("role_selected")}: ");
                    role = Console.ReadLine();
                    if (String.IsNullOrEmpty(role))
                    {
                        Console.WriteLine($"{LanguageLibrary.GetString("empty_role_warning")}");
                    }
                }
                switch (role)
                {
                    case "1":
                        ChangeLanguageSetting();
                        break;
                    case "2":
                        ChangeDebugSetting();
                        break;
                    case "3":
                        loop = "1";
                        break;
                    default:
                        Console.WriteLine($"{LanguageLibrary.GetString("invalid_role_error")}");
                        break;
                }
                // looping
                if (role != "3")
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("exit_or_continue_settings")}");
                    Console.WriteLine(
                        $"\t0: {LanguageLibrary.GetString("continue")}\n" +
                        $"\t1: {LanguageLibrary.GetString("exit")}"
                        );
                    Console.WriteLine($"{LanguageLibrary.GetString("specify_number")}");
                    Console.Write($"{LanguageLibrary.GetString("number_specified")}: ");
                    loop = Console.ReadLine();
                    switch (loop)
                    {
                        case "0":
                            Console.WriteLine($"{LanguageLibrary.GetString("continue_1_second")}");
                            Thread.Sleep(1000);
                            Console.Clear();
                            ChangeSettings(); // recursive UI function
                            break;
                        case "1":
                            Console.WriteLine($"{LanguageLibrary.GetString("exiting")}");
                            break;
                        default:
                            Console.WriteLine($"{LanguageLibrary.GetString("invalid_option_error_settings")}");
                            Console.WriteLine($"{LanguageLibrary.GetString("continue_1_second")}");
                            Thread.Sleep(1000);
                            Console.Clear();
                            loop = "1";
                            break;
                    }
                }
            }
        }
        private static void ChangeLanguageSetting()
        {
            string? lp_choice = "";
            while (String.IsNullOrEmpty(lp_choice))
            {
                Console.WriteLine("Language selection: ");
                Console.WriteLine(
                    $"\t0: Vietnamese\n" +
                    $"\t1: English"
                    );
                Console.Write("Specify your preferred language in the designated numbers above: ");
                lp_choice = Console.ReadLine();
                if (String.IsNullOrEmpty(lp_choice))
                {
                    Console.WriteLine("Choice cannot be empty.");
                }
            }
            switch (lp_choice)
            {
                case "0":
                    ModifyLanguage(LanguagePreference.Vietnamese);
                    break;
                case "1":
                    ModifyLanguage(LanguagePreference.English);
                    break;
                default:
                    Console.WriteLine("Invalid language preference specified.");
                    break;
            }
        }
        private static void ChangeDebugSetting()
        {
            string? ds_choice = "";
            while (String.IsNullOrEmpty(ds_choice))
            {
                Console.WriteLine("Debug mode selection: ");
                Console.WriteLine(
                    $"\t0: Disable\n" +
                    $"\t1: Enable"
                    );
                Console.Write("Specify your choice in the designated number above: ");
                ds_choice = Console.ReadLine();
                if (String.IsNullOrEmpty(ds_choice))
                {
                    Console.WriteLine("Your choice cannot be empty");
                }
            }
            switch (ds_choice)
            {
                case "0":
                    ModifyDebug(false);
                    break;
                case "1":
                    ModifyDebug(true);
                    break;
                default:
                    Console.WriteLine("Invalid choice entered.");
                    break;
            }
        }
        internal static void ModifyLanguage(LanguagePreference lp)
        {
            Properties.Settings.Default.language = (int)lp;
            SaveSettings();
        }
        internal static void ModifyDebug(bool ds)
        {
            Properties.Settings.Default.debug = ds;
            SaveSettings();
        }
        internal static void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }
        internal static Tuple<int, bool> ReadSettings()
        {
            return Tuple.Create(Properties.Settings.Default.language, Properties.Settings.Default.debug);
        }
    }
}

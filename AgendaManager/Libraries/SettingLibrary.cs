using AgendaLibrary.Definitions;

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
                    Console.WriteLine("Current settings (will be applied on next start)");
                    Console.WriteLine($"Language: {(LanguagePreference)current_settings.Item1}");
                    Console.WriteLine($"Debug status: {current_settings.Item2}");
                    Console.WriteLine();
                    Console.WriteLine("Role selection: ");
                    Console.WriteLine(
                        $"\t1: Modify language settings\n" +
                        $"\t2: Modify debug mode settings\n" +
                        $"\t3: Exit"
                    );
                    Console.WriteLine("Specify what settings to modify in the designated numbers above: ");
                    Console.Write("role specified: ");
                    role = Console.ReadLine();
                    if (String.IsNullOrEmpty(role))
                    {
                        Console.WriteLine("Role cannot be empty");
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
                        break;
                    default:
                        Console.WriteLine("Invalid role entered.");
                        break;
                }
                // looping
                if (role != "3")
                {
                    Console.WriteLine("Exit or continue in this section?");
                    Console.WriteLine(
                        $"\t0: Continue\n" +
                        $"\t1: Exit"
                        );
                    Console.WriteLine("Enter a number corresponding to the option you want to select");
                    Console.Write("option specified: ");
                    loop = Console.ReadLine();
                    switch (loop)
                    {
                        case "0":
                            Console.WriteLine("Continuing in 1 second.....");
                            Thread.Sleep(1000);
                            Console.Clear();
                            ChangeSettings(); // recursive UI function
                            break;
                        case "1":
                            Console.WriteLine("Exiting.....");
                            break;
                        default:
                            Console.WriteLine("Invalid option specified, defaulting to exiting this section.");
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

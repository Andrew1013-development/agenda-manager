using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using AgendaLibrary.Libraries;

namespace AgendaLibrary.Utilities
{
    public class Credits
    {
        private static readonly List<string> testers = new List<string> { "Hamyly", "Eviel" };
        private static readonly string developer = "Andrew1013-development";
        public static void ShowCredits() {
            Console.WriteLine($"----------CREDITS----------");
            Console.WriteLine($"{LanguageLibrary.GetString("main_dev")}: {developer}");
            Console.WriteLine($"{LanguageLibrary.GetString("testers")}: ");
            for (int i = 0; i < testers.Count; i++)
            {
                Console.WriteLine($"\t{testers[i]}");
            }
            Console.WriteLine();
        }   
    }
}

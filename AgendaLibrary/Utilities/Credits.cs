using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using Windows.UI.Core;

namespace AgendaLibrary.Utilities
{
    public class Credits
    {
        public static void ShowCredits() {
            List<string> testers = new List<string> { "Hamyly", "Eviel" };
            Console.WriteLine($"----------CREDITS----------");
            Console.WriteLine("Main developer: Andrew1013-development");
            Console.WriteLine("Testers: ");
            for (int i = 0; i < testers.Count; i++)
            {
                Console.WriteLine($"\t{testers[i]}");
            }
        }   
    }
}

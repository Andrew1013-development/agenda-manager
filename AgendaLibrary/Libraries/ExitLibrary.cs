using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgendaLibrary.Definitions;
using Windows.Foundation.Metadata;

namespace AgendaLibrary.Libraries
{
    public class ExitLibrary
    {
        // for agenda manager
        public static void ProperExit(exitCode exit)
        {
            int actual_exit = (int)exit;
            Console.WriteLine($"Agenda Manager exited with code {actual_exit}");
            if (actual_exit > 0 && actual_exit != 7)
            {
                Console.WriteLine("There was a problem during execution, program is terminating");
                Console.WriteLine($"Error: {exit}");
            }
            else
            {
                Console.WriteLine("Waiting 5 seconds to exit.....");
                Thread.Sleep(5000);
            }
            Environment.Exit(actual_exit);
        }
        // for agenda manager updater
        public static void ProperExit2(exitCode exit)
        {
            int actual_exit = (int)exit;
            Console.WriteLine($"Agenda Manager Updater exited with code {actual_exit}");
            if (actual_exit > 0 && actual_exit != 7)
            {
                Console.WriteLine("There was a problem during execution, program is terminating");
                Console.WriteLine($"Error: {exit}");
            }
            else
            {
                Console.WriteLine("Waiting 5 seconds to exit.....");
                Thread.Sleep(5000);
            }
            Environment.Exit(actual_exit);
        }
        public static string GetExitDescription(exitCode exit)
        {
            return "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaLibrary
{
    public class ExitLibrary
    {
        public static void ProperExit(exitCode exit)
        {
            int actual_exit = (int)exit;
            Console.WriteLine($"Agenda Manager is exiting with code {actual_exit}");
            if (exit > 0)
            {
                Console.WriteLine("There was a problem during execution, program is terminating");
                Console.WriteLine($"Error: {exit}");
            } else
            {
                Console.WriteLine("Waiting 5 seconds to exit.....");
                Thread.Sleep(5000);
            }
            //Environment.Exit(actual_exit);
        }
    }
}

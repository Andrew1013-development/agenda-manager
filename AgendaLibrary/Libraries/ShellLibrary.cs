using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaLibrary.Libraries
{
    public class ShellLibrary
    {
        public static void Shell()
        {
            string? command = "";
            while (command.ToLower() != "exit")
            {
                Console.Write("> ");
                command = Console.ReadLine();
                Console.WriteLine($"{LanguageLibrary.GetString("command_entered")}: {command}\n");
            }
        }
    }
}

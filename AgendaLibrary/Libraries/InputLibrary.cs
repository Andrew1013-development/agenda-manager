using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace AgendaLibrary.Libraries
{
    public class InputLibrary
    {
        public static string PasswordInput()
        {
            string passphrase = String.Empty;
            ConsoleKey key;
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // read key without displaying
                key = keyInfo.Key;
                if (key == ConsoleKey.Backspace && passphrase.Length > 0)
                {
                    Console.Write("\b \b");
                    passphrase = passphrase.Substring(0, passphrase.Length - 1); // decrease length
                }
                else
                {
                    if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write('*');
                        passphrase += keyInfo.KeyChar;
                    }
                }
            } while (key != ConsoleKey.Enter);
            return passphrase;
        }

        public static string MultilineInput()
        {
            List<string?> lines = new List<string?> { };
            string? buffer = "template";
            while (!(String.IsNullOrEmpty(buffer) || String.IsNullOrWhiteSpace(buffer)))
            {
                buffer = Console.ReadLine(); //force overwrite old string
                lines.Add(buffer);
            }
            string result = "";
            foreach (string? line in lines)
            {
                result += " " + line;
            }
            return result;
        }
    }
}

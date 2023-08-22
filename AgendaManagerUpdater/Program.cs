using System;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using AgendaLibrary;

// global variable
string? am_version = "";
string? amu_version = Assembly.GetExecutingAssembly().GetName()?.Version?.ToString(); // get version from AgendaManagerUpdater
var stopwatch = new Stopwatch();
Logger logger = new Logger("updater.log");
exitCode ec = exitCode.SuccessfulExecution;

Console.WriteLine("Fetching version information from Agenda Manager.....");
if (File.Exists(@"am_version.txt"))
{
    Console.WriteLine("Version file exists, fetching.....");
    am_version = File.ReadAllText(@"am_version.txt"); // get version from AgendaManager
}
else
{
    Console.WriteLine("Version file from Agenda Manager does not exist, try again later.");
    ExitLibrary.ProperExit(exitCode.UnknownExitCode); 
}

Console.WriteLine($"Agenda Manager Updater v{amu_version} - Agenda Manager v{am_version}");
Console.WriteLine("Waiting 5 seconds before starting update process.....");
Thread.Sleep(5000);
Console.WriteLine("Downloading and installing update.....");
stopwatch.Start();
Tuple<bool, Uri, exitCode> check_update = await UpdateLibrary.CheckForUpdate(am_version);
if (check_update.Item1 == true && check_update.Item3 == exitCode.SuccessfulExecution)
{
    ec = await UpdateLibrary.DownloadUpdate(check_update.Item2);
    if (ec == exitCode.SuccessfulExecution)
    {
        ec = UpdateLibrary.InstallUpdate();
        if (ec == exitCode.SuccessfulExecution)
        {
            stopwatch.Stop();
            Console.WriteLine($"Update installed in {stopwatch.ElapsedMilliseconds} ms");
        } 
        else
        {
            Console.WriteLine("There was a problem installing the update, try again later.");
        }
    } 
    else
    {
        Console.WriteLine("There was a problem downloading the update, try again later");
    }
} else
{
    Console.WriteLine("There was a problem fetching updates, try again later");
}
ExitLibrary.ProperExit(ec);
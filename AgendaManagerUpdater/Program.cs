using System;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using AgendaLibrary;

// global variable
string? am_version = "";
if (File.Exists(@"am_version.txt"))
{
    am_version = File.ReadAllText(@"am_version.txt"); // get version from AgendaManager
}
else
{
    Console.WriteLine("Cannot read version text file, try again later.");
    ExitLibrary.ProperExit(exitCode.UnknownExitCode); 
}
string? amu_version = Assembly.GetExecutingAssembly().GetName()?.Version?.ToString(); // get version from AgendaManagerUpdater
var stopwatch = new Stopwatch();
string music_path = $@"{Environment.CurrentDirectory}\King's Court - Jose Pavli - Project Wingman Soundtrack (2020).wav";
exitCode ec = exitCode.SuccessfulExecution;
SoundPlayer player = new SoundPlayer(music_path);
Thread music_thread = new Thread(() =>
{
    player.Play(); // play music synchronously
});
music_thread.IsBackground = false;

Console.WriteLine($"Agenda Manager Updater v{amu_version} - Agenda Manager v{am_version}");
Console.WriteLine("Downloading and installing update.....");
stopwatch.Start();
Tuple<bool, Uri, exitCode> check_update = await UpdateLibrary.CheckForUpdate(am_version);
if (check_update.Item1 == true && check_update.Item3 == exitCode.SuccessfulExecution)
{
    music_thread.Start();
    ec = UpdateLibrary.DownloadUpdate(check_update.Item2);
    if (ec == exitCode.SuccessfulExecution)
    {
        ec = UpdateLibrary.InstallUpdate();
        if (ec == exitCode.SuccessfulExecution)
        {
            stopwatch.Stop();
            Console.WriteLine("You can close the updater now, but don't close if you want to hear the rest of the music :))");
            Console.WriteLine($"Update installed in {stopwatch.ElapsedMilliseconds} ms");
            music_thread.Join();
        } 
        else
        {
            Console.WriteLine("There was a problem installing the update, try again later.");
            ExitLibrary.ProperExit(ec);
        }
    } else
    {
        Console.WriteLine("There was a problem downloading the update, try again later");
        ExitLibrary.ProperExit(ec);
    }
} else
{
    Console.WriteLine("There was a problem fetching updates, try again later");
    ExitLibrary.ProperExit(ec);
}


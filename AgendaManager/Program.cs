﻿//MongoDB
using MongoDB.Driver;
using MongoDB.Bson.IO;
//System
using System.Reflection;
using System.Diagnostics;
using System.Text;
//AgendaLibrary
using AgendaLibrary;
using AgendaLibrary.Definitions;
using AgendaLibrary.Libraries;
using AgendaLibrary.Utilities;
using AgendaLibrary.Types;
using AgendaManager.Properties;
using AgendaManager.Libraries;

#region variables
// global variables
// versioning
string? version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
bool install_update = false;
Uri update_uri = new Uri("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
// mongodb credentials + settings
string databaseString = "mongodb+srv://homework-database:homework123@cluster0.ygoqv7l.mongodb.net";
string uploader_password = DateTime.Now.ToShortDateString().Replace("/","");
FilterDefinition<Agenda> agenda_filter = Builders<Agenda>.Filter.Empty;
// others
exitCode ec = exitCode.SuccessfulExecution;
var stopwatch = new Stopwatch();
Logger logger = new Logger("manager.log");
// read from settings
bool debug_mode = Settings.Default.debug;
// localization
LanguagePreference lang_setting = (LanguagePreference)Settings.Default.language; // duplicate read at the start
LanguageLibrary ll_c = new LanguageLibrary(lang_setting);
#endregion

#region startup code
// setting up
stopwatch.Start();
Console.OutputEncoding = Encoding.Unicode; // allow output of Unicode characters
Console.InputEncoding = Encoding.Unicode; // allow input of Unicode characters

// mongodb connection
Console.WriteLine($"{LanguageLibrary.GetString("establish_connection")}");
logger.LogInformation("establishing a connection to database");
MongoClientSettings settings = new MongoClientSettings(); // empty settings
try
{
    settings = MongoClientSettings.FromConnectionString(databaseString);
    settings.ConnectTimeout = TimeSpan.FromSeconds(5);
    settings.IPv6 = false;
} catch (Exception)
{
    ExitLibrary.ProperExit(exitCode.DatabaseConnectionFailure);
}
MongoClient client = new MongoClient(settings);

logger.LogInformation("getting agenda collection");
IMongoCollection<Agenda> agenda_collection = client.GetDatabase("homework-database").GetCollection<Agenda>("agenda");
List<Agenda> agenda_search = agenda_collection.Find(agenda_filter).ToList();
logger.LogInformation("getting telemetry collection");
IMongoCollection<Telemetry> telemetry_collection = client.GetDatabase("homework-database").GetCollection<Telemetry>("telemetry");
logger.LogInformation("getting bug collection");
IMongoCollection<Bug> bug_collection = client.GetDatabase("homework-database").GetCollection<Bug>("bug");

Console.WriteLine($"{LanguageLibrary.GetString("establish_connection_finish")}");
logger.LogInformation("database connection established");

// json writer
Console.WriteLine($"{LanguageLibrary.GetString("setup_json")}");
logger.LogInformation("setting up JSON writer");
var json_settings = new JsonWriterSettings { Indent = true };
logger.LogInformation("JSON writer set up");
Console.WriteLine($"{LanguageLibrary.GetString("setup_json_finish")}");

// download updater thread
//Console.WriteLine($"{LanguageLibrary.GetString("updater_thread")}");
logger.LogInformation("setting up updater thread");
Thread download_updater_thread = new Thread( async() =>
{
    // run in task so it would wait until task is finished
    Task<exitCode> download_task = UpdateLibrary.DownloadUpdater(true);
    await download_task;
    ec = download_task.Result;
});
logger.LogInformation("updater thread set up.");
//Console.WriteLine($"{LanguageLibrary.GetString("updater_thread_finish")}");

stopwatch.Stop();
logger.LogInformation($"all parts started up in {stopwatch.ElapsedMilliseconds} ms");
//Console.WriteLine($"{LanguageLibrary.GetString("wait_to_start")}");
Thread.Sleep(500);
Console.Clear();
#endregion

#region debug code
#if DEBUG
Console.WriteLine($"***{LanguageLibrary.GetString("debug_warning")}***");
#endif
#endregion
Console.WriteLine();
Console.WriteLine($"{LanguageLibrary.GetString("welcome")} v{version} - {LanguageLibrary.GetString("library")} v{ExposeVersioning.LibraryVersion()} ({LanguageLibrary.GetString("started_up")} {stopwatch.ElapsedMilliseconds} ms)");
Console.WriteLine($"{LanguageLibrary.GetString("current_date_time")}: {DateTime.Now}");

// specify role (uploader / receiver)
string? role = "";
string? loop = "";
while (loop != "1")
{
    // read settings
    lang_setting = (LanguagePreference)Settings.Default.language;
    ll_c.ChangeLanguage(lang_setting);
    Console.WriteLine($"{LanguageLibrary.GetString("agenda_database")}: {agenda_search.Count} {LanguageLibrary.GetString("agenda")}");
    Console.WriteLine();
    Console.WriteLine($"{LanguageLibrary.GetString("role_selection")}: ");
    Console.WriteLine($"\t1: {LanguageLibrary.GetString("uploader_role")}\n" +
        $"\t2: {LanguageLibrary.GetString("receiver_role")}\n" +
        $"\t3: {LanguageLibrary.GetString("pruner_role")}\n" +
        $"\t4: {LanguageLibrary.GetString("credits_role")}\n" +
        $"\t5: {LanguageLibrary.GetString("bug_report_role")}\n" +
        $"\t6: {LanguageLibrary.GetString("update_role")}\n" +
        $"\t7: {LanguageLibrary.GetString("setting_role")}\n" +
        $"\t8: {LanguageLibrary.GetString("help_role")}\n" +
        $"\t9: {LanguageLibrary.GetString("exit_role")}"
        );
    while (String.IsNullOrEmpty(role))
    {
        Console.Write($"{LanguageLibrary.GetString("specify_role")}: ");
        role = Console.ReadLine();
        logger.LogInformation($"role selected : {role}");
        if (String.IsNullOrEmpty(role))
        {
            Console.WriteLine($"{LanguageLibrary.GetString("empty_role_warning")}");
            logger.LogInformation("role input length is invalid");
        }
    }

    switch (role)
    {
        case "0": // localized
            logger.LogInformation("accessed the secret menu");
            Console.Clear();
            Console.WriteLine($"{LanguageLibrary.GetString("secret_shell_access")}");
            Console.WriteLine($"{LanguageLibrary.GetString("secret_shell_note")}");
            ShellLibrary.Shell();
            ec = exitCode.SecretMenuAccessed;
            break;
        case "1": // localized
            logger.LogInformation("accessed the upload agenda menu");
            Console.Write($"{LanguageLibrary.GetString("specify_password")}: ");
            string? output = InputLibrary.PasswordInput();
            logger.LogInformation($"password specified: {output}");
            if (output == uploader_password)
            {
                Console.Clear();
                UploadLibrary.UploadAgenda(agenda_collection, debug_mode);
                UploadLibrary.UploadTelemetry(telemetry_collection);
            }
            else
            {
                Console.WriteLine($"{LanguageLibrary.GetString("wrong_password")}");
                logger.LogInformation("wrong password specified");
                ec = exitCode.WrongPassword;
            }
            break;
        case "2": // localized
            logger.LogInformation("accessed the receive agenda menu");
            Console.Clear();
            ReceiveLibrary.ReceiveAgenda(agenda_collection);
            break;
        case "3": // localized
            logger.LogInformation("accessed the pruning agenda menu");
            Console.Clear();
            // find from filter (less than the current date on system)
            agenda_filter = Builders<Agenda>.Filter.Lte("deadline", DateTime.Now.ToShortDateString());
            agenda_search = agenda_collection.Find(agenda_filter).ToList();
            Console.WriteLine($"{LanguageLibrary.GetString("agenda_prune")}: {agenda_search.Count} {LanguageLibrary.GetString("agenda")}");
            Console.WriteLine($"{LanguageLibrary.GetString("pruning")} {agenda_search.Count} {LanguageLibrary.GetString("agenda")}.....");
            // delete individual agendas
            agenda_collection.DeleteMany(agenda_filter);
            Console.WriteLine($"{LanguageLibrary.GetString("pruned")} {agenda_search.Count} {LanguageLibrary.GetString("agenda")}.");
            break;
        case "4": // localized
            logger.LogInformation("accessed the credits menu");
            Console.Clear();
            Credits.ShowCredits();
            break;
        case "5": // localized
            logger.LogInformation("accessed the bug report menu");
            Console.Clear();
            UploadLibrary.UploadBug(bug_collection,debug_mode);
            break;
        case "6": // localized
            logger.LogInformation("accessed the update menu");
            Console.Clear();
            Tuple<bool, Uri, exitCode> update_packet = await UpdateLibrary.CheckForUpdate(version, true);
            if (update_packet.Item1)
            {
                // jump to end of program to assist updating
                install_update = true;
                update_uri = update_packet.Item2;
                // start downloading updater
                download_updater_thread.Start();
            } 
            else
            {
                ec = update_packet.Item3;
            }
            break;
        case "7": // localized
            logger.LogInformation("accessed the settings menu");
            Console.Clear();
            SettingLibrary.ChangeSettings();
            Console.Clear();
            break;
        case "8": // localized
            logger.LogInformation("accessed the help menu");
            Console.WriteLine($"{LanguageLibrary.GetString("work_in_progress")}");
            break;
        case "9": // localized
            logger.LogInformation("manual exit");
            break;
        default: // localized
            Console.WriteLine($"{LanguageLibrary.GetString("invalid_role_error")}");
            logger.LogInformation("invalid input specified");
            break;
    }

    if (role != "9" && ec == exitCode.SuccessfulExecution)
    {
        Console.WriteLine($"{LanguageLibrary.GetString("exit_or_continue_program")}");
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
                role = String.Empty;
                Console.WriteLine($"{LanguageLibrary.GetString("continue_1_second")}");
                Thread.Sleep(1000);
                Console.Clear();
                break;
            case "1":
                ec = exitCode.SuccessfulExecution;
                break;
            default:
                Console.WriteLine($"{LanguageLibrary.GetString("invalid_option_error_program")}");
                Console.WriteLine($"{LanguageLibrary.GetString("continue_1_second")}");
                Thread.Sleep(1000);
                Console.Clear();
                loop = "0";
                role = String.Empty;
                break;
        }
    } 
    else
    {
        loop = "1";
    }
}

if (install_update)
{
    // finish download updater
    download_updater_thread.Join();
    // continue update process if updater downloaded successfully
    if (ec == exitCode.SuccessfulExecution)
    {
        // drop version info .txt
        StreamWriter am_version_writer = new StreamWriter(@"am_version.txt");
        am_version_writer.WriteLine(version);
        am_version_writer.Close();
        // move into updater
        if (File.Exists("AgendaManagerUpdater.exe"))
        {
            Process.Start("AgendaManagerUpdater.exe");
        }
        else
        {
            Console.WriteLine("Updater not found, update process aborted.");
            ec = exitCode.InstallUpdateFailure;
        }
    }
}
ExitLibrary.ProperExit(ec);
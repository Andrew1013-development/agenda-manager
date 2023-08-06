using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using AgendaLibrary;
using System.Data;


// global variables to use
// versioning
string? version = Assembly.GetExecutingAssembly()?.GetName().Version?.ToString();
bool install_update = false;
Uri update_uri = new Uri("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
// mongodb credentials + settings
string connectionString = "mongodb+srv://homework-database:homework123@cluster0.ygoqv7l.mongodb.net";
string uploader_password = DateTime.Now.ToShortDateString().Replace("/","");
var agenda_filter = Builders<Agenda>.Filter.Empty;
var telemetry_filter = Builders<Telemetry>.Filter.Empty;
// others
exitCode ec = exitCode.SuccessfulExecution;
var stopwatch = new Stopwatch();
StreamWriter logFile = File.CreateText("manager.log");
Trace.Listeners.Add(new TextWriterTraceListener(logFile));
Trace.AutoFlush = true;

#region startup code
// setting up
stopwatch.Start();

// logging
Console.WriteLine("Creating log file.....");
Trace.WriteLine($"agenda-manager v{version} - running on CLR {Environment.Version}");
Console.WriteLine("Logging file created.");

// mongodb connection
Console.WriteLine("Establishing connection to database.....");
Trace.WriteLine($"{DateTime.Now} - establishing a connection to database");
var settings = MongoClientSettings.FromConnectionString(connectionString);
var client = new MongoClient(settings);

Trace.WriteLine($"{DateTime.Now} - getting agenda collection");
var agenda_collection = client.GetDatabase("homework-database").GetCollection<Agenda>("agenda");
List<Agenda> agenda_search = agenda_collection.Find(agenda_filter).ToList();

Trace.WriteLine($"{DateTime.Now} - getting telemetry collection");
var telemetry_collection = client.GetDatabase("homework-database").GetCollection<Telemetry>("telemetry");

Trace.WriteLine($"{DateTime.Now} - getting bug collection");
var bug_collection = client.GetDatabase("homework-database").GetCollection<Bug>("bug");

Console.WriteLine("Connection to database established.");
Trace.WriteLine($"{DateTime.Now} - database connection established");

// json writer
Console.WriteLine("Setting up JSON writer.....");
Trace.WriteLine($"{DateTime.Now} - setting up JSON writer");
var json_settings = new JsonWriterSettings { Indent = true };
Trace.WriteLine($"{DateTime.Now} - JSON writer set up");
Console.WriteLine("JSON writer set up.");

// download updater thread
Console.WriteLine("Setting up thread for updater download.....");
Trace.WriteLine($"{DateTime.Now} - setting up updater thread");
Thread download_thread = new Thread(async() =>
{
    exitCode download_updater = await UpdateLibrary.DownloadUpdater();
    ec = download_updater;
});
Trace.WriteLine($"{DateTime.Now} - updater thread set up.");
Console.WriteLine("Thread set up.");

stopwatch.Stop();
Trace.WriteLine($"{DateTime.Now} - all parts started up in {stopwatch.ElapsedMilliseconds} ms");
Console.WriteLine("Waiting 3 seconds until starting up.....");
Thread.Sleep(3000);
#endregion

Console.Clear();
#region debug code only
#if DEBUG
Console.WriteLine("***THIS IS A DEBUG BUILD, PERFORMANCE WILL BE REDUCED, USE WITH CAUTION.***");
#endif
#endregion
Console.WriteLine();
Console.WriteLine($"Welcome to Agenda Manager v{version} - library v{ExposeVersioning.LibraryVersion()} (started up in {stopwatch.ElapsedMilliseconds} ms)");
Console.WriteLine($"Current date and time: {DateTime.Now}");
Console.WriteLine($"Agendas on database: {agenda_search.Count} agendas");
Console.WriteLine();

// specify role (uploader / receiver)
string? role = "";
Console.WriteLine("Role selection:");
Console.WriteLine("\t1: Uploader (uploading agendas to database)\n" +
    "\t2: Receiver (receiving upcoming agendas from database)\n" +
    "\t3: Pruner (pruning old out-of-date agendas from database)\n" +
    "\t4: View credits\n" +
    "\t5: Report a bug\n" +
    "\t6: Check for updates (not finished yet)\n" +
    "\t7: Change settings (not finished yet)\n" + 
    "\t8: Summon Help\n" + 
    "\t9: Exiting (exit the program)"
    );
while (role == null || role == String.Empty) {
    Console.Write("Specify your role in the designated number above: ");
    role = Console.ReadLine();
    Trace.WriteLine($"{DateTime.Now} - role selected : {role}");
    if (role == null || role == String.Empty) {
        Console.WriteLine("Role input cannot be empty.");
        Trace.WriteLine($"{DateTime.Now} - role input length is invalid");
    }
}

switch (role)
{
    case "0":
        Trace.WriteLine($"{DateTime.Now} - accessed the secret menu");
        Console.WriteLine("You have accessed the secret menu!");
        Console.WriteLine("This is just a shell in testing, type 'exit' to exit the shell.");
        string? command = "";
        while (command.ToLower() != "exit")
        {
            Console.Write("> ");
            command = Console.ReadLine();
            Console.WriteLine($"Command you entered: {command}\n");
        }
        ec = exitCode.SecretMenuAccessed;
        break;
    case "1":
        Trace.WriteLine($"{DateTime.Now} - accessed the upload agenda menu");
        Console.Write("Specify your password: ");
        string? output = Console.ReadLine();
        Trace.WriteLine($"{DateTime.Now} - password specified: {output}");
        if (output == uploader_password)
        {
            UploadLibrary.UploadAgenda(agenda_collection);
            UploadLibrary.UploadTelemetry(telemetry_collection);
        }
        else
        {
            Console.WriteLine("Wrong password entered.");
            Trace.WriteLine($"{DateTime.Now} - wrong password specified");
            ec = exitCode.WrongPassword;
        }
        break;
    case "2":
        Trace.WriteLine($"{DateTime.Now} - accessed the receive agenda menu");
        Console.WriteLine("Fetching agenda database.....");
        // find from filter (greater than the current date on system)
        agenda_filter = Builders<Agenda>.Filter.Gte("deadline", DateTime.Now.ToShortDateString());
        agenda_search = agenda_collection.Find(agenda_filter).ToList();
        // list individual agendas searched
        Console.WriteLine($"Agendas available: {agenda_search.Count} agendas");
        for (int i = 0; i < agenda_search.Count; i++)
        {
            Console.WriteLine(agenda_search.ElementAt(i).ToBsonDocument());
        }
        break;
    case "3":
        Trace.WriteLine($"{DateTime.Now} - accessed the pruning agenda menu");
        // find from filter (less than the current date on system)
        agenda_filter = Builders<Agenda>.Filter.Lte("deadline", DateTime.Now.ToShortDateString());
        agenda_search = agenda_collection.Find(agenda_filter).ToList();
        Console.WriteLine($"Agendas to be deleted: {agenda_search.Count}");
        Console.WriteLine($"Pruning {agenda_search.Count} old agendas.....");
        // delete individual agendas
        agenda_collection.DeleteMany(agenda_filter);
        Console.WriteLine($"Pruned {agenda_search.Count} old agendas");
        break;
    case "4":
        Trace.WriteLine($"{DateTime.Now} - accessed the credits menu");
        Console.WriteLine($"----------CREDITS----------");
        Console.WriteLine("Main developer: Andrew1013-development");
        Console.WriteLine("Testers: \n" +
            "\tHamyly\n" +
            "\tEviel\n" +
            "\tMonarch\n");
        //CreditLibrary.ShowCredits();
        break;
    case "5":
        Trace.WriteLine($"{DateTime.Now} - accessed the bug report menu");
        UploadLibrary.UploadBug(bug_collection);
        break;
    case "6":
        Trace.WriteLine($"{DateTime.Now} - accessed the update menu");
        Tuple<bool, Uri, exitCode> update_packet = await UpdateLibrary.CheckForUpdate(version);
        if (update_packet.Item1)
        {
            // jump to end of program to assist updating
            install_update = true;
            update_uri = update_packet.Item2;
            // start downloading updater
            download_thread.Start();
        }
        ec = update_packet.Item3;
        break;
    case "7":
        Trace.WriteLine($"{DateTime.Now} - accessed the settings menu");
        Console.WriteLine("This function is still work-in-progress.");
        break;
    case "8":
        Trace.WriteLine($"{DateTime.Now} - accessed the help menu");
        Console.WriteLine("This function is still work-in-progress.");
        break;
    case "9":
        Trace.WriteLine($"{DateTime.Now} - manual exiting");
        Console.WriteLine("Exiting.....");
        ec = exitCode.SuccessfulExecution;
        break;
    default:
        Console.WriteLine("Invalid role specified.");
        Trace.WriteLine($"{DateTime.Now} - invalid input specified");
        break;
}
if (install_update)
{
    // drop version info .txt
    StreamWriter am_version_writer = new StreamWriter(@"am_version.txt");
    am_version_writer.WriteLine(version);
    am_version_writer.Close();
    // move into updater
    Process.Start("AgendaManagerUpdater.exe");
}
ExitLibrary.ProperExit(ec);
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using csharp_mongodb_quickstart;
using System.Reflection;
using System.Diagnostics;

/* exit code
* 0 = normal exit
* 1 = database connection cannot be established
* 2 = invalid role specified
* 3 = invalid input (empty field or invalid string to parse)
* 4 = wrong password
* 5 = secret menu accessed
*/

// global variables to use
// versioning
string? version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
bool confidential = true;
// mongodb credentials
string connectionString = "mongodb+srv://homework-database:homework123@cluster0.ygoqv7l.mongodb.net/";
string uploader_password = DateTime.Now.ToShortDateString().Replace("/","");
var stopwatch = new Stopwatch();

// setting up
// logging
stopwatch.Start();
Console.WriteLine("Creating log file.....");
StreamWriter logFile = File.CreateText("manager.log");
Trace.Listeners.Add(new TextWriterTraceListener(logFile));
Trace.AutoFlush = true;
Trace.WriteLine($"agenda-manager v{version} - running on CLR {Environment.Version}");
Console.WriteLine("Logging file created.");
// mongodb connection
Console.WriteLine("Establishing connection to database.....");
Trace.WriteLine($"{DateTime.Now} - establishing a connection to database");
var settings = MongoClientSettings.FromConnectionString(connectionString);
var empty_filter = Builders<Agenda>.Filter.Empty;
var client = new MongoClient(settings);
Trace.WriteLine($"{DateTime.Now} - getting agenda collection");
var agenda_collection = client.GetDatabase("homework-database").GetCollection<Agenda>("agenda");
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
stopwatch.Stop();
Trace.WriteLine($"{DateTime.Now} - all parts started up in {stopwatch.ElapsedMilliseconds} ms");
Console.WriteLine("Waiting 1 seconds until starting up.....");
Thread.Sleep(1000);

Console.Clear();
if (confidential) {
    Console.WriteLine("***THIS IS A CONFIDENTIAL BUILD, ANY ACTION OF BEING SUS WILL BE PUNISHED***");
}
Console.WriteLine();
Console.WriteLine($"Welcome to Agenda Manager v{version}! (started up in {stopwatch.ElapsedMilliseconds} ms)");
Console.WriteLine($"Current date and time: {DateTime.Now}");
Console.WriteLine($"Agendas on database: {agenda_collection.Find(empty_filter).CountDocuments()} agendas");
Console.WriteLine();

// specify role (uploader / receiver)
string? role = "";
Console.WriteLine("Role selection:");
Console.WriteLine("\t1: Uploader (uploading agendas to database)\n" +
    "\t2: Receiver (receiving upcoming agendas from database)\n" +
    "\t3: Pruner (pruning old out-of-date agendas from database)\n" + 
    "\t4: Exiting (exit the program)\n" +
    "\t5: View credits\n" +
    "\t6: Report a bug\n" + 
    "\t7: Check for updates (not finished yet)");
while (role == null || role == String.Empty) {
    Console.Write("Specify your role in the designated number above: ");
    role = Console.ReadLine();
    Trace.WriteLine($"{DateTime.Now} - role selected : {role}");
    if (role == "1" || role == "2" || role == "3" || role == "4" || role == "5" || role == "6" || role == "0") {
        break;
    }
    else {
        Console.WriteLine($"{DateTime.Now} - Invalid input specified.");
        Trace.WriteLine("invalid role specified");
        Environment.Exit(2);
    }
}

if (role == "1")
{
    Console.Write("Specify your password: ");
    string? output = Console.ReadLine();
    Trace.WriteLine($"{DateTime.Now} - password specified: {output}");
    if (output == uploader_password)
    {
        upload_agenda_function();
        
        upload_telemetry_function();
        AgendaLibrary.UploadLibrary.UploadAgenda();
        AgendaLibrary.UploadLibrary.UploadTelemetry();
    }
    else
    {
        Console.WriteLine("Wrong password entered.");
        Trace.WriteLine($"{DateTime.Now} - wrong password specified");
        Environment.Exit(4);
    }
}
else if (role == "2")
{
    Console.WriteLine("Fetching agenda database.....");
    // find from filter (greater than the current date on system)
    var agenda_filter = Builders<Agenda>.Filter.Gte("deadline", DateTime.Now.ToShortDateString());
    List<Agenda> agenda_search = agenda_collection.Find(agenda_filter).ToList();
    // list individual agendas searched
    Console.WriteLine($"Agendas available: {agenda_search.Count} agendas");
    for (int i = 0; i < agenda_search.Count; i++)
    {
        Console.WriteLine(agenda_search.ElementAt(i).ToBsonDocument());
    }
}
else if (role == "3")
{
    // find from filter (less than the current date on system)
    var agenda_filter = Builders<Agenda>.Filter.Lte("deadline", DateTime.Now.ToShortDateString());
    List<Agenda> agenda_search = agenda_collection.Find(agenda_filter).ToList();
    Console.WriteLine($"Agendas to be deleted: {agenda_search.Count}");
    Console.WriteLine($"Pruning {agenda_search.Count} old agendas.....");
    // delete individual agendas
    agenda_collection.DeleteMany(agenda_filter);
    Console.WriteLine($"Pruned {agenda_search.Count} old agendas");
}
else if (role == "5")
{
    Console.WriteLine($"----------CREDITS----------");
    Console.WriteLine("Main developer: Andrew1013-development");
    Console.WriteLine("Testers: \n" +
        "\tHamyly (0.1.0 -> 0.1.2)\n" +
        "\tMonarch (0.1.0 -> 0.1.2)\n");
}
else if (role == "6") 
{
    upload_bug_function();
    AgendaLibrary.UploadLibrary.UploadBug();
}
else if (role == "0")
{
    Console.WriteLine("You have accessed the secret menu!");
    Console.WriteLine("don't be gay");
    Thread.Sleep(100);
    Environment.Exit(5);
}
else
{
    Console.WriteLine("Exiting.....");
    Environment.Exit(0);
}

Console.WriteLine("\nPress Enter to exit.");
Console.ReadKey();
Environment.Exit(0);

void upload_agenda_function() {
    // user input + user error handling
    string? subject_input = "";
    while (subject_input == String.Empty)
    {
        Console.Write("Subject: ");
        subject_input = Console.ReadLine();
        if (subject_input == null) {
            Console.WriteLine("Subject cannot be empty");
        }
    }
    
    string? deadline_input = "";
    while (deadline_input == String.Empty) {
        Console.Write("Deadline: ");
        deadline_input = Console.ReadLine();
        if (DateTime.TryParse(deadline_input, out DateTime dt))
        {
            deadline_input = dt.ToShortDateString();
        }
        else
        {
            Console.WriteLine("Input does not translate to a valid day.");
        }
    }
    
    string? content_input = "";
    while (content_input == String.Empty) {
        Console.Write("Content: ");
        content_input = Console.ReadLine();
        if (content_input == null)
        {
            Console.WriteLine("Content cannot be empty");
        }
    }
    
    Console.Write("Notes: ");
    string? notes_input = Console.ReadLine();

    // turn into "data packet"
    Console.WriteLine("Uploading agenda to database.....");
    Agenda newAgenda = new()
    {
        subject = subject_input,
        deadline = deadline_input,
        content = content_input,
        notes = notes_input,
        created = DateTime.Now.ToShortDateString(),
    };
    Console.WriteLine(newAgenda.ToJson(json_settings));
    // insert data packet into database
    agenda_collection.InsertOne(newAgenda);
    Console.WriteLine("Uploaded to database.");
}

void upload_telemetry_function() {
    Console.WriteLine("Uploading telemetry data to database.....");
    Telemetry newTelemetry = new Telemetry()
    {
        machine_name = Environment.MachineName,
        platform = Environment.OSVersion,
        platform_64bit = Environment.Is64BitOperatingSystem,
        clr_version = Environment.Version,
        cpu_count = Environment.ProcessorCount,
        username = Environment.UserName,
        system_dir = Environment.SystemDirectory,
        process_id = Environment.ProcessId,
        process_path = Environment.ProcessPath,
    };
    Console.WriteLine(newTelemetry.ToJson(json_settings));
    telemetry_collection.InsertOne(newTelemetry);
    Console.WriteLine("Uploaded telemetry data to database.");
}

void upload_bug_function() {
    Console.WriteLine("TIP: Press Ctrl+Z to denote the end of input, type with lines freely");
    string? name_input = "";
    while (name_input == String.Empty) {
        Console.WriteLine("Give a brief description of the bug");
        name_input = Console.In.ReadToEnd();
        if (name_input == null)
        {
            Console.WriteLine("Bug description cannot be empty.");
        }
    }
    
    string? reproduction_input = "";
    while (reproduction_input == String.Empty) {
        Console.WriteLine("Describe how to reproduce the bug");
        reproduction_input = Console.In.ReadToEnd();
        if (reproduction_input == null)
        {
            Console.WriteLine("Bug reproduction cannot be empty.");
        }
    }

    // turn into data packet
    Bug newBug = new Bug()
    {
        bug_name = name_input,
        bug_reproduction = reproduction_input,
    };
    // insert data packet into database
    Console.WriteLine(newBug.ToJson(json_settings));
    bug_collection.InsertOne(newBug);
    Console.WriteLine("Bug reported! Thanks for your feedback");
}
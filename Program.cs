using MongoDB.Driver;
using MongoDB.Bson;
using csharp_mongodb_quickstart;
using System.Collections.Specialized;
using Windows.Management.Update;

string version = "1.0.0";
string? connectionString = "mongodb+srv://homework-database:homework123@cluster0.ygoqv7l.mongodb.net/";
if (connectionString == string.Empty) {
    Console.WriteLine("no connection string specified in source code");   
    Environment.Exit(1);
}

// establish connection
Console.WriteLine("Establishing connection to database.....");
var client = new MongoClient(connectionString);
var agenda_collection = client.GetDatabase("homework-database").GetCollection<Agenda>("agenda");
var empty_filter = Builders<Agenda>.Filter.Empty;
Console.WriteLine("Connection to database established.");
Console.WriteLine();
Console.WriteLine($"Welcome to Agenda Manager v{version}!");
Console.WriteLine($"Current date and time: {DateTime.Now}");
Console.WriteLine($"Agendas on database: {agenda_collection.Find(empty_filter).CountDocuments()} agendas");
Console.WriteLine();

// specify role (uploader / receiver)
string? role = "";
Console.WriteLine("What is your role? 1 for uploader, 2 for receiver, 3 for exiting");
while (role == string.Empty || role == null) {
    Console.Write("Role: ");
    role = Console.ReadLine();
    if (role == "1" || role == "2" || role == "3") {
        break;
    }
    else {
        Console.WriteLine("Invaild input specified.");
    }
}

if (role == "1")
{
    // user input
    Console.Write("Subject: ");
    string? subject_input = Console.ReadLine();
    Console.Write("Deadline: ");
    string? deadline_input = DateTime.Parse(Console.ReadLine()).ToShortDateString();
    Console.Write("Content: ");
    string? content_input = Console.ReadLine();
    Console.Write("Notes: ");
    string? notes_input = Console.ReadLine();

    Console.WriteLine("Uploading to database.....");
    // turn into "data packet"
    Agenda newAgenda = new()
    {
        subject = subject_input,
        deadline = deadline_input,
        content = content_input,
        notes = notes_input,
        created = DateTime.Now.ToShortDateString(),
    };
    Console.WriteLine(newAgenda.ToBsonDocument());
    // insert data packet into database
    agenda_collection.InsertOne(newAgenda);
    Console.WriteLine("Uploaded to database.");
}
else if (role == "2")
{
    // find from filter (greater than the current date on system)
    var agenda_filter = Builders<Agenda>.Filter.Gte("deadline", DateTime.Now.ToShortDateString());
    List<Agenda> agenda_search = agenda_collection.Find(agenda_filter).ToList();
    // list individual agendas searched
    Console.WriteLine($"Agendas available: {agenda_search.Count()} agendas");
    for (int i = 0; i < agenda_search.Count; i++)
    {
        Console.WriteLine(agenda_search.ElementAt(i).ToBsonDocument());
    }
}
else {
    Console.WriteLine("Exiting.....");
    Environment.Exit(0);
}

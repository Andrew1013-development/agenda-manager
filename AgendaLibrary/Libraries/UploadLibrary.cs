using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System.Runtime.CompilerServices;

namespace AgendaLibrary
{
    public class UploadLibrary
    {
        public static async void UploadAgenda(IMongoCollection<Agenda> agenda_collection) 
        {
            var json_settings = new JsonWriterSettings { Indent = true };
            string? subject_input = "";
            while (subject_input == String.Empty)
            {
                Console.Write("Subject: ");
                subject_input = Console.ReadLine();
                if (String.IsNullOrEmpty(subject_input))
                {
                    Console.WriteLine("Subject cannot be empty");
                }
            }

            string? deadline_input = "";
            while (deadline_input == String.Empty)
            {
                Console.Write("Deadline: ");
                deadline_input = Console.ReadLine();
                if (DateTime.TryParse(deadline_input, out DateTime dt))
                {
                    deadline_input = dt.ToShortDateString();
                    break;
                }
                else
                {
                    if (String.IsNullOrEmpty(deadline_input))
                    {
                        Console.WriteLine("Date cannot be empty.");
                    }
                    else
                    {
                        Console.WriteLine("Date enter does not translate to a valid day.");
                    }
                }
            }

            string? content_input = "";
            while (content_input == String.Empty)
            {
                Console.Write("Content: ");
                content_input = Console.ReadLine();
                if (String.IsNullOrEmpty(content_input))
                {
                    Console.WriteLine("Content cannot be empty");
                }
            }

            Console.Write("Notes: ");
            string? notes_input = Console.ReadLine();

            // turn into "data packet"
            Console.WriteLine("Uploading agenda to database.....");
            Agenda newAgenda = new Agenda(subject_input, deadline_input, content_input, notes_input);
            // insert data packet into database (async)
            Task uploadTask = agenda_collection.InsertOneAsync(newAgenda);
            Console.WriteLine(newAgenda.ToJson(json_settings));
            await uploadTask;
            Console.WriteLine("Uploaded agenda to database.");
        }
        public static void UploadBug(IMongoCollection<Bug> bug_collection) 
        {
            var json_settings = new JsonWriterSettings { Indent = true };
            Console.WriteLine("TIP: Press Ctrl+Z to denote the end of input, type with lines freely");
            string? name_input = "";
            while (name_input == String.Empty)
            {
                Console.WriteLine("Give a brief description of the bug");
                name_input = Console.In.ReadToEnd();
                if (String.IsNullOrEmpty(name_input))
                {
                    Console.WriteLine("Bug description cannot be empty.");
                }
            }

            string? reproduction_input = "";
            while (reproduction_input == String.Empty)
            {
                Console.WriteLine("Describe how to reproduce the bug");
                reproduction_input = Console.In.ReadToEnd();
                if (String.IsNullOrEmpty(reproduction_input))
                {
                    Console.WriteLine("Bug reproduction cannot be empty.");
                }
            }

            // turn into data packet
            Bug newBug = new Bug(name_input, reproduction_input);
            // insert data packet into database
            Console.WriteLine(newBug.ToJson(json_settings));
            bug_collection.InsertOne(newBug);
            Console.WriteLine("Bug reported! Thanks for your feedback!");
        }
        public static async void UploadTelemetry(IMongoCollection<Telemetry> telemetry_collection)
        {
            var json_settings = new JsonWriterSettings { Indent = true };
            Telemetry newTelemetry = new Telemetry();
            Console.WriteLine("Uploading telemetry data to database.....");
            Task uploadTask = telemetry_collection.InsertOneAsync(newTelemetry);
            Console.WriteLine(newTelemetry.ToJson(json_settings));
            await uploadTask;
            Console.WriteLine("Uploaded telemetry data to database.");
        }
    }
}
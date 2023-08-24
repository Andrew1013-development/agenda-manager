using AgendaLibrary.Types;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System.Runtime.CompilerServices;
using System.Text;

namespace AgendaLibrary.Libraries
{
    public class UploadLibrary
    {     
        public static async void UploadAgenda(IMongoCollection<Agenda> agenda_collection) 
        {
            var json_settings = new JsonWriterSettings { Indent = true };
            string? subject_input = "";
            while (String.IsNullOrEmpty(subject_input))
            {
                Console.Write($"{LanguageLibrary.GetString("subject")}: ");
                subject_input = Console.ReadLine();
                if (String.IsNullOrEmpty(subject_input))
                {
                    Console.WriteLine("Subject cannot be empty");
                }
            }

            string? deadline_input = "";
            bool deadline_input_check = false;
            while (!deadline_input_check)
            {
                Console.Write($"{LanguageLibrary.GetString("deadline")}: ");
                deadline_input = Console.ReadLine();
                if (DateTime.TryParse(deadline_input, out DateTime dt))
                {
                    if (dt > DateTime.Now)
                    {
                        deadline_input = dt.ToShortDateString();
                        deadline_input_check = true;
                    } 
                    else
                    {
                        Console.WriteLine("Date entered cannot be earlier than or equal to today.");
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(deadline_input))
                    {
                        Console.WriteLine("Date cannot be empty.");
                    }
                    else
                    {
                        Console.WriteLine("Date entered does not translate to a valid day.");
                    }
                }
            }

            string? content_input = "";
            while (String.IsNullOrEmpty(content_input))
            {
                Console.Write($"{LanguageLibrary.GetString("content")}: ");
                content_input = Console.ReadLine();
                if (String.IsNullOrEmpty(content_input))
                {
                    Console.WriteLine("Content cannot be empty.");
                }
            }

            Console.Write($"{LanguageLibrary.GetString("notes")}: ");
            string? notes_input = Console.ReadLine();
            Console.WriteLine(subject_input);
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
            while (String.IsNullOrEmpty(name_input))
            {
                Console.WriteLine("Give a brief description of the bug");
                name_input = Console.In.ReadToEnd();
                if (String.IsNullOrEmpty(name_input))
                {
                    Console.WriteLine("Bug description cannot be empty.");
                }
            }

            string? reproduction_input = "";
            while (String.IsNullOrEmpty(reproduction_input))
            {
                Console.WriteLine("Describe how to reproduce the bug");
                reproduction_input = Console.In.ReadToEnd();
                if (String.IsNullOrEmpty(reproduction_input))
                {
                    Console.WriteLine("Bug reproduction cannot be empty.");
                }
            }

            // turn into data packet
            reproduction_input = bug_reproduction_processing(reproduction_input);
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
            //Console.WriteLine("Uploading telemetry data to database.....");
            Task uploadTask = telemetry_collection.InsertOneAsync(newTelemetry);
            //Console.WriteLine(newTelemetry.ToJson(json_settings));
            await uploadTask;
            //Console.WriteLine("Uploaded telemetry data to database.");
        }
        internal static string bug_reproduction_processing(string bug_reproduction_original)
        {
            string bug_reproducion_processed = bug_reproduction_original.Replace("\n", "");
            return bug_reproducion_processed;
        }
    }
}
using AgendaLibrary.Types;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace AgendaLibrary.Libraries
{
    public class UploadLibrary
    {
        static JsonWriterSettings json_settings = new JsonWriterSettings { Indent = true };
        public static async void UploadAgenda(IMongoCollection<Agenda> agenda_collection, bool debug) 
        {
            string? subject_input = "";
            while (String.IsNullOrEmpty(subject_input))
            {
                Console.Write($"{LanguageLibrary.GetString("subject")}: ");
                subject_input = Console.ReadLine();
                if (String.IsNullOrEmpty(subject_input))
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("empty_subject_error")}");
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
                        Console.WriteLine($"{LanguageLibrary.GetString("early_date_error")}");
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(deadline_input))
                    {
                        Console.WriteLine($"{LanguageLibrary.GetString("empty_date_error")}");
                    }
                    else
                    {
                        Console.WriteLine($"{LanguageLibrary.GetString("invalid_date_error")}");
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
                    Console.WriteLine($"{LanguageLibrary.GetString("empty_content_error")}");
                }
            }

            Console.Write($"{LanguageLibrary.GetString("notes")}: ");
            string? notes_input = Console.ReadLine();
            
            // turn into "data packet"
            Console.WriteLine($"{LanguageLibrary.GetString("upload_agenda")}");
            Agenda newAgenda = new Agenda(subject_input, deadline_input, content_input, notes_input);
            // insert data packet into database (async)
            Task uploadTask = agenda_collection.InsertOneAsync(newAgenda);
            Console.WriteLine(newAgenda.ToJson(json_settings));
            await uploadTask;
            Console.WriteLine($"{LanguageLibrary.GetString("upload_agenda_finish")}");
        }
        public static async void UploadBug(IMongoCollection<Bug> bug_collection, bool debug) 
        {
            Console.WriteLine($"{LanguageLibrary.GetString("bug_report_tip")}");
            string? name_input = "";
            while (String.IsNullOrEmpty(name_input))
            {
                Console.WriteLine($"{LanguageLibrary.GetString("bug_name")}");
                name_input = multi_line_input();
                if (String.IsNullOrEmpty(name_input))
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("empty_bug_name_error")}");
                }
            }

            string? reproduction_input = "";
            while (String.IsNullOrEmpty(reproduction_input))
            {
                Console.WriteLine($"{LanguageLibrary.GetString("bug_reproduction")}");
                reproduction_input = multi_line_input();
                if (String.IsNullOrEmpty(reproduction_input))
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("empty_bug_reproduction_error")}");
                }
            }

            // turn into data packet
            Bug newBug = new Bug(name_input, reproduction_input);
            // insert data packet into database
            Console.WriteLine($"{LanguageLibrary.GetString("bug_report")}");
            Task uploadTask = bug_collection.InsertOneAsync(newBug);
            if (debug)
            {
                Console.WriteLine(newBug.ToJson(json_settings));
            }
            await uploadTask;
            Console.WriteLine($"{LanguageLibrary.GetString("bug_report_finish")}");
        }
        public static async void UploadTelemetry(IMongoCollection<Telemetry> telemetry_collection)
        {
            Telemetry newTelemetry = new Telemetry();
            //Console.WriteLine("Uploading telemetry data to database.....");
            Task uploadTask = telemetry_collection.InsertOneAsync(newTelemetry);
            //Console.WriteLine(newTelemetry.ToJson(json_settings));
            await uploadTask;
            //Console.WriteLine("Uploaded telemetry data to database.");
        }
        internal static string multi_line_input()
        {
            List<string?> lines = new List<string?> { };
            string? buffer = "malai day hohohohoh";
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

    public class UploadLibraryGraphics
    {
        public static bool UploadAgenda(Agenda newAgenda, IMongoCollection<Agenda> agenda_collection)
        {
            // input validation
            if (String.IsNullOrEmpty(newAgenda.subject))
            {
                return false;
            }
            if (String.IsNullOrEmpty(newAgenda.content))
            {
                return false;
            }
            // upload
            agenda_collection.InsertOne(newAgenda);
            return true;
        }
        public static void UploadBug(Bug newBug, IMongoCollection<Bug> bug_collection)
        {
            bug_collection.InsertOne(newBug);
        }
        public static void UploadTelemetry(Telemetry newTelemetry, IMongoCollection<Telemetry> telemetry_collection)
        {
            telemetry_collection.InsertOne(newTelemetry);
        }
    }
}
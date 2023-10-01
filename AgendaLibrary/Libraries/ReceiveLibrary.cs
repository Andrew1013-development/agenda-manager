using AgendaLibrary.Types;
using MongoDB.Driver;

namespace AgendaLibrary.Libraries
{
    public class ReceiveLibrary
    {
        public static void ReceiveAgenda(IMongoCollection<Agenda> agenda_collection)
        {
            Console.WriteLine($"{LanguageLibrary.GetString("fetch_database")}");
            // find from filter (greater than the current date on system)
            FilterDefinition<Agenda> agenda_filter = Builders<Agenda>.Filter.Gt("deadline", DateTime.Now.ToShortDateString());
            List<Agenda> agenda_search = agenda_collection.Find(agenda_filter).ToList();
            agenda_search = agenda_search.FindAll(a => DateTime.Parse(a.deadline) > DateTime.Now); //temporary fix when comparing date strings
            // list individual agendas searched
            Console.WriteLine($"{LanguageLibrary.GetString("agenda_available")}: {agenda_search.Count} {LanguageLibrary.GetString("agenda")}");
            for (int i = 0; i < agenda_search.Count; i++)
            {
                Console.WriteLine($"{LanguageLibrary.GetString("agenda")} {i + 1}");
                Console.WriteLine($"{LanguageLibrary.GetString("subject")}: {agenda_search.ElementAt(i).subject}");
                Console.WriteLine($"{LanguageLibrary.GetString("content")}: {agenda_search.ElementAt(i).content}");
                Console.WriteLine($"{LanguageLibrary.GetString("deadline")}: {agenda_search.ElementAt(i).deadline}");
                if (!String.IsNullOrEmpty(agenda_search.ElementAt(i).notes))
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("notes")}: {agenda_search.ElementAt(i).notes}");
                }
                Console.WriteLine();
            }
        }
    }

    public class ReceiveLibraryGraphics
    {
        public static List<Agenda> ReceiveAgenda(IMongoCollection<Agenda> agenda_collection)
        {
            FilterDefinition<Agenda> agenda_filter = Builders<Agenda>.Filter.Gt("deadline", DateTime.Now.ToShortDateString());
            List<Agenda> agenda_search = agenda_collection.Find(agenda_filter).ToList();
            agenda_search = agenda_search.FindAll(a => DateTime.Parse(a.deadline) > DateTime.Now); //temporary fix when comparing date strings
            return agenda_search;
        }
    }
}

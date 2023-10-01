using MongoDB.Bson;
using System.Collections;

namespace AgendaLibrary.Types
{
    public class Agenda : IEquatable<Agenda> ,IComparable<Agenda>
    {
        public ObjectId Id { get; set; }
        public string subject { get; set; }
        public string deadline { get; set; }
        public string content { get; set; }
        public string? notes { get; set; }
        public string created { get; set; }
        public Agenda(string agenda_subject, string agenda_deadline, string agenda_content, string? agenda_notes) 
        {
            subject = agenda_subject;
            deadline = agenda_deadline;
            content = agenda_content;
            notes = agenda_notes;
            created = DateTime.Now.ToString();
        }

        public int CompareTo(Agenda that)
        {
            return this.deadline.CompareTo(that.deadline);
        }

        public bool Equals(Agenda that)
        {
            return this.Equals(that);
        }
    }
}

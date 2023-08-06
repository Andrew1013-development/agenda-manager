using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaLibrary
{
    public class Agenda {
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
    }
}

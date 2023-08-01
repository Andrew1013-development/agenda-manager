using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_mongodb_quickstart
{
    public class Agenda {
        public ObjectId Id { get; set; }
        public string? subject { get; set; }
        public string? deadline { get; set; }
        public string? content { get; set; }
        public string? notes { get; set; }
        public string? created { get; set; }
    }
}

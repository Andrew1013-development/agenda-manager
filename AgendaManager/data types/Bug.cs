using MongoDB.Bson;

namespace csharp_mongodb_quickstart
{
    public class Bug
    {
        public ObjectId Id { get; set; }
        public string bug_name { get; set; }
        public string bug_reproduction { get; set; }
        public Bug(string name, string reproduction) 
        {
            bug_name = name;
            bug_reproduction = reproduction;
        }
    }
}

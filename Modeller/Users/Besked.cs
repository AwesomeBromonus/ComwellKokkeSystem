using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Modeller
{
    public class Besked
    {
        public int Id { get; set; }

      
        public int AfsenderId { get; set; }

       
        public int ModtagerId { get; set; }

       
        public string Indhold { get; set; } = "";

      
        public DateTime Tidsstempel { get; set; }

    
        public bool ErGruppeBesked { get; set; }
    }
}

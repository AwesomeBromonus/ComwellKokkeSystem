using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Modeller
{
    
    public class Underdelmaal
    {
        public int Id { get; set; }
        [BsonElement("DelmaalId")]
        public int DelmaalId { get; set; } // FK til det oprettede delmål
        public string Beskrivelse { get; set; } = "";
        public string Status { get; set; } = "Ikke påbegyndt";
        public int DeadlineOffsetDage { get; set; } = 0;
    }

}

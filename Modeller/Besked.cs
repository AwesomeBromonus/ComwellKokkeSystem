using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Modeller
{
          public class Besked
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public int Id { get; set; }  // Unik ID for beskeden

            public int AfsenderId { get; set; }  // ID på den bruger, der sender beskeden
            public int ModtagerId { get; set; }  // ID på den bruger eller gruppe, der modtager beskeden
            public string Indhold { get; set; }  // Selve beskedteksten
            public DateTime Tidsstempel { get; set; }  // Tidspunkt for afsendelse
            public bool ErGruppeBesked { get; set; }  // Angiver om beskeden er en gruppebesked
        }
    }

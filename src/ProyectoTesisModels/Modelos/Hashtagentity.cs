using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoTesisModels.Modelos
{
    [BsonIgnoreExtraElements]
    public class Hashtagentity
    {
        public string text { get; set; }
        public int start { get; set; }
        public int end { get; set; }
    }
}

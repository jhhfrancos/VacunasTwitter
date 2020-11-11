using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoTesisModels.Modelos
{
    [BsonIgnoreExtraElements]
    public class Usermentionentity
    {
        public string name { get; set; }
        public string screenName { get; set; }
        public int id { get; set; }
        public int start { get; set; }
        public int end { get; set; }
    }
}

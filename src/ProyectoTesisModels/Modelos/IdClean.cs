using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ProyectoTesisModels.Modelos
{
    [BsonIgnoreExtraElements]
    public class IdClean
    {
        public ObjectId id { get; set; }
        public Int64 user { get; set; }
    }
}
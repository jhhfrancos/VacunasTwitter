using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ProyectoTesisModels.Modelos
{
    [BsonIgnoreExtraElements]
    public class IdClean
    {
        public Int64 idTweet { get; set; }
        public Int64 idUser { get; set; }
    }
}
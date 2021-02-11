using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoTesisModels.Modelos
{
    [BsonIgnoreExtraElements]
    public class TweetClean
    {
        public IdClean _id { get; set; }
        public Value value { get; set; }
    }
}

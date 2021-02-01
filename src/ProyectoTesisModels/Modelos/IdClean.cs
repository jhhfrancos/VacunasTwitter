using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProyectoTesisModels.Modelos
{
    [BsonIgnoreExtraElements]
    public class IdClean
    {
        public ObjectId id { get; set; }
        public int user { get; set; }
    }
}
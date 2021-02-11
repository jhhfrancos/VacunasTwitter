using MongoDB.Bson.Serialization.Attributes;

namespace ProyectoTesisModels.Modelos
{
    [BsonIgnoreExtraElements]
    public class Value
    {
        public string texto { get; set; }
        public string textoStop { get; set; }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProyectoTesisModels.Modelos
{
    [BsonIgnoreExtraElements]
    public class Mediaentity
    {
        [BsonId]
        [DataMember]
        public ObjectId id { get; set; }
        public string url { get; set; }
        public string mediaURL { get; set; }
        public string mediaURLHttps { get; set; }
        public string expandedURL { get; set; }
        public string displayURL { get; set; }
        public Sizes sizes { get; set; }
        public string type { get; set; }
        public int videoAspectRatioWidth { get; set; }
        public int videoAspectRatioHeight { get; set; }
        public int videoDurationMillis { get; set; }
        public object[] videoVariants { get; set; }
        public string extAltText { get; set; }
        public int start { get; set; }
        public int end { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Sizes
    {
        public _0 _0 { get; set; }
        public _1 _1 { get; set; }
        public _2 _2 { get; set; }
        public _3 _3 { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class _0
    {
        public int width { get; set; }
        public int height { get; set; }
        public int resize { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class _1
    {
        public int width { get; set; }
        public int height { get; set; }
        public int resize { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class _2
    {
        public int width { get; set; }
        public int height { get; set; }
        public int resize { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class _3
    {
        public int width { get; set; }
        public int height { get; set; }
        public int resize { get; set; }
    }
}

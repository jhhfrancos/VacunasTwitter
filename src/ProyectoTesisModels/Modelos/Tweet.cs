using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;

namespace ProyectoTesisModels.Modelos
{
    [BsonIgnoreExtraElements]
    public class Tweet
    {
        //public Id _id { get; set; }
        public string createdAt { get; set; }
        [BsonId]
        [DataMember]
        public ObjectId id { get; set; }
        public string text { get; set; }
        public int displayTextRangeStart { get; set; }
        public int displayTextRangeEnd { get; set; }
        public string source { get; set; }
        public bool isTruncated { get; set; }
        public Int64 inReplyToStatusId { get; set; }
        public Int64 inReplyToUserId { get; set; }
        public bool isFavorited { get; set; }
        public bool isRetweeted { get; set; }
        public int favoriteCount { get; set; }
        public string inReplyToScreenName { get; set; }
        public int retweetCount { get; set; }
        public bool isPossiblySensitive { get; set; }
        public string lang { get; set; }
        public object[] contributorsIDs { get; set; }
        public Tweet retweetedStatus { get; set; }
        public Usermentionentity[] userMentionEntities { get; set; }
        public object[] urlEntities { get; set; }
        public Hashtagentity[] hashtagEntities { get; set; }
        public Mediaentity[] mediaEntities { get; set; }
        public object[] symbolEntities { get; set; }
        public int currentUserRetweetId { get; set; }
        public User user { get; set; }
        public Int64 quotedStatusId { get; set; }

        override public string ToString()
        {
            return this.text;
        }
    }




}




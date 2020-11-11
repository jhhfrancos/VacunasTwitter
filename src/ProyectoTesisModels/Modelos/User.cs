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
    public class User
    {
        [BsonId]
        [DataMember]
        public ObjectId id { get; set; }
        public string name { get; set; }
        public string screenName { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public object[] descriptionURLEntities { get; set; }
        public bool isContributorsEnabled { get; set; }
        public string profileImageUrl { get; set; }
        public string profileImageUrlHttps { get; set; }
        public bool isDefaultProfileImage { get; set; }
        public bool isProtected { get; set; }
        public int followersCount { get; set; }
        public string profileBackgroundColor { get; set; }
        public string profileTextColor { get; set; }
        public string profileLinkColor { get; set; }
        public string profileSidebarFillColor { get; set; }
        public string profileSidebarBorderColor { get; set; }
        public bool profileUseBackgroundImage { get; set; }
        public bool isDefaultProfile { get; set; }
        public bool showAllInlineMedia { get; set; }
        public int friendsCount { get; set; }
        public string createdAt { get; set; }
        public int favouritesCount { get; set; }
        public int utcOffset { get; set; }
        public string profileBackgroundImageUrl { get; set; }
        public string profileBackgroundImageUrlHttps { get; set; }
        public string profileBannerImageUrl { get; set; }
        public bool profileBackgroundTiled { get; set; }
        public int statusesCount { get; set; }
        public bool isGeoEnabled { get; set; }
        public bool isVerified { get; set; }
        public bool translator { get; set; }
        public int listedCount { get; set; }
        public bool isFollowRequestSent { get; set; }
    }
}

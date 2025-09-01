﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace property.Domain.Entities
{
    public class Owner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOwner { get; set; }
	    public string Name { get; set; }
        public string Address { get; set; }
        public byte[] Photo { get; set; }
        public DateTime Birthday { get; set; }
    }
}

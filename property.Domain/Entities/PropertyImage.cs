using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace property.Domain.Entities
{
    public class PropertyImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPropertyImage { get; set; }
        public byte[] File { get; set; }
        public bool Enabled { get; set; }
        public string IdProperty { get; set; }
    }
}

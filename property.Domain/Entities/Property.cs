using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace property.Domain.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdProperty { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Price { get; set; }
        public long CodeInternal { get; set; }
        public int Year { get; set; }
        public string IdOwner { get; set; }
    }
}

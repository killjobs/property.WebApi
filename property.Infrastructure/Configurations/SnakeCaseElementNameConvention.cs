using MongoDB.Bson.Serialization.Conventions;
using System.Text.RegularExpressions;

namespace property.Infrastructure.Configurations
{
    public class SnakeCaseElementNameConvention : IMemberMapConvention
    {
        public string Name => "SnakeCase";
        public void Apply(MongoDB.Bson.Serialization.BsonMemberMap memberMap)
        {
            var originalName = memberMap.MemberName;
            var snakeCaseName = Regex.Replace(originalName, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
            memberMap.SetElementName(snakeCaseName);
        }
    }
}

using MongoDB.Driver;
using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Repositories;
using property.Infrastructure.Context;

namespace property.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _collection;
        public PropertyRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<Property>("Properties");
        }
        public async Task<Property> AddPropertyAsync(Property property)
        {
            await _collection.InsertOneAsync(property);

            return property;
        }
        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
        public async Task<Property> GetPropertyByIdAsync(string idProperty)
        {
            return await _collection.Find(p => p.IdProperty == idProperty).FirstOrDefaultAsync();
        }
        public async Task<List<Property>> GetPropertyByIdOwnerAsync(string idOwner)
        {
            return await _collection.Find(p => p.IdOwner == idOwner).ToListAsync();
        }
        public async Task<List<Property>> GetPropertyByPriceAsync(CompletePropertyRequestDto completePropertyRequestDto, string idOwner)
        {
            var builder = Builders<Property>.Filter;

            var filter = builder.Eq(p => p.IdOwner, idOwner);
            if (completePropertyRequestDto.MaxPrice > 0){
                filter &= builder.Gte(p => p.Price, completePropertyRequestDto.MinPrice) & builder.Lte(p => p.Price, completePropertyRequestDto.MaxPrice);
            }
            

            if (!string.IsNullOrEmpty(completePropertyRequestDto.NameProperty?.Trim()) || !string.IsNullOrEmpty(completePropertyRequestDto.AddressProperty?.Trim()))
            {
                var nameOrAddressFilter = builder.Empty;

                if (!string.IsNullOrEmpty(completePropertyRequestDto.NameProperty?.Trim()))
                    nameOrAddressFilter &= builder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(completePropertyRequestDto.NameProperty.Trim(), "i"));

                if (!string.IsNullOrEmpty(completePropertyRequestDto.AddressProperty?.Trim()))
                    nameOrAddressFilter &= builder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(completePropertyRequestDto.AddressProperty.Trim(), "i"));

                filter &= nameOrAddressFilter;
            }

            return await _collection.Find(filter).ToListAsync();
        }
        public async Task<bool> ExistPropertyAsyc(PropertyDto propertyDto)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.Name, propertyDto.Name)
                & Builders<Property>.Filter.Eq(p => p.Address, propertyDto.Address)
                & Builders<Property>.Filter.Eq(p => p.CodeInternal, propertyDto.CodeInternal);

            var existOwner = await _collection.Find(filter).FirstOrDefaultAsync();

            return existOwner != null;
        }
        public async Task<bool> ExistPropertyAsyc(string idProperty)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.IdProperty, idProperty);

            var existOwner = await _collection.Find(filter).FirstOrDefaultAsync();

            return existOwner != null;
        }
        public async Task<bool> UpdatePropertyAsync(Property property)
        {
            var filter = Builders<Property>.Filter.Eq(o => o.IdProperty, property.IdProperty);
            var result = await _collection.ReplaceOneAsync(filter, property);
            return result.MatchedCount > 0;
        }
        public async Task<bool> DeletePropertyAsync(string idProperty)
        {
            var filter = Builders<Property>.Filter.Eq(o => o.IdProperty, idProperty);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ExistPropertyCodeInternalAsyc(long codeInternal)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.CodeInternal, codeInternal);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();
            return result != null;
        }
        public async Task<bool> ExistPropertyCodeInternalAsyc(long codeInternal, string idProperty)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.CodeInternal, codeInternal) & Builders<Property>.Filter.Ne(p => p.IdProperty, idProperty);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();
            return result != null;
        }
    }
}

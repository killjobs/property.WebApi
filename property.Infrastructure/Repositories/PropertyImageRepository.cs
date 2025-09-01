using MongoDB.Driver;
using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Repositories;
using property.Infrastructure.Context;

namespace property.Infrastructure.Repositories
{
    public class PropertyImageRepository : IPropertyImageRepository
    {
        private readonly IMongoCollection<PropertyImage> _collection;
        public PropertyImageRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<PropertyImage>("PropertyImages");
        }
        public async Task<PropertyImage> AddPropertyImageAsync(PropertyImage propertyImage)
        {
            await _collection.InsertOneAsync(propertyImage);

            return propertyImage;
        }
        public async Task<IEnumerable<PropertyImage>> GetAllPropertyImagesAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
        public async Task<PropertyImage> GetPropertyImageByIdAsync(string idPropertyImage)
        {
            return await _collection.Find(p => p.IdPropertyImage == idPropertyImage).FirstOrDefaultAsync();
        }
        public async Task<bool> ExistPropertyImageAsyc(PropertyImageDto propertyImageDto)
        {
            var filter = Builders<PropertyImage>.Filter.Eq(pi => pi.File, Convert.FromBase64String(propertyImageDto.File))
                 & Builders<PropertyImage>.Filter.Eq(pi => pi.Enabled, propertyImageDto.Enabled)
                 & Builders<PropertyImage>.Filter.Eq(pi => pi.IdProperty, propertyImageDto.IdProperty);

            var existOwner = await _collection.Find(filter).FirstOrDefaultAsync();

            return existOwner != null;
        }
        public async Task<bool> UpdatePropertyImageAsync(PropertyImage propertyImage)
        {
            var filter = Builders<PropertyImage>.Filter.Eq(pi => pi.IdPropertyImage, propertyImage.IdPropertyImage);
            var result = await _collection.ReplaceOneAsync(filter, propertyImage);
            return result.MatchedCount > 0;
        }
        public async Task<bool> DeletePropertyImageAsync(string idPropertyImage)
        {
            var filter = Builders<PropertyImage>.Filter.Eq(pi => pi.IdPropertyImage, idPropertyImage);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<PropertyImage> GetPropertyImageByIdPropertyAsync(string idProperty)
        {
            var filter = Builders<PropertyImage>.Filter.Eq(pi => pi.IdProperty, idProperty);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}

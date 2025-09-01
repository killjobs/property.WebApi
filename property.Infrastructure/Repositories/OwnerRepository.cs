using MongoDB.Driver;
using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Repositories;
using property.Infrastructure.Context;

namespace property.Infrastructure.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IMongoCollection<Owner> _collection;

        public OwnerRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<Owner>("Owners");
        }
        public async Task<Owner> AddOwnerAsync(Owner owner)
        {
            await _collection.InsertOneAsync(owner);

            return owner;
        }
        public async Task<IEnumerable<Owner>> GetAllOwnersAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Owner> GetOwnerByIdAsync(string idOwner)
        {
            return await _collection.Find(o => o.IdOwner == idOwner).FirstOrDefaultAsync();
        }
        public async Task<bool> ExistOwnerAsyc(OwnerDto ownerDto)
        {
            var filter = Builders<Owner>.Filter.Eq(o => o.Name, ownerDto.Name) & Builders<Owner>.Filter.Eq(o => o.Address, ownerDto.Address);

            var existOwner = await _collection.Find(filter).FirstOrDefaultAsync();

            return existOwner != null;
        }

        public async Task<bool> ExistOwnerAsyc(string idOwner)
        {
            var filter = Builders<Owner>.Filter.Eq(o => o.IdOwner, idOwner);

            var existOwner = await _collection.Find(filter).FirstOrDefaultAsync();

            return existOwner != null;
        }

        public async Task<bool> UpdateOwnerAsync(Owner owner)
        {
            var filter = Builders<Owner>.Filter.Eq(o => o.IdOwner, owner.IdOwner);
            var result = await _collection.ReplaceOneAsync(filter, owner);
            return result.MatchedCount > 0;
        }

        public async Task<bool> DeleteOwnerAsync(string idOwner)
        {
            var filter = Builders<Owner>.Filter.Eq(o => o.IdOwner, idOwner);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}

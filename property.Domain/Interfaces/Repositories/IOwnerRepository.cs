using property.Domain.Dtos;
using property.Domain.Entities;

namespace property.Domain.Interfaces.Repositories
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllOwnersAsync();
        Task<Owner> GetOwnerByIdAsync(string idOwner);
        Task<Owner> AddOwnerAsync(Owner owner);
        Task<bool> ExistOwnerAsyc(OwnerDto ownerDto);
        Task<bool> ExistOwnerAsyc(string idOwner);
        Task<bool> UpdateOwnerAsync(Owner owner);
        Task<bool> DeleteOwnerAsync(string idOwner);
    }
}

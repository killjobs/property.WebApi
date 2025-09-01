using property.Domain.Dtos;
using property.Domain.Entities;

namespace property.Domain.Interfaces.Services
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetAllOwnersAsync();
        Task<Owner> GetOwnerByIdAsync(string id);
        Task<Owner> AddOwnerAsync(OwnerDto ownerDto);
        Task<bool> ExistOwnerAsyc(string idOwner);
        Task<bool> UpdateOwnerAsync(Owner owner);
        Task<bool> DeleteOwnerAsync(string idOwner);
        Task<List<CompletePropertyDto>> GetAllCompleteProperty(CompletePropertyRequestDto completePropertyRequestDto);
    }
}

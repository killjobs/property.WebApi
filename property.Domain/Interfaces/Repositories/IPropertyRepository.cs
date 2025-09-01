using property.Domain.Dtos;
using property.Domain.Entities;

namespace property.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetAllPropertiesAsync();
        Task<Property> GetPropertyByIdAsync(string idProperty);
        Task<Property> AddPropertyAsync(Property property);
        Task<List<Property>> GetPropertyByIdOwnerAsync(string idOwner);
        Task<List<Property>> GetPropertyByPriceAsync(CompletePropertyRequestDto completePropertyRequestDto, string idOwner);
        Task<bool> ExistPropertyAsyc(PropertyDto propertyDto);
        Task<bool> ExistPropertyAsyc(string idProperty);
        Task<bool> ExistPropertyCodeInternalAsyc(long codeInternal, string idProperty);
        Task<bool> ExistPropertyCodeInternalAsyc(long codeInternal);
        Task<bool> UpdatePropertyAsync(Property property);
        Task<bool> DeletePropertyAsync(string idProperty);
    }
}

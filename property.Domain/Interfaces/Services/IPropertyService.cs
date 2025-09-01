using property.Domain.Dtos;
using property.Domain.Entities;

namespace property.Domain.Interfaces.Services
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetAllPropertiesAsync();
        Task<Property> GetPropertyByIdAsync(string id);
        Task<Property> AddPropertyAsync(PropertyDto propertyDto);
        Task<bool> UpdatePropertyAsync(Property property);
        Task<bool> DeletePropertyAsync(string idProperty);
        Task<bool> ExistPropertyAsyc(string idProperty);
    }
}

using property.Domain.Dtos;
using property.Domain.Entities;

namespace property.Domain.Interfaces.Repositories
{
    public interface IPropertyImageRepository
    {
        Task<IEnumerable<PropertyImage>> GetAllPropertyImagesAsync();
        Task<PropertyImage> GetPropertyImageByIdAsync(string idPropertyImage);
        Task<PropertyImage> AddPropertyImageAsync(PropertyImage propertyImage);
        Task<bool> ExistPropertyImageAsyc(PropertyImageDto propertyImageDto);
        Task<bool> UpdatePropertyImageAsync(PropertyImage propertyImage);
        Task<bool> DeletePropertyImageAsync(string idPropertyImage);
        Task<PropertyImage> GetPropertyImageByIdPropertyAsync(string idProperty);
    }
}

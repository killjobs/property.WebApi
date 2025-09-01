using property.Domain.Dtos;
using property.Domain.Entities;

namespace property.Domain.Interfaces.Services
{
    public interface IPropertyImageService
    {
        Task<IEnumerable<PropertyImage>> GetAllPropertyImagesAsync();
        Task<PropertyImage> GetPropertyImageByIdAsync(string id);
        Task<PropertyImage> AddPropertyImageAsync(PropertyImageDto propertyImageDto);
        Task<bool> UpdatePropertyImageAsync(PropertyImage propertyImage);
        Task<bool> DeletePropertyImageAsync(string idPropertyImage);

        Task<PropertyImage> GetPropertyImageByIdPropertyAsync(string idProperty);
    }
}

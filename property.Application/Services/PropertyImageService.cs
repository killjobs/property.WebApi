using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Handlers;
using property.Domain.Interfaces.Repositories;
using property.Domain.Interfaces.Services;

namespace propertyImage.Application.Services
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IPropertyService _propertyService;
        private readonly IImageHandler _imageHandler;

        public PropertyImageService(IPropertyImageRepository propertyImageRepository,
            IPropertyService propertyService, IImageHandler imageHandler)
        {
            _propertyImageRepository = propertyImageRepository;
            _propertyService = propertyService;
            _imageHandler = imageHandler;
        }
        public async Task<PropertyImage> GetPropertyImageByIdAsync(string id)
        {
            return await _propertyImageRepository.GetPropertyImageByIdAsync(id);
        }
        public async Task<IEnumerable<PropertyImage>> GetAllPropertyImagesAsync()
        {
            return await _propertyImageRepository.GetAllPropertyImagesAsync();
        }
        public async Task<PropertyImage> AddPropertyImageAsync(PropertyImageDto propertyImageDto)
        {
            if (await _propertyImageRepository.ExistPropertyImageAsyc(propertyImageDto))
            {
                throw new Exception("Already exists this propertyImage");
            }
            if (!await _propertyService.ExistPropertyAsyc(propertyImageDto.IdProperty))
            {
                throw new Exception($"Does not exists property with id {propertyImageDto.IdProperty}");
            }
            if (!_imageHandler.ValidateImage(propertyImageDto.File))
            {
                throw new Exception("File field is not valid");
            }

            PropertyImage propertyImage = new PropertyImage
            {
                File = Convert.FromBase64String(propertyImageDto.File),
                Enabled = propertyImageDto.Enabled,
                IdProperty = propertyImageDto.IdProperty
            };

            return await _propertyImageRepository.AddPropertyImageAsync(propertyImage);
        }
        public async Task<bool> UpdatePropertyImageAsync(PropertyImage propertyImage)
        {
            var existsProperty = await _propertyImageRepository.GetPropertyImageByIdAsync(propertyImage.IdPropertyImage);
            if (existsProperty == null)
            {
                throw new KeyNotFoundException($"Property with id {propertyImage.IdPropertyImage} does not exist.");
            }

            return await _propertyImageRepository.UpdatePropertyImageAsync(propertyImage);
        }
        public async Task<bool> DeletePropertyImageAsync(string idPropertyImage)
        {
            var existsProperty = await _propertyImageRepository.GetPropertyImageByIdAsync(idPropertyImage);
            if (existsProperty == null)
            {
                throw new KeyNotFoundException($"Property with id {idPropertyImage} does not exist.");
            }

            return await _propertyImageRepository.DeletePropertyImageAsync(idPropertyImage);
        }

        public async Task<PropertyImage> GetPropertyImageByIdPropertyAsync(string idProperty)
        {
            return await _propertyImageRepository.GetPropertyImageByIdPropertyAsync(idProperty);
        }
    }
}

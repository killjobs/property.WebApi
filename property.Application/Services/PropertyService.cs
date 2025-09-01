using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Repositories;
using property.Domain.Interfaces.Services;
using System.Diagnostics;

namespace property.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IOwnerService _ownerService;

        public PropertyService(IPropertyRepository propertyRepository, IPropertyImageRepository propertyImageRepository, IOwnerService ownerService)
        {
            _propertyRepository = propertyRepository;
            _propertyImageRepository = propertyImageRepository;
            _ownerService = ownerService;
        }
        public async Task<Property> GetPropertyByIdAsync(string id)
        {
            return await _propertyRepository.GetPropertyByIdAsync(id);
        }
        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            return await _propertyRepository.GetAllPropertiesAsync();
        }
        public async Task<Property> AddPropertyAsync(PropertyDto propertyDto)
        {
            if (await _propertyRepository.ExistPropertyAsyc(propertyDto))
            {
                throw new Exception("Already exists this property");
            }
            if (await _propertyRepository.ExistPropertyCodeInternalAsyc(propertyDto.CodeInternal))
            {
                throw new Exception($"Already exists a property with this code internal: {propertyDto.CodeInternal}");
            }
                if (!await _ownerService.ExistOwnerAsyc(propertyDto.IdOwner))
            {
                throw new Exception($"Does not exist Owner with id: {propertyDto.IdOwner}");
            }

            Property property = new Property
            {
                Name = propertyDto.Name,
                Address = propertyDto.Address,
                Price = propertyDto.Price,
                CodeInternal = propertyDto.CodeInternal,
                Year = propertyDto.Year,
                IdOwner = propertyDto.IdOwner
            };

            return await _propertyRepository.AddPropertyAsync(property);
        }
        public async Task<bool> UpdatePropertyAsync(Property property)
        {
            var existsProperty = await _propertyRepository.GetPropertyByIdAsync(property.IdProperty);
            if (existsProperty == null)
            {
                throw new KeyNotFoundException($"Property with id {property.IdProperty} does not exist.");
            }
            if (await _propertyRepository.ExistPropertyCodeInternalAsyc(property.CodeInternal, property.IdProperty))
            {
                throw new Exception($"Already exists a property with this code internal: {property.CodeInternal}");
            }
            if (!await _ownerService.ExistOwnerAsyc(property.IdOwner))
            {
                throw new Exception($"Does not exist Owner with id: {property.IdOwner}");
            }

            return await _propertyRepository.UpdatePropertyAsync(property);
        }
        public async Task<bool> DeletePropertyAsync(string idProperty)
        {
            var existsProperty = await _propertyRepository.GetPropertyByIdAsync(idProperty);
            if (existsProperty == null)
            {
                throw new KeyNotFoundException($"Property with id {idProperty} does not exist.");
            }

            return await _propertyRepository.DeletePropertyAsync(idProperty);
        }

        public async Task<bool> ExistPropertyAsyc(string idProperty)
        {
             return await _propertyRepository.ExistPropertyAsyc(idProperty);
        }
    }
}

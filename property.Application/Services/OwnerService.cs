using Microsoft.VisualBasic;
using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Handlers;
using property.Domain.Interfaces.Repositories;
using property.Domain.Interfaces.Services;

namespace property.Application.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IImageHandler _imageHandler;

        public OwnerService(IOwnerRepository ownerRepository, IPropertyRepository propertyRepository, 
            IPropertyImageRepository propertyImageRepository, IImageHandler imageHandler)
        {
            _ownerRepository = ownerRepository;
            _propertyRepository = propertyRepository;
            _propertyImageRepository = propertyImageRepository;
            _imageHandler = imageHandler;
        }

        public async Task<Owner> GetOwnerByIdAsync(string id)
        {
            return await _ownerRepository.GetOwnerByIdAsync(id);
        }

        public async Task<IEnumerable<Owner>> GetAllOwnersAsync()
        {
            return await _ownerRepository.GetAllOwnersAsync();
        }

        public async Task<Owner> AddOwnerAsync(OwnerDto ownerDto)
        {
            if (await _ownerRepository.ExistOwnerAsyc(ownerDto))
            {
                throw new Exception("Already exists this owner");
            }
            if (!_imageHandler.ValidateImage(ownerDto.Photo))
            {
                throw new Exception("Photo field is not valid");
            }

            Owner owner = new Owner
            {
                Name = ownerDto.Name,
                Address = ownerDto.Address,
                Photo = Convert.FromBase64String(ownerDto.Photo),
                Birthday = ownerDto.Birthday
            };

            return await _ownerRepository.AddOwnerAsync(owner);
        }

        public async Task<bool> UpdateOwnerAsync(Owner owner)
        {
            var existsOwner = await _ownerRepository.GetOwnerByIdAsync(owner.IdOwner);
            if(existsOwner == null)
            {
                throw new KeyNotFoundException($"Owner with id {owner.IdOwner} does not exist.");
            }

            return await _ownerRepository.UpdateOwnerAsync(owner);
        }

        public async Task<bool> DeleteOwnerAsync(string idOwner)
        {
            var existsOwner = await _ownerRepository.GetOwnerByIdAsync(idOwner);
            if (existsOwner == null)
            {
                throw new KeyNotFoundException($"Owner with id {idOwner} does not exist.");
            }

            return await _ownerRepository.DeleteOwnerAsync(idOwner);
        }

        public async Task<bool> ExistOwnerAsyc(string idOwner)
        {
            return await _ownerRepository.ExistOwnerAsyc(idOwner);
        }
        public async Task<List<CompletePropertyDto>> GetAllCompleteProperty(CompletePropertyRequestDto completePropertyRequestDto)
        {
            var result = new List<CompletePropertyDto>();
            var owners = await _ownerRepository.GetAllOwnersAsync();
            foreach (var owner in owners)
            {
                CompletePropertyDto completedProperty = new CompletePropertyDto();
                List<ListPropertiesDto> listProperties = new List<ListPropertiesDto>();
                completedProperty.IdOwner = owner.IdOwner;
                completedProperty.NameOwner = owner.Name;

                var properties = await _propertyRepository.GetPropertyByPriceAsync(completePropertyRequestDto, owner.IdOwner);

                foreach (var property in properties)
                {
                    var propertyImage = await _propertyImageRepository.GetPropertyImageByIdPropertyAsync(property.IdProperty);
                    var propertyImageFile = new byte[0];
                    var propertyImageId = string.Empty;
                    if (propertyImage != null)
                    {
                        propertyImageFile = propertyImage.File;
                        propertyImageId = propertyImage.IdPropertyImage;
                    }
                    ListPropertiesDto propertyData = new ListPropertiesDto()
                    {
                        IdProperty = property.IdProperty,
                        IdPropertyImage = propertyImageId,
                        NameProperty = property.Name,
                        AddressProperty = property.Address,
                        Price = property.Price,
                        File = propertyImageFile,
                        CodeInternal = property.CodeInternal
                    };
                    listProperties.Add(propertyData);
                }
                completedProperty.ListProperties = listProperties;
                if(completedProperty.ListProperties.Count() > 0)
                {
                    result.Add(completedProperty);
                }
            }
            return result;
        }

    }
}

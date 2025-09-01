using Moq;
using property.Application.Services;
using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Handlers;
using property.Domain.Interfaces.Repositories;
using property.Domain.Interfaces.Services;
using property.Tests.Builders;

namespace property.Tests.Services
{
    public class OwnerServiceTest
    {
        private Mock<IOwnerRepository> _mockRepo;
        private Mock<IPropertyRepository> _mockPropertyRepo;
        private Mock<IPropertyImageRepository> _mockPropertyImageRepo;
        private Mock<IImageHandler> _mockImageHandler;
        private IOwnerService _ownerService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IOwnerRepository>();
            _mockPropertyRepo = new Mock<IPropertyRepository>();
            _mockPropertyImageRepo = new Mock<IPropertyImageRepository>();
            _mockImageHandler = new Mock<IImageHandler>();
            _ownerService = new OwnerService(_mockRepo.Object, _mockPropertyRepo.Object, _mockPropertyImageRepo.Object, _mockImageHandler.Object);
        }

        [Test]
        public async Task AddOwnerWhenOwnerIsNewReturnOwner()
        {
            var idOwner = Guid.NewGuid().ToString();
            var ownerDto = new OwnerDtoBuilder().Build();
            
            var owner = new OwnerBuilder()
                .WithIdOwner(idOwner)
                .Build();

            _mockRepo.Setup(r => r.ExistOwnerAsyc(It.IsAny<OwnerDto>())).ReturnsAsync(false);
            _mockRepo.Setup(r => r.AddOwnerAsync(It.IsAny<Owner>())).ReturnsAsync(owner);

            var result = await _ownerService.AddOwnerAsync(ownerDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Julian"));
            Assert.That(result.IdOwner, Is.EqualTo(idOwner));

            _mockRepo.Verify(r => r.ExistOwnerAsyc(It.IsAny<OwnerDto>()), Times.Once);
            _mockRepo.Verify(r => r.AddOwnerAsync(It.IsAny<Owner>()), Times.Once);
        }

        [Test]
        public async Task AddOwnerWhenOwnerAlreadyExistsReturnThrowExcpetion()
        {
            var ownerDto = new OwnerDtoBuilder().Build();

            _mockRepo.Setup(r => r.ExistOwnerAsyc(It.IsAny<OwnerDto>())).ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<Exception>(
                async () => await _ownerService.AddOwnerAsync(ownerDto)
            );

            Assert.That(exception.Message, Is.EqualTo("Already exists this owner"));
            _mockRepo.Verify(r => r.AddOwnerAsync(It.IsAny<Owner>()), Times.Never);
        }
        [Test]
        public async Task UpdateOwnerWhenOwnerAlreadyExistsReturnTrue()
        {
            var idOwner = Guid.NewGuid().ToString();
            var ownerDto = new OwnerDtoBuilder().Build();
            var owner = new OwnerBuilder()
                .WithIdOwner(idOwner)
                .Build();

            _mockRepo.Setup(r => r.GetOwnerByIdAsync(idOwner)).ReturnsAsync(owner);

            _mockRepo.Setup(r => r.UpdateOwnerAsync(It.IsAny<Owner>())).ReturnsAsync(true);

            var result = await _ownerService.UpdateOwnerAsync(owner);

            Assert.That(result, Is.True);
            _mockRepo.Verify(r => r.GetOwnerByIdAsync(idOwner), Times.Once);
            _mockRepo.Verify(r => r.UpdateOwnerAsync(It.IsAny<Owner>()), Times.Once);
        }
        [Test]
        public void UpdateOwnerWhenOwnerDoesNotExistThrowException()
        {
            var idOwner = Guid.NewGuid().ToString();
            var owner = new OwnerBuilder()
                .WithIdOwner(idOwner)
                .Build();

            _mockRepo.Setup(r => r.GetOwnerByIdAsync(idOwner)).ReturnsAsync((Owner?)null);

            var exception = Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _ownerService.UpdateOwnerAsync(owner)
            );

            Assert.That(exception.Message, Is.EqualTo($"Owner with id {idOwner} does not exist."));
            _mockRepo.Verify(r => r.UpdateOwnerAsync(It.IsAny<Owner>()), Times.Never);
        }
        [Test]
        public async Task DeleteOwnerWhenOwnerExistsReturnTrue()
        {
            var idOwner = Guid.NewGuid().ToString();
            var owner = new OwnerBuilder()
                .WithIdOwner(idOwner)
                .Build();

            _mockRepo.Setup(r => r.GetOwnerByIdAsync(idOwner)).ReturnsAsync(owner);
            _mockRepo.Setup(r => r.DeleteOwnerAsync(idOwner)).ReturnsAsync(true);

            var result = await _ownerService.DeleteOwnerAsync(idOwner);

            Assert.That(result, Is.True);
            _mockRepo.Verify(r => r.GetOwnerByIdAsync(idOwner), Times.Once);
            _mockRepo.Verify(r => r.DeleteOwnerAsync(idOwner), Times.Once);
        }
        [Test]
        public void DeleteOwnerWhenOwnerDoesNotExistReturnThrowException()
        {
            var idOwner = Guid.NewGuid().ToString();
            _mockRepo.Setup(r => r.GetOwnerByIdAsync(idOwner)).ReturnsAsync((Owner?)null);

            var exception = Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _ownerService.DeleteOwnerAsync(idOwner)
            );

            Assert.That(exception.Message, Is.EqualTo($"Owner with id {idOwner} does not exist."));
            _mockRepo.Verify(r => r.GetOwnerByIdAsync(idOwner), Times.Once);
            _mockRepo.Verify(r => r.DeleteOwnerAsync(It.IsAny<string>()), Times.Never);
        }
        [Test]
        public async Task GetAllCompletePropertyWithFilterReturnsMatchingProperties()
        {
            var idOwner = Guid.NewGuid().ToString();
            var idProperty = Guid.NewGuid().ToString();

            var owners = new List<Owner>
            {
                new OwnerBuilder().WithIdOwner(idOwner).Build()
            };

            var properties = new List<Property>
            {
                new Builders.PropertyBuilder().WithIdProperty(idProperty).Build()
            };

            _mockRepo.Setup(r => r.GetAllOwnersAsync()).ReturnsAsync(owners);
            _mockPropertyRepo.Setup(r => r.GetPropertyByPriceAsync(It.IsAny<CompletePropertyRequestDto>(), idOwner))
                             .ReturnsAsync(properties);
            _mockPropertyImageRepo.Setup(r => r.GetPropertyImageByIdPropertyAsync(idProperty)).ReturnsAsync((PropertyImage?)null);

            var filterDto = new CompletePropertyRequestDto
            {
                MinPrice = 0,
                MaxPrice = 250000,
                NameProperty = "Building",
                AddressProperty = "Street"
            };

            var result = await _ownerService.GetAllCompleteProperty(filterDto);

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().ListProperties.Count, Is.EqualTo(properties.Count()));
            Assert.That(result.First().ListProperties.First().NameProperty, Is.EqualTo(properties.First().Name));
            Assert.That(result.First().ListProperties.First().File, Is.Empty);
        }
        [Test]
        public async Task GetAllCompletePropertyWithFilterReturnsEmpty()
        {
            var idOwner = Guid.NewGuid().ToString();
            var owners = new List<Owner> { 
                new OwnerBuilder().WithIdOwner(idOwner).Build()
            };

            _mockRepo.Setup(r => r.GetAllOwnersAsync()).ReturnsAsync(owners);
            _mockPropertyRepo.Setup(r => r.GetPropertyByPriceAsync(It.IsAny<CompletePropertyRequestDto>(), idOwner))
                             .ReturnsAsync(new List<Property>());

            var filterDto = new CompletePropertyRequestDto { MinPrice = 550000, MaxPrice = 650000 };

            var result = await _ownerService.GetAllCompleteProperty(filterDto);

            Assert.That(result, Is.Empty);
        }
    }
}
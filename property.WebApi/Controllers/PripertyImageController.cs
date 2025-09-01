using Microsoft.AspNetCore.Mvc;
using property.Application.Services;
using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Services;

namespace property.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PripertyImageController : ControllerBase
    {
        private readonly IPropertyImageService _propertyImageService;

        public PripertyImageController(IPropertyImageService propertyImageService)
        {
            _propertyImageService = propertyImageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPropertyImages()
        {
            var owners = await _propertyImageService.GetAllPropertyImagesAsync();
            return Ok(owners);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyImage>> GetPropertyImageById(string id)
        {
            var owner = await _propertyImageService.GetPropertyImageByIdAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            return Ok(owner);
        }

        [HttpPost]
        public async Task<IActionResult> AddPropertyImage([FromBody] PropertyImageDto ownerDto)
        {
            try
            {
                PropertyImage owner = await _propertyImageService.AddPropertyImageAsync(ownerDto);
                return CreatedAtAction(nameof(GetPropertyImageById), new { id = owner.IdPropertyImage }, owner);
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePropertyImage([FromBody] PropertyImage owner)
        {
            var updatedPropertyImage = await _propertyImageService.UpdatePropertyImageAsync(owner);
            if (!updatedPropertyImage)
            {
                return NotFound(updatedPropertyImage);
            }
            return Ok(updatedPropertyImage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropertyImage(string id)
        {
            try
            {
                var deletedPropertyImage = await _propertyImageService.DeletePropertyImageAsync(id);
                return Ok(deletedPropertyImage);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }
    }
}

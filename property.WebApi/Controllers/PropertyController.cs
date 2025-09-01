using Microsoft.AspNetCore.Mvc;
using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Services;

namespace property.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProperties()
        {
            var propertys = await _propertyService.GetAllPropertiesAsync();
            return Ok(propertys);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetPropertyById(string id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            return Ok(property);
        }

        [HttpPost]
        public async Task<IActionResult> AddProperty([FromBody] PropertyDto propertyDto)
        {
            try
            {
                Property property = await _propertyService.AddPropertyAsync(propertyDto);
                return CreatedAtAction(nameof(GetPropertyById), new { id = property.IdProperty }, property);
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProperty([FromBody] Property property)
        {
            var updatedProperty = await _propertyService.UpdatePropertyAsync(property);
            if (!updatedProperty)
            {
                return NotFound(updatedProperty);
            }
            return Ok(updatedProperty);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(string id)
        {
            try
            {
                var deletedProperty = await _propertyService.DeletePropertyAsync(id);
                return Ok(deletedProperty);
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

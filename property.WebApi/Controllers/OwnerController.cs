using Microsoft.AspNetCore.Mvc;
using property.Application.Services;
using property.Domain.Dtos;
using property.Domain.Entities;
using property.Domain.Interfaces.Services;

namespace property.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            var owners = await _ownerService.GetAllOwnersAsync();
            return Ok(owners);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Owner>> GetOwnerById(string id)
        {
            var owner = await _ownerService.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            return Ok(owner);
        }

        [HttpGet]
        [Route("CompleteProperty")]
        public async Task<ActionResult<CompletePropertyDto>> GetAllCompleteProperty([FromQuery] CompletePropertyRequestDto completePropertyRequestDto)
        {
            try
            {
                var completeProperties = await _ownerService.GetAllCompleteProperty(completePropertyRequestDto);
                return Ok(completeProperties);
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }           
        }

        [HttpPost]
        public async Task<IActionResult> AddOwner([FromBody] OwnerDto ownerDto)
        {
            try
            {
                Owner owner = await _ownerService.AddOwnerAsync(ownerDto);
                return CreatedAtAction(nameof(GetOwnerById), new { id = owner.IdOwner }, owner);
            }
            catch(Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOwner([FromBody] Owner owner)
        {
            var updatedOwner = await _ownerService.UpdateOwnerAsync(owner);
            if (!updatedOwner)
            {
                return NotFound(updatedOwner);
            }
            return Ok(updatedOwner);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(string id)
        {
            try
            {
                var deletedOwner = await _ownerService.DeleteOwnerAsync(id);
                return Ok(deletedOwner);
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

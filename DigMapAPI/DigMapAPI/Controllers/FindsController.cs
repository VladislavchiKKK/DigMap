using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DigMap.Application.Interfaces.Services;
using DigMap.Application.DTOs.Finds;

namespace DigMap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FindsController : ControllerBase
    {
        private readonly IFindItemService _service;

        public FindsController(IFindItemService service)
        {
            _service = service;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<ActionResult<List<FindItemDto>>> GetAll()
        {
            var items = await _service.GetAllUserItemsAsync(GetUserId());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FindItemDto>> GetOne(int id)
        {
            var item = await _service.GetItemByIdAsync(id, GetUserId());

            if (item == null) return NotFound("Знахідку не знайдено або ви не є власником.");

            return Ok(item);
        }

        [HttpPost("coin")]
        public async Task<ActionResult<FindItemDto>> AddCoin([FromBody] CreateCoinDto dto)
        {
            var createdCoin = await _service.CreateCoinAsync(dto, GetUserId());
            return Ok(createdCoin);
        }

        [HttpPost("artifact")]
        public async Task<ActionResult<FindItemDto>> AddArtifact([FromBody] CreateArtifactDto dto)
        {
            var createdArtifact = await _service.CreateArtifactAsync(dto, GetUserId());
            return Ok(createdArtifact);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteItemAsync(id, GetUserId());
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VenderTest.DTOs;
using VenderTest.Models;
using VenderTest.Service;

namespace VenderTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(Item item)
        {
            try
            {
                var result = await _itemService.AddItem(item);

                if (result.Status > 0)
                {
                    return Ok(result);
                }

                return BadRequest(new ApiResponseDto
                {
                    Status = result.Status,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto
                {
                    Status = 0,
                    Message = $"Controller error while adding item: {ex.Message}"
                });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(Item item)
        {
            try
            {
                var result = await _itemService.UpdateItem(item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto
                {
                    Status = 0,
                    Message = $"Controller error while updating item: {ex.Message}"
                });
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchItemCode,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var items = await _itemService.GetAllItems(searchItemCode, pageNumber, pageSize);
                var totalCount = items.Any() ? items.First().TotalCount : 0;

                return Ok(new
                {
                    Status = 1,
                    Message = "Success",
                    TotalCount = totalCount,
                    Data = items
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Message = $"Controller error while fetching items: {ex.Message}",
                    TotalCount = 0,
                    Data = new List<ItemDto>()
                });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _itemService.DeleteItem(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto
                {
                    Status = 0,
                    Message = $"Controller error while deleting item: {ex.Message}"
                });
            }
        }
    }
}
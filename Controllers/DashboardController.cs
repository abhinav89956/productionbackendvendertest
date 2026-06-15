using Microsoft.AspNetCore.Mvc;
using VenderTest.Service;

namespace VenderTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("GetVendor/{userId:int}")]
        public async Task<IActionResult> GetVendor(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new { message = "Invalid User Id" });
            }

            try
            {
                var result = await _service.GetVendorDetails(userId);

                if (result == null)
                {
                    return NotFound(new { message = "Vendor not found" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal Server Error",
                    error = ex.Message
                });
            }
        }
    }
}
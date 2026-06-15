using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VenderTest.DTOs;
using VenderTest.Models;
using VenderTest.Service;
using YourProject.Models;

namespace VenderTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet("GetAllVenders")]
        public async Task<IActionResult> GetAllVenders(string? searchVenderCode, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _vendorService.GetAllVenders(searchVenderCode, pageNumber, pageSize);

                return Ok(new
                {
                    Status = 1,
                    Message = "Success",
                    TotalCount = result.FirstOrDefault()?.TotalCount ?? 0,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Message = $"Controller Error: {ex.Message}"
                });
            }
        }

        [HttpPost("AddVendor")]
        public async Task<IActionResult> AddVendor([FromBody] Vendor vendor)
        {
            try
            {
                if (vendor == null)
                {
                    return BadRequest(new
                    {
                        Status = 0,
                        Message = "Vendor data is null"
                    });
                }

                var response = await _vendorService.AddVendor(vendor);

                if (response.Status == 1)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Message = $"Controller Error: {ex.Message}"
                });
            }
        }

        [HttpPut("UpdateVendor")]
        public async Task<IActionResult> UpdateVendor([FromBody] Vendor vendor)
        {
            try
            {
                if (vendor == null || vendor.VenderId <= 0)
                {
                    return BadRequest(new
                    {
                        Status = 0,
                        Message = "Invalid vendor data"
                    });
                }

                var response = await _vendorService.UpdateVendor(vendor);

                if (response.Status == 1)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Message = $"Controller Error: {ex.Message}"
                });
            }
        }

        [HttpDelete("DeleteVendor/{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        Status = 0,
                        Message = "Invalid vendor ID"
                    });
                }

                var vendor = new Vendor { VenderId = id };
                var response = await _vendorService.DeleteVender(vendor);

                if (response.Status == 1)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Message = $"Controller Error: {ex.Message}"
                });
            }
        }

        [HttpPost("AssignItems")]
        public async Task<IActionResult> AssignItems([FromBody] VenderAsignDto model)
        {
            try
            {
                var response = await _vendorService.AsignItems(model.ItemCode, model.VenderCode);

                if (response.Status == 1)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Message = $"Controller Error: {ex.Message}"
                });
            }
        }

        [HttpPost("UnAssignItems")]
        public async Task<IActionResult> UnAssignItems([FromBody] VenderAsignDto model)
        {
            try
            {
                var response = await _vendorService.UnAsignItems(model.ItemCode, model.VenderCode);

                if (response.Status == 1)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 0,
                    Message = $"Controller Error: {ex.Message}"
                });
            }
        }
    }
}
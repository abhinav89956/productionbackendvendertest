using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VenderTest.DTOs;
using VenderTest.Services;

namespace VenderTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BarCodeController : ControllerBase
    {
        private readonly IBarCodeService _service;

        public BarCodeController(IBarCodeService service)
        {
            _service = service;
        }

        [HttpGet("GetBarcodes")]
        public async Task<IActionResult> GetBarcodes()
        {
            try
            {
                var result = await _service.GetVenderBarcodes();

                if (result == null || !(result is IEnumerable<BarCodeDto> list) || !list.Any())
                {
                    var emptyList = new List<BarCodeDto>
                    {
                        new BarCodeDto { Status = 0, Message = "No barcodes found" }
                    };

                    return Ok(emptyList);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<BarCodeDto>
                {
                    new BarCodeDto { Status = -1, Message = $"Error: {ex.Message}" }
                };

                return Ok(errorList);
            }
        }

        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateBarcode([FromBody] VenderItemsDto request)
        {
            try
            {
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.VenderCode) ||
                    string.IsNullOrWhiteSpace(request.ItemCode))
                {
                    return BadRequest("VenderCode and ItemCode are required.");
                }

                var result = await _service.InsertVenderItemBarcodeAsync(
                    request.VenderCode,
                    request.ItemCode,
                    request.ManufacturingDate,
                    request.ExpiryDate,
                    request.VarCode
                );

                int statusCode = (result as dynamic)?.Status ?? -1;
                string message = (result as dynamic)?.Message ?? "Unknown error";

                if (statusCode <= 0)
                    return BadRequest(message);

                return Ok(new
                {
                    Status = statusCode,
                    VarCode = (result as dynamic)?.VarCode,
                    BarcodeBase64 = (result as dynamic)?.barcodeBase64,
                    PdfBase64 = (result as dynamic)?.pdfBase64
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = -1,
                    Message = $"An error occurred while generating the barcode: {ex.Message}"
                });
            }
        }

        [HttpDelete("{barcodeId}")]
        public async Task<IActionResult> DeleteBarcode(int barcodeId)
        {
            try
            {
                var result = await _service.DeleteBarcode(barcodeId);

                int statusCode = (result as dynamic)?.Status ?? -1;
                string message = (result as dynamic)?.Message ?? "Unknown error";

                if (statusCode <= 0)
                    return BadRequest(message);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = -1,
                    Message = $"An error occurred while deleting the barcode: {ex.Message}"
                });
            }
        }

        [HttpGet("GetVendorItems")]
        public async Task<IActionResult> GetVendorItems(string venderCode)
        {
            try
            {
                var result = await _service.GetItemsByVenderCode(venderCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<VenderItemsDto>
                {
                    new VenderItemsDto { Status = 0, Message = $"Error: {ex.Message}" }
                };

                return Ok(errorList);
            }
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VenderTest.Models;
using VenderTest.Service;

namespace VenderTest.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet("getst")]
        public async Task<IActionResult> GetSettings()
        {
            try
            {
                var data = await _settingService.GetSettings();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new Setting
                {
                    Status = 0,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("updatest")]
        public async Task<IActionResult> UpdateSettings([FromBody] Setting model)
        {
            try
            {
                if (model == null)
                {
                    return Ok(new Setting
                    {
                        Status = 0,
                        Message = "Invalid data."
                    });
                }

                var response = await _settingService.UpdateSettings(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new Setting
                {
                    Status = 0,
                    Message = ex.Message
                });
            }
        }
    }
}
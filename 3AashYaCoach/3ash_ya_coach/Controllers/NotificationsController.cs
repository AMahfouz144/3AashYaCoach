using _3AashYaCoach._3ash_ya_coach.Dtos;
using _3AashYaCoach._3ash_ya_coach.Models;
using _3AashYaCoach._3ash_ya_coach.Services.DeviceTokenService;
using _3AashYaCoach.Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3AashYaCoach._3ash_ya_coach.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IDeviceTokenService _deviceTokenService;

        public NotificationsController(IDeviceTokenService deviceTokenService)
        {
            _deviceTokenService = deviceTokenService;
        }

        [HttpPost("register-token")]
        public async Task<IActionResult> RegisterToken([FromBody] DeviceTokenDto dto)
        {
            await _deviceTokenService.SaveOrUpdateTokenAsync(dto.UserId, dto.Token);
            return Ok(new { message = "Token saved successfully." });
        }
    }


}

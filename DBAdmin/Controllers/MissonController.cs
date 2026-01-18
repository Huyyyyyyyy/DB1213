using DBAdmin.DTOs.Requests;
using DBAdmin.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DBAdmin.Controllers
{
    [ApiController]
    [Route("api/mission")]
    public class MissionController : ControllerBase
    {
        private readonly IMission _service;

        public MissionController(IMission service)
        {
            _service = service;
        }

        [HttpPost("claim")]
        public async Task<IActionResult> DailyClaim(DailyClaimRequest request)
        {
            var result = await _service.DailyClaimAsync(request);
            return Ok(result);
        }
    }
}

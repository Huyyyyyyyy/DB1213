using DBAdmin.DTOs.Requests;
using DBAdmin.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DBAdmin.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUser _service;

        public UserController(IUser service)
        {
            _service = service;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(GetUserRequest request)
        {
            return Ok(await _service.GetUserAsync(request));
        }
    }
}

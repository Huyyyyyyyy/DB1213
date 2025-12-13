using DBApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DBApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class GeneralController : ControllerBase
    {
        private readonly GeneralService _service;
        public GeneralController(GeneralService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("No file uploaded.");
            var result = await _service.SaveFile(file);
            return Ok(new { path = result });
        }
    }   
}

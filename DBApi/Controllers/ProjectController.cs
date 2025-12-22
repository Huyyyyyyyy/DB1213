using DBApi.DTOs.Requests;
using DBApi.DTOs.Responses;
using DBApi.Services;
using DBDatabase.Entities.Media;
using DBUtils;
using Microsoft.AspNetCore.Mvc;

namespace DBApi.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _service;

        public ProjectController(ProjectService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<AddProjectResponse>> Add([FromForm] AddProjectRequest request)
        {

            var rs = await _service.AddProject(request.proj_name, request.proj_type, request.proj_img, request.proj_official_site, request.proj_other_site);
            return Ok(rs);
        }
    }
}

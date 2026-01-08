using DBApi.DTOs.Requests;
using DBApi.DTOs.Responses;
using DBApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DBApi.Controllers
{
    [ApiController]
    [Route("api/contributor")]
    public class ContributorController : ControllerBase
    {
        private readonly ContributorService _service;

        public ContributorController(ContributorService service)
        {
            _service = service;
        }

        [HttpGet("get")]
        public async Task<ActionResult<GetContributorResponse>> Get([FromQuery] GetContributorRequest request)
        {
            if (request.page < 1) request.page = 1;
            if (request.limit < 1) request.limit = 10;
            if (request.limit > 50) request.limit = 50;
            var rs = await _service.GetContributors(request.page, request.limit);
            return Ok(rs);
        }

        [HttpPost("add")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<AddContributorResponse>> Add([FromForm] AddContributorRequest request)
        {

            var rs = await _service.AddContributor(
                request.cont_username, 
                request.cont_nickname, 
                request.cont_x_link, 
                request.cont_image_url, 
                request.cont_note, 
                request.cont_wallet_address);
            return Ok(rs);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SoftDeleteContributorResponse>> SoftDelete([FromRoute] SoftDeleteContributorRequest request)
        {
            if (request?.cont_id == null || request.cont_id.Equals(Guid.Empty)) return BadRequest();
            var rs = await _service.SoftDeleteContributor(request.cont_id);
            if (rs.code == 404) return NotFound(rs);
            return Ok(rs);
        }
    }
}

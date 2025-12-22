using DBApi.DTOs.Requests;
using DBApi.DTOs.Responses;
using DBApi.Services;
using DBDatabase.Entities.Media;
using DBUtils;
using Microsoft.AspNetCore.Mvc;

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
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<UploadResponse>> Upload([FromForm] UploadRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.content)) return BadRequest(new { message = "no content" });
        if (request.file == null || request.file.Length == 0) return BadRequest(new { message = "no file detected" });
        if (MediaRow.toFileType(request.file.ContentType) == null) return BadRequest(new { message = "type is not allowed" });
        if (request.file.Length > Const.MAX_FILE_LENGTH) return BadRequest(new { message = "maximum is 10MB" });
        var rs = await _service.SaveFile(request.file, request.content);
        return Ok(rs);
    }

    [HttpGet("posts")]
    public async Task<ActionResult<GetPostResponse>> GetPost([FromQuery] GetPostRequest request)
    {
        if (request.page < 1) request.page = 1;
        if (request.limit < 1) request.limit = 10;
        if (request.limit > 50) request.limit = 50;
        var rs = await _service.GetPosts(request.page, request.limit);
        return Ok(rs);
    }

    [HttpGet("posts/{id}")]
    public async Task<ActionResult<GetPostByIdResponse>> GetPostById([FromRoute] GetPostByIdRequest request)
    {
        if (request?.id == null || request.id.Equals(Guid.Empty)) return BadRequest();
        var rs = await _service.GetPostById(request.id);
        if (rs.code == 404) return NotFound(rs);
        return Ok(rs);
    }

    [HttpDelete("posts/{id}")]
    public async Task<ActionResult<DeletePostResponse>> DeletePost([FromRoute] DeletePostRequest request)
    {
        if (request?.id == null || request.id.Equals(Guid.Empty)) return BadRequest();
        var rs = await _service.DeletePost(request.id);
        if (rs.code == 404) return NotFound(rs);
        return Ok(rs);
    }

    [HttpPut("posts/{id}")]
    public async Task<ActionResult<UploadResponse>> UpdatePost(
        [FromBody, FromRoute] UpdatePostRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.content))
            return BadRequest(new { message = "content is required" });

        var rs = await _service.UpdatePost(request.id, request.content);
        if (rs.code == 404) return NotFound(rs);
        return Ok(rs);
    }
}

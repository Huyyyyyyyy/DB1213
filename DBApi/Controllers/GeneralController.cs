using DBApi.DTOs.Requests;
using DBApi.DTOs.Responses;
using DBApi.Services;
using DBDatabase.Entities;
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
    public async Task<ActionResult<UploadResponse>> UploadFile(
        [FromForm] UploadRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.content)) return BadRequest(new { message = "no content" });
        if (request.file == null || request.file.Length == 0) return BadRequest(new { message = "no file detected" });
        if (MediaRow.toFileType(request.file.ContentType) == null) return BadRequest(new { message = "type is not allowed" });
        if (request.file.Length > Const.MAX_FILE_LENGTH) return BadRequest(new { message = "maximum is 10MB" });
        var rs = await _service.SaveFile(request.file, request.content);
        return Ok(rs);
    }
}

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

    // ========== ADMIN: Đăng bài mới ==========
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

    // ========== WEB UI: Lấy danh sách bài đăng ==========
    [HttpGet("posts")]
    public async Task<ActionResult<PostListResponse>> GetPosts(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        if (page < 1) page = 1;
        if (limit < 1) limit = 10;
        if (limit > 50) limit = 50; // Giới hạn tối đa

        var rs = await _service.GetPosts(page, limit);
        return Ok(rs);
    }

    // ========== WEB UI: Lấy chi tiết 1 bài đăng ==========
    [HttpGet("posts/{id}")]
    public async Task<ActionResult<UploadResponse>> GetPostById(Guid id)
    {
        var rs = await _service.GetPostById(id);
        if (rs.code == 404) return NotFound(rs);
        return Ok(rs);
    }

    // ========== ADMIN: Xóa bài đăng ==========
    [HttpDelete("posts/{id}")]
    public async Task<ActionResult<UploadResponse>> DeletePost(Guid id)
    {
        var rs = await _service.DeletePost(id);
        if (rs.code == 404) return NotFound(rs);
        return Ok(rs);
    }

    // ========== ADMIN: Sửa bài đăng ==========
    [HttpPut("posts/{id}")]
    public async Task<ActionResult<UploadResponse>> UpdatePost(
        Guid id, 
        [FromBody] UpdatePostRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.content))
            return BadRequest(new { message = "content is required" });

        var rs = await _service.UpdatePost(id, request.content);
        if (rs.code == 404) return NotFound(rs);
        return Ok(rs);
    }
}

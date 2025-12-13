using Microsoft.AspNetCore.Mvc;

namespace DBApi.DTOs.Requests
{
    public class UploadRequest
    {
        [FromForm]
        public string content { get; set; } = string.Empty;
        public required IFormFile file { get; set; }
    }
}

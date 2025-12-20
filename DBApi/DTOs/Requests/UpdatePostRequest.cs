using Microsoft.AspNetCore.Mvc;

namespace DBApi.DTOs.Requests
{
    public class UpdatePostRequest
    {
        [FromBody]
        public string? content { get; set; }
        [FromRoute]
        public Guid id { get; set; }
    }
}

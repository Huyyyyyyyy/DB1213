using Microsoft.AspNetCore.Mvc;

namespace DBApi.DTOs.Requests
{
    public class GetPostByIdRequest
    {
        [FromRoute]
        public Guid id { get; set; }
    }
}

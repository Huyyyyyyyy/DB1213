using Microsoft.AspNetCore.Mvc;

namespace DBApi.DTOs.Requests
{
    public class DeletePostRequest
    {
        [FromRoute]
        public Guid id { get; set; }
    }
}

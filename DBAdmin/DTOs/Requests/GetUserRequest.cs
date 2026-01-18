using Microsoft.AspNetCore.Mvc;

namespace DBAdmin.DTOs.Requests
{
    public class GetUserRequest
    {
        [FromRoute]
        public Guid user_id { get; set; }
    }
}

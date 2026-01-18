using Microsoft.AspNetCore.Mvc;

namespace DBAdmin.DTOs.Requests
{
    public class DailyClaimRequest
    {
        [FromBody]
        public Guid user_id { get; set; }
    }
}

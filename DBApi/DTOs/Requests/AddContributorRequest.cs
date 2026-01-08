using Microsoft.AspNetCore.Mvc;

namespace DBApi.DTOs.Requests
{
    public class AddContributorRequest
    {
        [FromForm]
        public required string cont_username { get; set; } = string.Empty;
        [FromForm]
        public string? cont_nickname { get; set; } = null;
        [FromForm]
        public string? cont_x_link { get; set; } = null;
        [FromForm]
        public string? cont_image_url { get; set; } = null;
        [FromForm]
        public string? cont_note { get; set; } = null;
        [FromForm]
        public string? cont_wallet_address { get; set; } = null;
    }
}

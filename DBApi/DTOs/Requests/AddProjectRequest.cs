using Microsoft.AspNetCore.Mvc;

namespace DBApi.DTOs.Requests
{
    public class AddProjectRequest
    {
        [FromForm]
        public string proj_name {get; set;} = string.Empty;
        [FromForm]
        public string proj_type {get; set;} = string.Empty;
        [FromForm]
        public string proj_img {get; set;} = string.Empty;
        [FromForm]
        public string proj_official_site {get; set;} = string.Empty;
        [FromForm]
        public string proj_other_site { get; set; } = string.Empty;
    }
}

using Microsoft.AspNetCore.Mvc;

namespace DBApi.DTOs.Requests
{
    public class GetPostRequest
    {
        [FromQuery]
        public int page { get; set; } = 1;
        [FromQuery]
        public int limit { get; set; } = 10;
    }   
}
